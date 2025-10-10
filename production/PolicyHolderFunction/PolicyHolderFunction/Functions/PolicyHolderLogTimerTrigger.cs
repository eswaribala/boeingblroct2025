using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PolicyHolderFunction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Functions
{
    public class PolicyHolderLogTimerTrigger
    {
        private readonly ILogger<PolicyHolderLogTimerTrigger> _logger;
        private readonly IPolicyHolderRepository _policyHolderRepository;
        public PolicyHolderLogTimerTrigger(ILogger<PolicyHolderLogTimerTrigger> logger, IPolicyHolderRepository policyHolderRepository)
        {
            _logger = logger;
            _policyHolderRepository = policyHolderRepository;
        }
        // Every 1 minute
        [Function("PolicyHolderLogTimerTrigger")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            try
            {
                var policyHolders = await _policyHolderRepository.ListOpenAsync();
                _logger.LogInformation($"Retrieved {policyHolders.Count} policy holders.");
                foreach (var policyHolder in policyHolders)
                {
                    _logger.LogInformation($"PolicyHolder ID: {policyHolder.Id}, Name: {policyHolder.FirstName}, Email: {policyHolder.Email}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving policy holders: {ex.Message}");

            }
        }
    }
}
