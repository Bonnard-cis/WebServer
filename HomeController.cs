using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebServer.Models;
using System.IO;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private String path = @"C:\Users\boncho\vsts-extension-samples-master";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/MyFiles/{*id}")]
        public IActionResult MyFiles(String id)
        {
            DirectoryInfo di = null;
            if (id != null)
            {              
                di = new DirectoryInfo(Path.Combine(path,id));          
            }
            else
            {                
                di = new DirectoryInfo(path);                                                      
            }
            
            IEnumerable<FileSystemInfo> files = di.GetFileSystemInfos().AsEnumerable();                                                   
            Document[] myFiles = new Document[files.Count()];
            int i = 0;
            foreach(FileSystemInfo fsi in files)
            {
                Document doc = new Document();
                doc.Type = (fsi is FileInfo) ? "file" : "folder";
                doc.Name = fsi.Name;
                doc.Id = id;
                myFiles[i] = doc;
                i++;
            }
            this.Response.Headers["Access-Control-Allow-Origin"] = "*";
            return Json(myFiles);            
        }

        [HttpGet("/download/{*file}")]
        public IActionResult download(String file)
        {
            return File(System.IO.File.OpenRead(Path.Combine(path,file)), "application/octet-stream");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
