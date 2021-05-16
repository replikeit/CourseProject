using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CourseProject.App.Web.Models
{
    public class SongViewModel
    {
        [Required]
        public string Tittle { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
    }
}