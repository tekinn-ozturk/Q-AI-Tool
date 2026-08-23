using AiQaAssistant.Api.Parsers.Interfaces;
using UglyToad.PdfPig;

namespace AiQaAssistant.Api.Parsers
{
    public class PdfDocumentParser : IDocumentParser
    {
       

        public string SupportedExtension => ".pdf";
        public Task<string> ParseAsync(string filePath)
        {
            // pdf plugini gelen filepathi açarak bi document değişkenine atıyor.
            using var document = PdfDocument.Open(filePath);

            // boş bir text değişkeni
            var text = string.Empty;

            //foreach ile documentin tüm sayfalarını dolanıyoruz.
            foreach (var page in document.GetPages())
            {
                // bizim text = bizim text + document.page.text
                text = text + page.Text;
            }
            return Task.FromResult(text);
          

        }
    }
}
