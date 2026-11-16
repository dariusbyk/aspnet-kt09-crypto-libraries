using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CryptoLibraries.Models;
using CryptoLibraries.Services;

namespace CryptoLibraries.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Text { get; set; }

        [BindProperty]
        public string Library { get; set; } = "BouncyCastle";

        public EncryptionResult Result { get; set; }
        public string Error { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (string.IsNullOrEmpty(Text))
            {
                Error = "Введите текст.";
                return;
            }

            if (Library == "BouncyCastle")
            {
                BouncyCastleService service = new BouncyCastleService();
                Result = service.Process(Text);
            }
            else if (Library == "Libsodium")
            {
                LibsodiumService service = new LibsodiumService();
                Result = service.Process(Text);
            }
            else
            {
                Error = "Выберите библиотеку из списка.";
            }
        }
    }
}
