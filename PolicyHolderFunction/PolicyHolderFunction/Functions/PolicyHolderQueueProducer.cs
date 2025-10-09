using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Text.Json;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderQueueProducer
    {
        private readonly ILogger<PolicyHolderQueueProducer> _logger;
        private readonly IPolicyHolderRepository repo;


        public PolicyHolderQueueProducer(ILogger<PolicyHolderQueueProducer> logger, IPolicyHolderRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        [Function("AddToQueue")]
        [OpenApiOperation(operationId: "GetPolicyholder", tags: new[] { "PolicyHolder" })]
        [OpenApiParameter(name: "id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "id")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PolicyHolder))]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
        public async Task<HttpResponseData> Get(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "policyholders/{id}")] HttpRequestData req, string id)
        {
            var policyHolderInstance = await repo.GetPolicyHolderAsync(id);
            if (policyHolderInstance is null) return req.CreateResponse(HttpStatusCode.NotFound);

            //publishing to queue
            

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteAsJsonAsync(policyHolderInstance);
            return res;
        }
    }
}

        

    

