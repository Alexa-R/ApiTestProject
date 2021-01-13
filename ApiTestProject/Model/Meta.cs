using Newtonsoft.Json;

namespace ApiTestProject.Model
{
    public class Meta
    {
        [JsonProperty(PropertyName = "pagination")]
        public Pagination Pagination { get; set; }
    }
}