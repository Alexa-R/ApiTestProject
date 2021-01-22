using System.Net.Http;
using System.Threading.Tasks;
using ApiTestProject.Model;

namespace ApiTestProject.Helpers
{
    public static class ActionsOnUser
    {
        public static JsonRootObjectWithOneUser CreateUser(HttpRequestMessage request, string token, User user, Task<HttpResponseMessage> httpResponse, HttpClient client)
        {
            request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            Authorization.TokenAuthorization(request, token);

            user = new User { Name = "Amela", Gender = "Female", Email = "Amela@mail.ru", Status = "Active" };
            request.Content = JsonParser.SerializeUser(user);
            httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(httpResponse);

            return jsonRootObject;
        }

        public static int GetUserById(Task<HttpResponseMessage> httpResponse, HttpClient client, int id)
        {
            httpResponse = client.GetAsync(string.Format(EndPoints.UserById, id));
            var jsonRootObject = JsonParser.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }

        public static int DeleteUser(HttpRequestMessage request, string token, Task<HttpResponseMessage> httpResponse, HttpClient client, int id)
        {
            request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            Authorization.TokenAuthorization(request, token);
            httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }
    }
}