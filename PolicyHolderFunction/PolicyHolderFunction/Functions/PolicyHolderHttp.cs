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

            var created = await repo.AddPolicyHolderAsync(policyHolder);
            var res = req.CreateResponse(HttpStatusCode.Created);
            await res.WriteAsJsonAsync(created);
            return res;
        }

    }
}
