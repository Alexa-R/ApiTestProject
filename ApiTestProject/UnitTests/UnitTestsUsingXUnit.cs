using System;
using System.Net.Http;
using ApiTestProject.Helpers;
using ApiTestProject.Model;
using Xunit;

namespace ApiTestProject.UnitTests
{
    public class UnitTestsUsingXUnit : IDisposable
    {
        private HttpClient _client;
        private const string Token = "Bearer 58bf693b4ba9cf357c3b6b8a0744bc2f9edd9a42cd8228f3c4d0ddd64c42f6ba";
        private User _user;

        public UnitTestsUsingXUnit()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            if (ActionsOnUserHelper.GetUserById(_client, _user.Id) == 200)
            {
                ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);
            }

            _client.Dispose();
        }

        [Fact]
        public void CreateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            Assert.Equal(201, jsonRootObject.Code);
        }

        [Fact]
        public void UpdateUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.UpdateUser(Token, _client);
            _user = jsonRootObject.Data;

            Assert.Equal(200, jsonRootObject.Code);
        }

        [Fact]
        public void DeleteUserTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.DeleteUser(Token, _client, _user.Id);

            Assert.Equal(204, statusCode);
        }

        [Fact]
        public void GetUserByIdTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUser(Token, _client);
            _user = jsonRootObject.Data;

            var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);

            Assert.Equal(200, statusCode);
        }

        [Fact]
        public void GetUserByFakeIdTest()
        {
            _user = new User { Id = 1234567890 };
            var statusCode = ActionsOnUserHelper.GetUserById(_client, _user.Id);

            Assert.Equal(404, statusCode);
        }

        [Fact]
        public void CreateUserWithoutTokenTest()
        {
            var jsonRootObject = ActionsOnUserHelper.CreateUserWithoutToken(_client);
            _user = jsonRootObject.Data;

            Assert.Equal(401, jsonRootObject.Code);
        }
    }
}