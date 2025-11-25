//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductTestFeature;
//using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;

//namespace VietausWebAPI.WebAPI.Controllers.v1.Labs
//{
//    [ApiController]
//    [Route("api/ProductTest")]
//    [AllowAnonymous]
//    public class ProductTestController : Controller
//    {
//        private readonly IProductTestService _productTestService;

//        public ProductTestController(IProductTestService productTestService)
//        {
//            _productTestService = productTestService;
//        }

//        [HttpGet("GetPaged")]
//        public async Task<IActionResult> GetProductTestPaged([FromQuery] ProductTestQuery? query)
//        {
//            var result = await _productTestService.GetAllAsync(query);
//            return Ok(result);
//        }

//        [HttpGet("GetById/{ExternalId}")]
//        public async Task<IActionResult> GetProductTestById(string ExternalId)
//        {
//            var result = await _productTestService.GetPagedByIdAsync(ExternalId);
//            return Ok(result);
//        }
//    }
//}
