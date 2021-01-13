using Newtonsoft.Json;

namespace ApiTestProject.Model
{
    public class Pagination
    {
        [JsonProperty(PropertyName = "total")] 
        public int Total { get; set; }

        [JsonProperty(PropertyName = "pages")] 
        public int Pages { get; set; }

        [JsonProperty(PropertyName = "page")] 
        public int Page { get; set; }

        [JsonProperty(PropertyName = "limit")] 
        public int Limit { get; set; }
    }
}