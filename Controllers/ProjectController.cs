using AzDoMVCApp.Models;
using AzDoMVCApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace AzDoMVCApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AzDoService _service;
       
        public ProjectController(ILogger<HomeController> logger, IConfiguration configuration, AzDoService service)
        { 
            _logger = logger;
            _configuration = configuration;
            _service = service;
        }

        // GET: ProjectController
        public async Task<ActionResult> Index()
        {

            Wave wave = new Wave();
            wave.PopulateWaves();
           return View(wave);
        }

        [HttpPost]
        public async Task<ActionResult> Index(Wave wave)
        {
            List<ProjectValue> projects = await _service.GetProjectsByWave(wave.WaveValue);
            ViewBag.Projects = projects;

            return View("Index", new Wave());


        }
     

         


    }
} 


