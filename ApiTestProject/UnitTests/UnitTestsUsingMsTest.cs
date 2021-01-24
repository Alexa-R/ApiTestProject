﻿using System.Net.Http;
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
            var jsonRootObject = ActionsOnUserHelper.UpdateUser(Token, _client);
            _user = jsonRootObject.Data;

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
            var jsonRootObject = ActionsOnUserHelper.CreateUserWithoutToken(_client);
            _user = jsonRootObject.Data;

            Assert.AreEqual(401, jsonRootObject.Code);
        }
    }
}