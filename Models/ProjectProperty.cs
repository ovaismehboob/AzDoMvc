namespace AzDoMVCApp.Models
{

    public class Root 
    {
        public int count { get; set; }
        public List<ProjectProperty> value { get; set; }
    }

    public class ProjectProperty
    {
        public string name { get; set; }
        public string value { get; set; }
    }



}
 