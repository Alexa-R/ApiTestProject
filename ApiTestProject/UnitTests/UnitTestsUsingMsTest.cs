using System.Net.Http;
using System.Threading.Tasks;
using ApiTestProject.Helpers;
using ApiTestProject.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiTestProject.UnitTests
{
    [TestClass]
    public class UnitTestsUsingMsTest
    {
        private HttpClient _client;
        private HttpRequestMessage _request;
        private Task<HttpResponseMessage> _httpResponse;
        private const string Token = "Bearer 58bf693b4ba9cf357c3b6b8a0744bc2f9edd9a42cd8228f3c4d0ddd64c42f6ba";
        private User _user;

        [TestInitialize]
        public void InitializeTest()
        {
            _client = new HttpClient();
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            if (_user != null)
            {
                var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);
                if (statusCode == 200)
                {
                    ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);
                }

                _user = null;
            }
            
            _client.Dispose();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(201, jsonRootObject.Code);
        }

        [TestMethod]
        public void UpdateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            _request = new HttpRequestMessage(HttpMethod.Put, string.Format(EndPoints.UserById, _user.Id));
            AuthorizationHelper.TokenAuthorization(_request, Token);

            var inputNewStatus = "Inactive";
            var updatedUser = new User() { Name = _user.Name, Gender = _user.Gender, Email = _user.Email, Status = inputNewStatus };
            _request.Content = JsonParserHelper.SerializeUser(updatedUser);
            _httpResponse = _client.SendAsync(_request);

            jsonRootObject = JsonParserHelper.DeserializeHttpResponse(_httpResponse);

            Assert.AreEqual(200, jsonRootObject.Code);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);

            Assert.AreEqual(204, statusCode);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetUserByFakeIdTest()
        {
            var id = 1234567890;
            var statusCode = ActionsOnUserHelper.GetUserById(_client, id);

            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void CreateUserWithoutTokenTest()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            
            _user = new User { Name = "Vika", Gender = "Female", Email = "Vika@mail.ru", Status = "Active" };
            _request.Content = JsonParserHelper.SerializeUser(_user);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(_httpResponse);
            _user.Id = jsonRootObject.Data.Id;

            Assert.AreEqual(401, jsonRootObject.Code);
        }
    }
}