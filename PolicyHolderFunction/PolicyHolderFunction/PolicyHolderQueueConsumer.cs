using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderQueueConsumer
    {
        private readonly ILogger<PolicyHolderQueueConsumer> _logger;
        public PolicyHolderQueueConsumer(ILogger<PolicyHolderQueueConsumer> logger)
        {
            _logger = logger;
        }
        
        [Function("ProcessPolicyQueue")]
        public void Run(
          [QueueTrigger("policyholderqueue", Connection = "AzureWebJobsStorage")] string queueMessage)
        {
            _logger.LogInformation("Queue message received at: {time}", DateTime.UtcNow);
            _logger.LogInformation("Raw message: {msg}", queueMessage);

            try
            {
                //var policy = JsonSerializer.Deserialize<PolicyHolder>(queueMessage);
               // _logger.LogInformation("Processed Policy: {PolicyNumber}, Name: {Name}",
                 //   policy.PolicyNo, policy.FirstName);
                 _logger.LogInformation("Processed message: {msg}", queueMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing message.");
            }
        }
        


    }
}
