using Newtonsoft.Json;

namespace ApiTestProject.Model
{
    public class JsonRootObjectWithOneUser
    {
        [JsonProperty(PropertyName = "code")] 
        public int Code { get; set; }

        [JsonProperty(PropertyName = "meta")] 
        public Meta Meta { get; set; }

        [JsonProperty(PropertyName = "data")] 
        public User Data { get; set; }
    }
}