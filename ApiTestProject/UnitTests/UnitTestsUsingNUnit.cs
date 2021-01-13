using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        [SetUp]
        public void InitializeTest()
        {
            _client = new HttpClient();
        }

        [TearDown]
        public void CleanUpTest()
        {
            _client.Dispose();
        }

        [Test]
        [TestCase("Elena", "Female", "Elena@mail.ru", "Active")]
        public void CreateUserTest(string inputName, string inputGender, string inputEmail, string inputStatus)
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);
            _request.Headers.Add("Authorization", Token);

            var user = new User {Name = inputName, Gender = inputGender, Email = inputEmail, Status = inputStatus};

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            _request.Content = data;

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(201, jsonRootObject.Code);

            DeleteUser(jsonRootObject.Data.Id);
        }

        [Test]
        [TestCase("Inactive")]
        public void UpdateUserTest(string inputNewStatus)
        {
            var user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Put, string.Format(EndPoints.UserById, user.Id));
            _request.Headers.Add("Authorization", Token);

            var updatedUser = new User() {Name = user.Name, Gender = user.Gender, Email = user.Email, Status = inputNewStatus};
            var json = JsonConvert.SerializeObject(updatedUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            _request.Content = data;

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Assert.AreEqual(200, jsonRootObject.Code);

            DeleteUser(user.Id);
        }

        [Test]
        public void DeleteUserTest()
        {
            var user = CreateUser();

            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, user.Id));
            _request.Headers.Add("Authorization", Token);

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
            var user = CreateUser();

            _httpResponse = _client.GetAsync(string.Format(EndPoints.UserById, user.Id));
            var responseData = _httpResponse.Result.Content.ReadAsStringAsync().Result;

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            Console.WriteLine(responseData);

            Assert.AreEqual(200, jsonRootObject.Code);

            DeleteUser(user.Id);
        }

        [Test]
        [TestCase(1234567890)]
        public void GetUserByFakeIdTest(int id)
        {
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
        [TestCase("Vika", "Male", "Vika@mail.ru", "Active")]
        public void CreateUserWithoutTokenTest(string inputName, string inputGender, string inputEmail,
            string inputStatus)
        {
            _request = new HttpRequestMessage(HttpMethod.Post, EndPoints.UserAll);

            var user = new User {Name = inputName, Gender = inputGender, Email = inputEmail, Status = inputStatus};
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            _request.Content = data;

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
            _request.Headers.Add("Authorization", Token);

            var user = new User {Name = "Alexandra", Gender = "Female", Email = "Alexandra@mail.ru", Status = "Active"};
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            _request.Content = data;

            _httpResponse = _client.SendAsync(_request);

            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            return jsonRootObject.Data;
        }

        private User DeleteUser(int id)
        {
            _request = new HttpRequestMessage(HttpMethod.Delete, string.Format(EndPoints.UserById, id));
            _request.Headers.Add("Authorization", Token);

            _httpResponse = _client.SendAsync(_request);
            var jsonRootObject =
                JsonConvert.DeserializeObject<JsonRootObjectWithOneUser>(_httpResponse.Result.Content
                    .ReadAsStringAsync()
                    .Result);

            return jsonRootObject.Data;
        }
    }
}