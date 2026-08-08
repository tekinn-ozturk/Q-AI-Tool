using AiQaAssistant.Api.DTOs;

namespace AiQaAssistant.Api.Interfaces
{
    public interface IDocumentService
    {
        Task<UploadDocumentResponseDto> UploadDocumentAsync(IFormFile file);
    }
}
