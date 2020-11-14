using Microsoft.AspNetCore.Http;
using Microsoft.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DroppingBox.Models.ViewModels
{
    public class UploadModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        public string Comment { get; set; }

        public string ReturnUrl { get; set; } = "/";

    }
}
