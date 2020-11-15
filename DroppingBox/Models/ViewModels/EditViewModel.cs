using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DroppingBox.Models.ViewModels
{
    public class EditViewModel
    {
        [Required]
        [Display(Name = "File")]
        public string FileId { get; set; }
        public string FileLink { get; set; }
        public string Comment { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
