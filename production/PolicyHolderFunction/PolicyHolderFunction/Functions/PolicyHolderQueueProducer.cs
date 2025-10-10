using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
using System.Net;
using System.Text.Json;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderQueueProducer
    {
        private readonly ILogger<PolicyHolderQueueProducer> _logger;
        private readonly IPolicyHolderRepository repo;
        private readonly QueueClient _queue;

        public PolicyHolderQueueProducer(ILogger<PolicyHolderQueueProducer> logger, IPolicyHolderRepository repo,QueueClient queueClient)
        {
            _logger = logger;
            this.repo = repo;
            _queue = queueClient;
            _queue.CreateIfNotExists(); // safe if already exists
        }

        [Function("AddToQueue")]
        
        public async Task<HttpResponseData> CreateMessage(
     [HttpTrigger(AuthorizationLevel.Function, "get", Route = "policyholders/publish/{id}")] HttpRequestData req,
     string id)
        {
           // queueMessage = null; // default

            var policyHolderInstance = await repo.GetPolicyHolderAsync(id);
            if (policyHolderInstance is null)
                return req.CreateResponse(HttpStatusCode.NotFound);
            var payload = JsonSerializer.Serialize(policyHolderInstance);
            var send = await _queue.SendMessageAsync(payload);
            _logger.LogInformation("Sent. MessageId={MessageId}, PopReceipt={PopReceipt}", send.Value.MessageId, send.Value.PopReceipt);

            var res = req.CreateResponse(HttpStatusCode.OK);
            await res.WriteStringAsync($"Enqueued to: {_queue.Uri}\nMessageId: {send.Value.MessageId}");
            return res;
           
        }
    }
}

        

    

