using System;
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
                DeleteUser(_user.Id);
                _user = null;
            }
            _client.Dispose();
        }

        [Test]
        public void CreateUserTest()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            Authorization.TokenAuthorization(_request, Token);

            _user = new User { Name = "Lizy", Gender = "Female", Email = "Lizy@mail.ru", Status = "Active" };
            _request.Content = JsonParser.SerializeUser(_user);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);
            _user.Id = jsonRootObject.Data.Id;

            Assert.AreEqual(201, jsonRootObject.Code);
        }

        [Test]
        public void UpdateUserTest()
        {
            _user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Put, string.Format(EndPoints.UserById, _user.Id));
            Authorization.TokenAuthorization(_request, Token);

            var inputNewStatus = "Inactive";
            var updatedUser = new User() { Name = _user.Name, Gender = _user.Gender, Email = _user.Email, Status = inputNewStatus };
            _request.Content = JsonParser.SerializeUser(updatedUser);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);

            Assert.AreEqual(200, jsonRootObject.Code);
        }

        [Test]
        public void DeleteUserTest()
        {
            _user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, _user.Id));
            Authorization.TokenAuthorization(_request, Token);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);

            Assert.AreEqual(204, jsonRootObject.Code);
        }

        [Test]
        public void GetUserByIdTest()
        {
            _user = CreateUser();
            _httpResponse = _client.GetAsync(string.Format(EndPoints.UserById, _user.Id));
            var responseData = _httpResponse.Result.Content.ReadAsStringAsync().Result;

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);
            Console.WriteLine(responseData);

            Assert.AreEqual(200, jsonRootObject.Code);
        }

        [Test]
        public void GetUserByFakeIdTest()
        {
            var id = 1234567890;
            _httpResponse = _client.GetAsync(string.Format(EndPoints.UserById, id));
            var responseData = _httpResponse.Result.Content.ReadAsStringAsync().Result;

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);
            Console.WriteLine(responseData);

            Assert.AreEqual(404, jsonRootObject.Code);
        }

        [Test]
        public void CreateUserWithoutTokenTest()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);

            _user = new User { Name = "Vika", Gender = "Female", Email = "Vika@mail.ru", Status = "Active" };
            _request.Content = JsonParser.SerializeUser(_user);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);
            _user.Id = jsonRootObject.Data.Id;

            Assert.AreEqual(401, jsonRootObject.Code);
        }

        private User CreateUser()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            Authorization.TokenAuthorization(_request, Token);

            _user = new User { Name = "John", Gender = "Female", Email = "John@mail.ru", Status = "Active" };
            _request.Content = JsonParser.SerializeUser(_user);
            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject = JsonParser.DeserializeHttpResponse(_httpResponse);

            return jsonRootObject.Data;
        }

        private void DeleteUser(int id)
        {
            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            Authorization.TokenAuthorization(_request, Token);
            _httpResponse = _client.SendAsync(_request);

            JsonParser.DeserializeHttpResponse(_httpResponse);
        }
    }
}