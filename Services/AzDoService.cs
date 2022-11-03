using AzDoMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzDoMVCApp.Services
{
    public class AzDoService
    {

        IConfiguration _configuration;
        public AzDoService(IConfiguration configuration)
        {
            _configuration = configuration; 
        }


        public async Task<List<ProjectValue>> GetProjectsTags(string projectId)
        {

            List<ProjectValue> proj = new List<ProjectValue>();
            try
            {
                string organization = _configuration["Organization"];
                var personalaccesstoken = _configuration["PAT"];

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/" + organization + "/"+projectId+ "/_apis/wit/tags?api-version=6.0-preview.1").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return proj;


        }

        public async Task<List<ProjectValue>> GetAllProjectsAsync()
        {

            List<ProjectValue> proj = new List<ProjectValue>();
            try
            {
                string organization = _configuration["Organization"];
                var personalaccesstoken = _configuration["PAT"];

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/" + organization + "/_apis/projects?api-version=6.0&$top=1000").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        proj = (JsonConvert.DeserializeObject<Project>(responseBody)).value;
                    }
                }

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return proj;


        }


        public async Task<List<ProjectValue>> GetProjectsByWave(string wave)
        {

            List<ProjectValue> proj = new List<ProjectValue>();

            List<ProjectValue> returnLst = new List<ProjectValue>();

            try
            {
                string organization = _configuration["Organization"];
                var personalaccesstoken = _configuration["PAT"];

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/" + organization + "/_apis/projects?api-version=6.0&$top=1000").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        proj = (JsonConvert.DeserializeObject<Project>(responseBody)).value;
                    }
                }

                foreach (var prj in proj)
                {
                    if (CheckIfWaveExistAsync(prj.id, wave).Result)
                    {
                       returnLst.Add(prj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return returnLst;
  

        }


        private async Task<bool> CheckIfWaveExistAsync(string projectId, string waveValue)
        {
            string organization = _configuration["Organization"];
            var personalaccesstoken = _configuration["PAT"];
            bool exist = false;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalaccesstoken))));

                using (HttpResponseMessage response = client.GetAsync(
                            "https://dev.azure.com/" + organization + "/_apis/projects/" + projectId + "/properties?keys="+waveValue+"&api-version=6.0-preview.1").Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var root = (JsonConvert.DeserializeObject<Root>(responseBody)).value;

                    if (root.Count > 0)
                    {
                        exist = true;
                    }
                }
            }
            return exist;

        }

        public async Task AddMetataProperties(string projectId, string key, string value)
        {
            string organization = _configuration["Organization"];
            var personalaccesstoken = _configuration["PAT"]; 
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalaccesstoken))));

                var httpVerb = new HttpMethod("PATCH");
                var httpRequestMessage = new HttpRequestMessage(httpVerb, "https://dev.azure.com/" + organization + "/_apis/projects/" + projectId + "/properties?api-version=6.0-preview.1")
                {
                    Content = new StringContent("[{'op': 'add','path':'/" + key + "','value':'" + value + "'}]", Encoding.UTF8, "application/json-patch+json")
                };

                try
                {
                    var response = await client.SendAsync(httpRequestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseCode = response.StatusCode;
                        var responseJson = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Unexpected http response {responseCode}: {responseJson}");

                    }
                  
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error patching ", exception);
                }
            }
        }

        public async Task<List<ProjectProperty>> GetProjectWaves(string projectId)
        {
            string organization = _configuration["Organization"];
            var personalaccesstoken = _configuration["PAT"];
            List<ProjectProperty> lstWave = new List<ProjectProperty>();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalaccesstoken))));

                var httpVerb = new HttpMethod("GET");
                var httpRequestMessage = new HttpRequestMessage(httpVerb, "https://dev.azure.com/" + organization + "/_apis/projects/" + projectId + "/properties?api-version=6.0-preview.1");

                using (HttpResponseMessage response = client.GetAsync(
                             "https://dev.azure.com/" + organization + "/_apis/projects/" + projectId + "/properties?api-version=6.0-preview.1").Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var root = (JsonConvert.DeserializeObject<Root>(responseBody)).value;

                    if (root.Count > 0)
                    {
                     
                        foreach (var item in root)
                        {
                            if (item.name.Contains("Microsoft.TeamFoundation.Project.Tag"))
                            {
                                var name = item.name;
                                name = name.Replace("Microsoft.TeamFoundation.Project.Tag.", "");
                                lstWave.Add(new ProjectProperty {  name=name, value=name});
                            }
                        }

                    }
                }
            }
            return lstWave;
        }

    }

}
