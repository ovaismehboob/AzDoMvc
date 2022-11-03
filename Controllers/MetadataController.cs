using AzDoMVCApp.Models;
using AzDoMVCApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzDoMVCApp.Extensions;
using System.Transactions;

namespace AzDoMVCApp.Controllers
{
    public class MetadataController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AzDoService _service;

        public MetadataController(ILogger<HomeController> logger, IConfiguration configuration, AzDoService service)
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
        }

        // GET: MetadataController
        public ActionResult Index() 
        {
            List<ProjectValue> projects = _service.GetAllProjectsAsync().Result;

            Project project = new Project();
            project.value = projects;
            return View(project);
        }

        // GET: MetadataController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        

        public ActionResult GetWavesByProject(string projectId)
        {
            try 
            {
                var lst= _service.GetProjectWaves(projectId).Result;

          

                // var lst = _service.GetProjectsTags(projectId).Result;
                Wave wave = new Wave();
                wave.PopulateWaves();

                foreach (var wv in wave.WavesList)
                {
                    foreach (var wvprj in lst)
                    {
                        if (wv.Value == wvprj.value)
                        {
                            wv.Selected = true;
                            break;
                        }
                    }

                }

                TempData.Put<String>("Project", projectId);
                TempData.Put<Wave>("Wave", wave);

                return View(lst);
             }
            catch(Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Update(List<string> rows)
        {
            try
            {

                //To do to update the Metadata for Waves.
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
