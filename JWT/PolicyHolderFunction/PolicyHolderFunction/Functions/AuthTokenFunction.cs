using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
using System.Net;

namespace PolicyHolderFunction.Functions
{
    

    public sealed class AuthTokenFunction
    {
        private readonly IJwtIssuer _issuer;
        private readonly JwtOptions _opts;

        public AuthTokenFunction(IJwtIssuer issuer, JwtOptions opts)
        {
            _issuer = issuer; _opts = opts;
        }

        // POST /api/auth/token
        // Body (optional): { "subject":"alice", "roles":["Policy.Writer"], "scopes":["Policy.Create"] }
        public record TokenRequest(string? Subject, string[]? Roles, string[]? Scopes);

        [Function("auth-token")]
        [OpenApiOperation(operationId: "AuthToken", tags: new[] { "Authentication" },
    Summary = "Issue local JWT token", Description = "Generates a development JWT with HS256 signing.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(TokenRequest),
    Required = false, Description = "Optional subject, roles, and scopes.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
    contentType: "application/json", bodyType: typeof(object),
    Summary = "Returns JWT token", Description = "Returns { access_token, token_type }")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/token")] HttpRequestData req)
        {
            var body = await req.ReadFromJsonAsync<TokenRequest>() ?? new(null, null, null);
            var sub = string.IsNullOrWhiteSpace(body.Subject) ? "local-user" : body.Subject;

            var token = _issuer.Issue(
                subject: sub,
                roles: body.Roles,
                scopes: body.Scopes,
                lifetime: TimeSpan.FromMinutes(60)
            );

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteAsJsonAsync(new { access_token = token, token_type = "Bearer" });
            return res;
        }
    }
}
