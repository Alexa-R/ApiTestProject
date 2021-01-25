using System.Net.Http;
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
            if (ActionsOnUserHelper.GetUserById(_client, _user.Id) == 200)
            {
                ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);
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
            _user = new User { Id = 1234567890 };
            var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);

            Assert.AreEqual(404, statusCode);
        }

        [Test]
        public void CreateUserWithoutTokenTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUserWithoutToken(_client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(401, jsonRootObject.Code);
        }
    }
}