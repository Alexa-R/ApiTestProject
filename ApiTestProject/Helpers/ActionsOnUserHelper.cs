using System.Net.Http;
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
        public static JsonRootObjectWithOneUser CreateUserWithoutToken(HttpClient client)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);

            var user = new User { Name = "Vika", Gender = "Female", Email = "Vika@mail.ru", Status = "Active" };
            request.Content = JsonParserHelper.SerializeUser(user);
            var httpResponse = client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

            return jsonRootObject;
        }

        public static JsonRootObjectWithOneUser UpdateUser(string token, HttpClient client)
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(token, client);
            var user = jsonRootObject.Data;

            var request = new HttpRequestMessage(HttpMethod.Put, string.Format(EndPoints.UserById, user.Id));
            AuthorizationHelper.TokenAuthorization(request, token);

            var inputNewStatus = "Inactive";
            var updatedUser = new User() { Name = user.Name, Gender = user.Gender, Email = user.Email, Status = inputNewStatus };
            request.Content = JsonParserHelper.SerializeUser(updatedUser);
            var httpResponse = client.SendAsync(request);

            jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);

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