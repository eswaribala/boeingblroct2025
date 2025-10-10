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
            try
            {
                var resp = await _container.CreateItemAsync(policyHolder, new PartitionKey(policyHolder.Id), cancellationToken: ct);
                return resp.Resource;
            }
            catch (Microsoft.Azure.Cosmos.CosmosException ex)
            {
                // Copy to locals so the debugger isn’t calling the getter repeatedly
                string msg = ex.Message;                  // safe at runtime
                string aid = ex.ActivityId;
                int code = (int)ex.StatusCode;
                int sub = ex.SubStatusCode;
                string diag = ex.Diagnostics?.ToString();

                Console.Error.WriteLine($"Cosmos error {code}/{sub} ActivityId={aid}");
                Console.Error.WriteLine(msg);
                Console.Error.WriteLine(diag);

                // Put a breakpoint on the next line and inspect the locals 'msg', 'aid', etc.
                throw;
            }
            
            }


        public async Task<bool> DeletePolicyHolderAsync(string policyNo, CancellationToken ct = default)
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

        

        public async Task<PolicyHolder> GetPolicyHolderAsync(string id,CancellationToken ct=default)
        {
            try
            {
                var resp = await _container.ReadItemAsync<PolicyHolder>(id, new PartitionKey(id), cancellationToken: ct);
                return resp.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IReadOnlyList<PolicyHolder>> ListOpenAsync(int max = 1, CancellationToken ct = default)
        {
            // If you have a "Status" flag for "open", filter here; otherwise remove the Where.
            var query = _container
                .GetItemLinqQueryable<PolicyHolder>(
                    allowSynchronousQueryExecution: false,
                    requestOptions: new QueryRequestOptions
                    {
                        // Cross-partition queries are automatic in v3.
                        // If you want to constrain to a partition, set PartitionKey = new PartitionKey(<value>)
                        MaxItemCount = max
                    })
                //.Where(p => p.Status == "Open")        // optional business filter
               // .OrderByDescending(p => p.StartDate)     // Make sure StartDate is indexed & consistent type
                .Take(max)
                .ToFeedIterator();                       // requires Microsoft.Azure.Cosmos.Linq

            var results = new List<PolicyHolder>(Math.Min(max, 50));
            while (query.HasMoreResults && results.Count < max)
            {
                var page = await query.ReadNextAsync(ct);
                results.AddRange(page);
            }
            return results;

        }

        public async Task<PolicyHolder> UpdatePolicyHolderAsync(PolicyHolder policyHolder, CancellationToken ct=default)
        {
            // Read → apply mutation → Replace
            var existing = await GetPolicyHolderAsync(policyHolder.Id, ct);
            if (existing is null) return null;
            existing.PolicyNo=policyHolder.PolicyNo;
            existing.StartDate=policyHolder.StartDate;
            existing.EndDate=policyHolder.EndDate;
            existing.Address=policyHolder.Address;
            existing.Email=policyHolder.Email;
            existing.FirstName=policyHolder.FirstName;
            existing.LastName=policyHolder.LastName;
            

            var resp = await _container.ReplaceItemAsync(existing, existing.Id, new PartitionKey(existing.Id), cancellationToken: ct);
            return resp.Resource;

        }




    }
}
