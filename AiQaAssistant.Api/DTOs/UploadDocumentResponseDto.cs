namespace AiQaAssistant.Api.DTOs
{
    public class UploadDocumentResponseDto
    {
        //Bu string null olmasın Varsayılan değeri boş string olsun.
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Text { get; set; }


    }
}
