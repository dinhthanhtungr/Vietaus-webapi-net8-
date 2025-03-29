using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.Hubs;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    public class ProposalController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserConnectionService _userConnectionService;
        //private readonly IProposalService _proposalService;
    }
}
