using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IProposalService
    {
        Task CreateProposal(SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO);
    }
}
