namespace AiQaAssistant.Api.Parsers.Interfaces
{
    public interface IDocumentParser
    {
        //SupportedExtension bir davranış değil, nesnenin sahip olduğu bir bilgidir bu sebeple metot yerine properties olarak yazılır.
        string SupportedExtension { get; }

        Task<string> ParseAsync(string filePath);

        
    }
}
