using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VietausWebAPI.WebAPI.Controllers
{
    //[Route("api/v{version:apiVersion}/[Controller]")]
    //[ApiController]
    [Route("api/[Controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {

    }
}
