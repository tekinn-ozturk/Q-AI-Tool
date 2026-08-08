namespace AiQaAssistant.Api.Parsers.Interfaces
{
    public interface IDocumentParser
    {
        string SupportedExtension { get; }

        Task<string> ParseAsync(string filePath);

        // 2 projeyi aynı  git reposuna at mutlaka
    }
}
