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
                var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);
                if (statusCode == 200)
                {
                    ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);
                }

                _user = null;
            }

            _client.Dispose();
        }

        [Test]
        public void CreateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(201, jsonRootObject.Code);
        }

        [Test]
        public void UpdateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.UpdateUser(Token, _client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(200, jsonRootObject.Code);
        }

        [Test]
        public void DeleteUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);

            Assert.AreEqual(204, statusCode);
        }

        [Test]
        public void GetUserByIdTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);

            Assert.AreEqual(200, statusCode);
        }

        [Test]
        public void GetUserByFakeIdTest()
        {
            var id = 1234567890;
            var statusCode = ActionsOnUserHelper.GetUserById(_client, id);

            Assert.AreEqual(404, statusCode);
        }

        [Test]
        public void CreateUserWithoutTokenTest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);

            _user = new User { Name = "Vika", Gender = "Female", Email = "Vika@mail.ru", Status = "Active" };
            request.Content = JsonParserHelper.SerializeUser(_user);
            var httpResponse = _client.SendAsync(request);

            var jsonRootObject = JsonParserHelper.DeserializeHttpResponse(httpResponse);
            _user.Id = jsonRootObject.Data.Id;

            Assert.AreEqual(401, jsonRootObject.Code);
        }
    }
}