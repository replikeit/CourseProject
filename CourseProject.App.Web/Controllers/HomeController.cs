using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CourseProject.App.Web.Models;
using CourseProject.Core.Audio.Interfaces;
using CourseProject.Core.DataAccess;
using CourseProject.Core.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.App.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly MusicDbContext _dbContext;
        private IAudioService _audioService;

        public HomeController(IWebHostEnvironment appEnvironment, MusicDbContext dbContext, IAudioService audioService)
        {
            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
            _audioService = audioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult History()
        {
            return View(_dbContext.Songs.ToArray());
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(SongViewModel model)
        {
            if (ModelState.IsValid && model.File != null)
            {
                if (model.File.ContentType != "audio/x-wav")
                {
                    ModelState.AddModelError("", "Uploaded file is not an audio file.");
                }
                else
                {
                    try
                    {
                        string path = _appEnvironment.WebRootPath + "/files/" + model.File.FileName;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                            await model.File.CopyToAsync(fileStream);
                    
                        var song = new Song() {Tittle = model.Tittle, FilePath = path};
                        await _audioService.SongProcess(song);
                    
                        return RedirectToAction("History"); 
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
            }

            return View(model);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}