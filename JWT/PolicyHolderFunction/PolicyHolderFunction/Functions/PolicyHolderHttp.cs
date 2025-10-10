using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    // OpenApiSecurityLocationType
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderHttp
    {

        private readonly ILogger<PolicyHolderHttp> _logger;
        private readonly IPolicyHolderRepository repo;
        private readonly IJwtValidator _validator;

        public PolicyHolderHttp(ILogger<PolicyHolderHttp> logger,IPolicyHolderRepository policyHolder, IJwtValidator validator)
        {
            _logger = logger;
            repo = policyHolder;
            _validator = validator;
        }

        [Function("Create")]
        
        [OpenApiOperation(operationId: "Create", tags: new[] { "PolicyHolder" })]
        [OpenApiRequestBody("application/json", typeof(PolicyHolder), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.Created, "application/json", typeof(PolicyHolder))]
        public async Task<HttpResponseData> Create(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "policyholders")] HttpRequestData req, FunctionContext ctx)
        {
            var log = ctx.GetLogger("policyholders-create");

            // 1) Get token
            if (!req.Headers.TryGetValues("Authorization", out var authVals))
                return await UnauthorizedJsonAsync(req, "Missing Authorization header");
            var auth = authVals.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return await UnauthorizedJsonAsync(req, "Invalid Authorization header");

            ClaimsPrincipal user;
            try { user = _validator.Validate(auth["Bearer ".Length..].Trim()); }
            catch (Exception ex)
            {
                log.LogWarning(ex, "JWT validation failed");
                return await UnauthorizedJsonAsync(req, "Invalid token");
            }

            // 2) Access control: require scope Policy.Create OR role Policy.Writer
            bool hasScope = user.Claims.Any(c => c.Type == "scp" && c.Value.Split(' ').Contains("Policy.Create"));
            bool hasRole = user.Claims.Any(c => (c.Type == ClaimTypes.Role || c.Type == "roles")
                                                && c.Value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("Policy.Writer"));
            if (!hasScope && !hasRole)
            {
                var forbid = req.CreateResponse(HttpStatusCode.Forbidden);
                await forbid.WriteStringAsync("Missing scope 'Policy.Create' or role 'Policy.Writer'.");
                return forbid;
            }


            var policyHolder = await JsonSerializer.DeserializeAsync<PolicyHolder>(req.Body);
            if (policyHolder is null)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("Invalid body");
                return bad;
            }
            // Guarantee lowercase "id" is present & non-empty
            if (string.IsNullOrWhiteSpace(policyHolder.Id))
                policyHolder.Id = Guid.NewGuid().ToString("N");

            // Debug: confirm what goes to Cosmos
            //_log.LogInformation("Outgoing Cosmos doc: {json}", Newtonsoft.Json.JsonConvert.SerializeObject(doc));

            // PK is /id -> pass doc.Id
           
            var created = await repo.AddPolicyHolderAsync(policyHolder);
            var res = req.CreateResponse(HttpStatusCode.Created);
            await res.WriteAsJsonAsync(created);
            return res;
        }


        [Function("Get")]
        [OpenApiOperation(operationId: "GetPolicyholder", tags: new[] { "PolicyHolder" })]
        [OpenApiParameter(name: "id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "id")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PolicyHolder))]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
        public async Task<HttpResponseData> Get(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "policyholders/{id}")] HttpRequestData req, string id)
        {
            var claim = await repo.GetPolicyHolderAsync(id);
            if (claim is null) return req.CreateResponse(HttpStatusCode.NotFound);

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteAsJsonAsync(claim);
            return res;
        }

        // PUT /api/policyholders/{id}  (full update/upsert)
        [Function("UpdatePolicyholder")]
        [OpenApiOperation(operationId: "UpdatePolicyholder", tags: new[] { "PolicyHolder" })]
        [OpenApiParameter(name: "id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody("application/json", typeof(PolicyHolder), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PolicyHolder))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<HttpResponseData> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "policyholders")] HttpRequestData req)
        {
            var policyHolder = await JsonSerializer.DeserializeAsync<PolicyHolder>(req.Body);
            if (policyHolder is null)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("Invalid body");
                return bad;
            }
           

            var updated = await repo.UpdatePolicyHolderAsync(policyHolder);
            if (updated is null) return req.CreateResponse(HttpStatusCode.NotFound);

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteAsJsonAsync(updated);
            return res;


        }

        // DELETE /api/policyholders/{id}
        [Function("DeletePolicyholder")]
        [OpenApiOperation(operationId: "DeletePolicyholder", tags: new[] { "PolicyHolder" })]
        [OpenApiParameter(name: "id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithoutBody(HttpStatusCode.NoContent)]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "policyholders/{id}")] HttpRequestData req,
            string id)
        {
            var ok = await repo.DeletePolicyHolderAsync(id);
            return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);

        }

        [Function("ListOpenPolicyholders")]
        [OpenApiOperation(operationId: "ListOpenPolicyholders", tags: new[] { "PolicyHolder" },
                     Summary = "List open policyholders",
                     Description = "Retrieves up to `max` most recent open policyholders ordered by StartDate desc.")]
        [OpenApiParameter(name: "max", In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                     Required = false, Type = typeof(int),
                     Summary = "Maximum number of items to return (default 50)")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IEnumerable<PolicyHolder>))]
        public async Task<HttpResponseData> Run(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "policyholders")] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int.TryParse(query["max"], out var max);
            if (max <= 0) max = 50;

            var result = await repo.ListOpenAsync(max);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }

        private static async Task<HttpResponseData> UnauthorizedJsonAsync(HttpRequestData req, string msg)
        {
            var r = req.CreateResponse(HttpStatusCode.Unauthorized);
            r.Headers.Add("WWW-Authenticate",
                $"Bearer error=\"invalid_token\", error_description=\"{msg}\"");
            await r.WriteAsJsonAsync(new { error = "invalid_token", error_description = msg });
            return r;
        }
    }


}
