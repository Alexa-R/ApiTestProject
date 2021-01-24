using System.Net.Http;
using System.Threading.Tasks;
using ApiTestProject.Model;

namespace ApiTestProject.Helpers
{
    public static class ActionsOnUserHelper
    {
        public static JsonRootObjectWithOneUser CreateUser(HttpRequestMessage request, string token, User user, Task<HttpResponseMessage> httpResponse, HttpClient client)
        {
            request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            AuthorizationHelper.TokenAuthorization(request, token);

            user = new User { Name = "Amela", Gender = "Female", Email = "Amela@mail.ru", Status = "Active" };
            request.Content = JsonParserHelper.SerializeUser(user);
            httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject;
        }

        public static int GetUserById(Task<HttpResponseMessage> httpResponse, HttpClient client, int id)
        {
            httpResponse = client.GetAsync(string.Format(EndPoints.UserById, id));
            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }

        public static int DeleteUser(HttpRequestMessage request, string token, Task<HttpResponseMessage> httpResponse, HttpClient client, int id)
        {
            request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            AuthorizationHelper.TokenAuthorization(request, token);
            httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }
    }
}