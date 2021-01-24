using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiTestProject.Model;
using Newtonsoft.Json;

namespace ApiTestProject.Helpers
{
    public static class JsonParserHelper
    {
        public static StringContent SerializeUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            return data;
        }

        public static JsonRootObjectWithOneUser DeserializeHttpResponse(Task<HttpResponseMessage> httpResponse)
        {
            var responseData = httpResponse.Result.Content.ReadAsStringAsync().Result;
            var jsonRootObject = JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(responseData);

            return jsonRootObject;
        }
    }
}