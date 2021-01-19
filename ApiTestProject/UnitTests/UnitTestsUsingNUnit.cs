using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiTestProject.Helpers;
using ApiTestProject.Model;
using Newtonsoft.Json;
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
            }
            _client.Dispose();
        }

        //[Test]
        //[TestCase("Sanchas", "Female", "Sanchas@mail.ru", "Active")]
        public void CreateUserTest(string inputName, string inputGender, string inputEmail, string inputStatus)
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            Authorization.TokenAuthorization(_request, Token);

            _user = new User {Name = inputName, Gender = inputGender, Email = inputEmail, Status = inputStatus};
            _request.Content = JsonParser.SerializeUser(_user);

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(201, jsonRootObject.Code);

            DeleteUser(jsonRootObject.Data.Id);
        }

        [Test]
        public void UpdateUserTest()
        {
            _user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Put, string.Format(EndPoints.UserById, _user.Id));
            Authorization.TokenAuthorization(_request, Token);

            var inputNewStatus = "Inactive";

            var updatedUser = new User() {Name = _user.Name, Gender = _user.Gender, Email = _user.Email, Status = inputNewStatus};
            _request.Content = JsonParser.SerializeUser(_user);

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(200, jsonRootObject.Code);

            DeleteUser(_user.Id);
        }

        [Test]
        public void DeleteUserTest()
        {
            _user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, _user.Id));
            Authorization.TokenAuthorization(_request, Token);

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(204, jsonRootObject.Code);
        }

        [Test]
        public void GetUserByIdTest()
        {
            _user = CreateUser();

            _httpResponse = _client.GetAsync(string.Format(EndPoints.UserById, _user.Id));
            var responseData = _httpResponse.Result.Content.ReadAsStringAsync().Result;

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Console.WriteLine(responseData);

            Assert.AreEqual(200, jsonRootObject.Code);

            DeleteUser(_user.Id);
        }

        [Test]
        public void GetUserByFakeIdTest()
        {
            var id = 1234567890;

            _httpResponse = _client.GetAsync(string.Format(EndPoints.UserById, id));
            var responseData = _httpResponse.Result.Content.ReadAsStringAsync().Result;

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Console.WriteLine(responseData);

            Assert.AreEqual(404, jsonRootObject.Code);
        }

        [Test]
        public void CreateUserWithoutTokenTest()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);

            var inputName = "Vika";
            var inputGender = "Female";
            var inputEmail = "Vika@mail.ru";
            var inputStatus = "Active";

            _user = new User {Name = inputName, Gender = inputGender, Email = inputEmail, Status = inputStatus};
            _request.Content = JsonParser.SerializeUser(_user);

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(401, jsonRootObject.Code);

            DeleteUser(jsonRootObject.Data.Id);
        }

        private User CreateUser()
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            Authorization.TokenAuthorization(_request, Token);

            _user = new User {Name = "Alexandra", Gender = "Female", Email = "Alexandra@mail.ru", Status = "Active"};
            _request.Content = JsonParser.SerializeUser(_user);

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            return jsonRootObject.Data;
        }

        private void DeleteUser(int id)
        {
            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            Authorization.TokenAuthorization(_request, Token);

            _httpResponse = _client.SendAsync(_request);
            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);
        }
    }
}