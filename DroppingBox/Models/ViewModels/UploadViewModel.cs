using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DroppingBox.Models.ViewModels
{
    public class UploadViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
        public string Comment { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
