using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderHttp
    {

        private readonly ILogger<PolicyHolderHttp> _logger;
        private readonly IPolicyHolderRepository repo;

        public PolicyHolderHttp(ILogger<PolicyHolderHttp> logger,IPolicyHolderRepository policyHolder)
        {
            _logger = logger;
            repo= policyHolder;
        }
       

        [Function("Create")]
        
        public async Task<HttpResponseData> Create(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "policyholders")] HttpRequestData req)
        {
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
            _logger.LogInformation("Outgoing Cosmos doc: {json}", Newtonsoft.Json.JsonConvert.SerializeObject(policyHolder));

            // PK is /id -> pass doc.Id
           
            var created = await repo.AddPolicyHolderAsync(policyHolder);
            var res = req.CreateResponse(HttpStatusCode.Created);
            await res.WriteAsJsonAsync(created);
            return res;
        }


        [Function("Get")]
        
        public async Task<HttpResponseData> Get(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "policyholders/{id}")] HttpRequestData req, string id)
        {
            var policyHolderInstance = await repo.GetPolicyHolderAsync(id);
            if (policyHolderInstance is null) return req.CreateResponse(HttpStatusCode.NotFound);

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteAsJsonAsync(policyHolderInstance);
            return res;
        }

        // PUT /api/policyholders/{id}  (full update/upsert)
        [Function("UpdatePolicyholder")]
       
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
        
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "policyholders/{id}")] HttpRequestData req,
            string id)
        {
            var ok = await repo.DeletePolicyHolderAsync(id);
            return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);

        }

        [Function("ListOpenPolicyholders")]
        
        public async Task<HttpResponseData> Run(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "policyholders")] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int.TryParse(query["max"], out var max);
            if (max <= 0) max = 50;

            var result = await repo.ListOpenAsync(max);
            _logger.LogInformation("Fetching policyholder. Correlation={OperationId}", System.Diagnostics.Activity.Current?.RootId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }


}
