using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolicyHolderFunction.Models;
namespace PolicyHolderFunction.Data
{
    public interface IPolicyHolderRepository
    {
        Task<PolicyHolder> AddPolicyHolderAsync(PolicyHolder policyHolder);
        Task<PolicyHolder> GetPolicyHolderAsync(string policyNo);
        Task<IEnumerable<PolicyHolder>> GetAllPolicyHoldersAsync();
        Task<bool> UpdatePolicyHolderAsync(PolicyHolder policyHolder);
        Task<bool> DeletePolicyHolderAsync(string policyNo);
    }
}
