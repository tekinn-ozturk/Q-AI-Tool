using AiQaAssistant.Api.DTOs;
using AiQaAssistant.Api.Interfaces;
using AiQaAssistant.Api.Parsers.Interfaces;


namespace AiQaAssistant.Api.Services
{
    //GERÇEK İŞİN YAPILDIĞI KISIM SERVİSLERDİR
    public class DocumentService : IDocumentService
    {
        //Uygulamanın çalıştığı ortam ve klasör bilgileri için lazım olanın interface'ini (IWebHostEnvironment) inject ettik ve bir örneğini aldık.
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<IDocumentParser> _parsers;

        //constructor metod yani Document Service her çağrıldığında bu metotta çalışacak.
        public DocumentService(IWebHostEnvironment environment, IConfiguration configuration, IEnumerable<IDocumentParser> parsers)
        {
            //bu DocumentService class'ı eger bi environment ile çağrılırsa parametredeki env ile bizim yarattığımız _env 'ye eşitlememiz gerekiyor, çünki buradaki tüm işlemler _environment üzerinden dönücek çünkü _environment artık zaten bir IWebHostEnvironment örneği. eşitliyoruzz ya.

            _environment = environment;
            _configuration = configuration;
            _parsers = parsers;
        }
        public async Task<UploadDocumentResponseDto> UploadDocumentAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Dosya bulunamadı.");
            }

            //kabul edilen dosya uzantıları
            //var allowedExtensions = new[] { ".pdf", ".txt", ".docx", ".png", ".jpg", ".jpeg" };
            var allowedExtensions = _configuration.GetSection("AllowedExtensions").Get<string[]>() ?? Array.Empty<string>();




            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            //Yüklenen dosyanın uzantısı alınır. (.pdf, .jpg kısmını alır)

            //_parsers listesindeki SupportedExtension değeri bizim dosyanın uzantısıyla aynı olan ilk parser'ı bul.
            var parser = _parsers.FirstOrDefault(
             p => p.SupportedExtension == fileExtension);

            if (parser == null)
            {
                throw new ArgumentException("Bu dosya tipi için parser bulunamadı.");
            }

            // kabul edilen dosya tipi içermiyorsa alınan dosya tipini bu uyarıyı patlatır.
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Desteklenmeyen dosya tipi.");
            }


            //_environment.ContentRootPath projenin ana dizinidir. bu satırda uploads dosyasının yolunu verdik. C:\Users\PC\Desktop\AiQaAssistant\Uploads gibi bi path döndürür. Path.Combine methodu.
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");

            //Directory, .NET'te klasörlerle (directory/folder) işlem yapmak için kullanılan bir sınıftır.
            if (!Directory.Exists(uploadsFolder))
            {
                //eğer uploadfolder yoksa yaratma fonksiyonu.
                Directory.CreateDirectory(uploadsFolder);
            }

            // yüklenen dosyanın adı değiştirlir. guid eklenir ki ilerde çakışma olmasın aynı dosya isimleri olursa.
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            // yüklenen dosyanın yolu döndürülür.
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //bilgisayarın localinde bi dosya yaratıyor 
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                //ve onun içine yazıyor
                await file.CopyToAsync(stream);

            }
            var text = await parser.ParseAsync(filePath);

            //service UploadDocumentResponseDto dönmeli servis bu DTO imzalı
            return new UploadDocumentResponseDto
            {
                FileName = uniqueFileName,
                FilePath = filePath,
                FileSize = file.Length,
                Message = "Dosya başarıyla yüklendi.",
                Text = text 
            };
        }
    }
}
