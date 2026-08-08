using AiQaAssistant.Api.DTOs;
using AiQaAssistant.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiQaAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<UploadDocumentResponseDto>> UploadDocument(IFormFile file)
        {
            try
            {
                // Diyelim ki sunucuya 100 kişi istek attı senkron olsaydı çok fazla bekleme süresi olurdu isteği alırdı sonlanması beklerdi sonlanınca yeni istek alırdı. ama asyn'lik ile birlikte isteği alan thread isteği aldığı gibi başka isteğe de gidebiliyor.

                // await kullanmak için diske, veritabanına giden bir iş olması lazım ki manası olsun.
                var result = await _documentService.UploadDocumentAsync(file);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
