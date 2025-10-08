using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PolicyHolderFunction.Models;
using System.Net;



namespace PolicyHolderFunction.Data
{
    public class PolicyHolderRepository : IPolicyHolderRepository
    {
        private readonly Container _container;

        public PolicyHolderRepository(Container container)
        {
            _container = container;
        }

        public async Task<PolicyHolder> AddPolicyHolderAsync(PolicyHolder policyHolder, CancellationToken ct=default)
        {
            var resp = await _container.CreateItemAsync(policyHolder, new PartitionKey(policyHolder.PolicyNo), cancellationToken: ct);
            return resp.Resource;
        }


        public async Task<bool> DeletePolicyHolderAsync(string policyNo)
        {
            try
            {

                await _container.DeleteItemAsync<PolicyHolder>(policyNo, new PartitionKey(policyNo));
                return true;
             }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public Task<bool> DeletePolicyHolderAsync(string policyNo, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PolicyHolder> GetPolicyHolderAsync(string policyNo,CancellationToken ct=default)
        {
            try
            {
                var resp = await _container.ReadItemAsync<PolicyHolder>(policyNo, new PartitionKey(policyNo), cancellationToken: ct);
                return resp.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IReadOnlyList<PolicyHolder>> ListOpenAsync(int max = 50, CancellationToken ct = default)
        {
            var queryable =
    _container.GetItemLinqQueryable<PolicyHolder>(allowSynchronousQueryExecution: false)
             
              .OrderByDescending(p => p.StartDate)
              .Take(max);

            var it = queryable.ToFeedIterator(); // IMMEDIATE when iterated
            var results = new List<PolicyHolder>();
            while (it.HasMoreResults && results.Count < max)
            {
                var page = await it.ReadNextAsync(ct);
                results.AddRange(page);
            }
            return results;

        }

        public async Task<PolicyHolder> UpdatePolicyHolderAsync(PolicyHolder policyHolder, CancellationToken ct=default)
        {
            // Read → apply mutation → Replace
            var existing = await GetPolicyHolderAsync(policyHolder.PolicyNo, ct);
            if (existing is null) return null;
            
            var resp = await _container.ReplaceItemAsync(existing, existing.PolicyNo, new PartitionKey(existing.PolicyNo), cancellationToken: ct);
            return resp.Resource;

        }

    }
}
