using System.Net.Http;
using System.Threading.Tasks;
using ApiTestProject.Model;

namespace ApiTestProject.Helpers
{
    public static class ActionsOnUserHelper
    {
        public static JsonRootObjectWithOneUser CreateUser(string token, HttpClient client)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            AuthorizationHelper.TokenAuthorization(request, token);

            var user = new User { Name = "Amela", Gender = "Female", Email = "Amela@mail.ru", Status = "Active" };
            request.Content = JsonParserHelper.SerializeUser(user);
            var httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject;
        }

        public static int GetUserById(HttpClient client, int id)
        {
            var httpResponse = client.GetAsync(string.Format(EndPoints.UserById, id));
            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }

        public static int DeleteUser(string token, HttpClient client, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            AuthorizationHelper.TokenAuthorization(request, token);
            var httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject.Code;
        }
    }
}