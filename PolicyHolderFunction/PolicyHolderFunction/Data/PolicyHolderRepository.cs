using Microsoft.Azure.Cosmos;
using PolicyHolderFunction.Models;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;


namespace PolicyHolderFunction.Data
{
    public class PolicyHolderRepository : IPolicyHolderRepository
    {
        private readonly Container _container;

        public PolicyHolderRepository(Container container)
        {
            _container = container;
        }

        public async Task<PolicyHolder> AddPolicyHolderAsync(PolicyHolder policyHolder)
        {
            var resp = await _container.CreateItemAsync(policyHolder, new PartitionKey(claim.id), cancellationToken: ct);
            return resp.Resource;
        }


        public Task<bool> DeletePolicyHolderAsync(string policyNo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PolicyHolder>> GetAllPolicyHoldersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PolicyHolder> GetPolicyHolderAsync(string policyNo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePolicyHolderAsync(PolicyHolder policyHolder)
        {
            throw new NotImplementedException();
        }
    }
}
