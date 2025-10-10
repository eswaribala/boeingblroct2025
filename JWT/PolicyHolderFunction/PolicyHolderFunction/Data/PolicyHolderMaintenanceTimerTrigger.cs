using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public class PolicyHolderMaintenanceTimerTrigger
    {
        private readonly ILogger<PolicyHolderMaintenanceTimerTrigger> _logger;
        private readonly IPolicyHolderRepository _repo;

        public PolicyHolderMaintenanceTimerTrigger(
            ILogger<PolicyHolderMaintenanceTimerTrigger> logger,
            IPolicyHolderRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // seconds minutes hours day month day-of-week
        // "0 0 * * * *" => top of every hour
        // For quick testing locally, try "0 */1 * * * *" (every 1 minutes)
        [Function(nameof(PolicyHolderMaintenanceTimerTrigger))]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer, CancellationToken ct)
        {
            var now = DateTimeOffset.Now;
            _logger.LogInformation("Policy maintenance started at {Now}. IsPastDue={PastDue}. Next={Next}",
                now, timer.IsPastDue, timer.ScheduleStatus?.Next);

            try
            {
                var count = await _repo.ListOpenAsync(100, ct);
                    
                _logger.LogInformation("Policy maintenance finished. Closed {Count} expired policies.", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Policy maintenance failed.");
                throw; // Let Functions platform observe the failure
            }
        }
    }
}
