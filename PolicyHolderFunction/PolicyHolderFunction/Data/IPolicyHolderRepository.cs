using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace PolicyHolderFunction.Data
{
    public interface IPolicyHolderRepository
    {
        Task<PolicyHolder> AddPolicyHolderAsync(PolicyHolder policyHolder, CancellationToken ct=default);
        Task<PolicyHolder> GetPolicyHolderAsync(string policyNo, CancellationToken ct = default);
        Task<IReadOnlyList<PolicyHolder>> ListOpenAsync(int max = 50, CancellationToken ct = default);
        Task<bool> UpdatePolicyHolderAsync(PolicyHolder policyHolder, CancellationToken ct = default);
        Task<bool> DeletePolicyHolderAsync(string policyNo, CancellationToken ct = default);
    }
}
