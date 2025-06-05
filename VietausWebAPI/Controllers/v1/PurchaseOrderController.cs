using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using VietausWebAPI.Core.DTO;
using VietausWebAPI.WebAPI.DatabaseContext;
using VietausWebAPI.WebAPI.Helpers;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseOrderController (ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //[HttpPost("generate-pdf")]
        //public IActionResult GeneratePdf([FromBody] PurchaseOrderModel model)
        //{
        //    try
        //    {
        //        if (model == null)
        //            return BadRequest("Model is null");

        //        var document = new PurchaseOrderPdf(model);
        //        var pdfBytes = document.GeneratePdf();
        //        return File(pdfBytes, "application/pdf", $"PO_{model.POCode ?? "Unknown"}.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"PDF generation failed: {ex.Message}");
        //    }
        //}

        // {890dc05f-50ec-4b51-ac69-2f85a24ea1ad}
        [HttpGet("generate-pdf")]
        public async Task<IActionResult> GeneratePdf([FromQuery] Guid id)
        {
            try
            {
                // Lấy dữ liệu từ database
                var po = await _context.PurchaseOrdersMaterialData
                    .Include(p => p.PurchaseOrderDetailsMaterialData)
                    .ThenInclude(p => p.Material)
                    .Include(p => p.Employee)
                    .Include(p => p.Supplier)
                    .FirstOrDefaultAsync(p => p.Poid == id);

                if (po == null) 
                {
                    return BadRequest("False");
                }



                var model = _mapper.Map<PurchaseOrderModel>(po);

                if (model == null)
                    return BadRequest("Model is null");

                var document = new PurchaseOrderPdf(model);
                var pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf", $"PO_{model.POCode ?? "Unknown"}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"PDF generation failed: {ex.Message}");
            }
        }
    }
}
