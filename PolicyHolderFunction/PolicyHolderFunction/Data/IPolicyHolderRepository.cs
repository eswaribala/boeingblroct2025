using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public interface IPolicyHolderRepository
    {
        Task<bool> AddPolicyHolderAsync(Models.PolicyHolder policyHolder);
        Task<Models.PolicyHolder> GetPolicyHolderAsync(string policyNo);
        Task<IEnumerable<Models.PolicyHolder>> GetAllPolicyHoldersAsync();
        Task<bool> UpdatePolicyHolderAsync(Models.PolicyHolder policyHolder);
        Task<bool> DeletePolicyHolderAsync(string policyNo);
    }
}
