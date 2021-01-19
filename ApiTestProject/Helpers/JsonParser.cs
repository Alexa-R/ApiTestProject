using System.Net.Http;
using System.Text;
using ApiTestProject.Model;
using Newtonsoft.Json;

namespace ApiTestProject.Helpers
{
    public static class JsonParser
    {
        public static StringContent SerializeUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            return data;
        }
    }
}