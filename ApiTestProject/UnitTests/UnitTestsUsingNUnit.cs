using System.Net.Http;
using System.Threading.Tasks;
using ApiTestProject.Helpers;
using ApiTestProject.Model;
using NUnit.Framework;

namespace ApiTestProject.UnitTests
{
    [TestFixture]
    public class UnitTestsUsingNUnit
    {
        private HttpClient _client;
        private HttpRequestMessage _request;
        private Task<HttpResponseMessage> _httpResponse;
        private const string Token = "Bearer 58bf693b4ba9cf357c3b6b8a0744bc2f9edd9a42cd8228f3c4d0ddd64c42f6ba";
        private User _user;

        [SetUp]
        public void InitializeTest()
        {
            _client = new HttpClient();
        }

        [TearDown]
        public void CleanUpTest()
        {
            if (_user != null)
            {
                var statusCode = ActionsOnUserHelper.GetUserById(_httpResponse, _client, _user.Id);
                if (statusCode == 200)
                {
                    ActionsOnUserHelper.DeleteUser(_request, Token, _httpResponse, _client, _user.Id);
                }

                _user = null;
            }

            _client.Dispose();
        }

        [Test]
        public void CreateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(_request, Token, _user, _httpResponse, _client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(201, jsonRootObject.Code);
        }

        [Test]
        public void UpdateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(_request, Token, _user, _httpResponse, _client);
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

        [Test]
        public void DeleteUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(_request, Token, _user, _httpResponse, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.DeleteUser(_request, Token, _httpResponse, _client, _user.Id);

            Assert.AreEqual(204, statusCode);
        }

        [Test]
        public void GetUserByIdTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(_request, Token, _user, _httpResponse, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.GetUserById(_httpResponse, _client, _user.Id);

            Assert.AreEqual(200, statusCode);
        }

        [Test]
        public void GetUserByFakeIdTest()
        {
            var id = 1234567890;
            var statusCode = ActionsOnUserHelper.GetUserById(_httpResponse, _client, id);

            Assert.AreEqual(404, statusCode);
        }

        [Test]
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