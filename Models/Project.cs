
using System.Security.Policy;

namespace AzDoMVCApp.Models
{
    public class ProjectValue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }

        
    }

    public class Project
    {

        public string ProjectId { get; set; }
        public int count { get; set; }
        public List<ProjectValue> value { get; set; }

        public List<ProjectProperty> ProjectTags { get; set; }
    }

    public class TeamValue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string identityUrl { get; set; }
        public string projectName { get; set; }
        public string projectId { get; set; }
    }

    public class Team
    {
        public List<TeamValue> value { get; set; }
        public int count { get; set; }
    }


    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
    }

    public class Identity
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Value
    {
        public Identity identity { get; set; }
    }

    public class TeamMember
    {
        public List<Value> value { get; set; }
        public int count { get; set; }
    }





}
