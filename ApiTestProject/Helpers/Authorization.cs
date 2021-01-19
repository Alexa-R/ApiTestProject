﻿using System.Net.Http;

namespace ApiTestProject.Helpers
{
    public static class Authorization
    {
        public static void TokenAuthorization(HttpRequestMessage request, string token)
        {
            request.Headers.Add("Authorization", token);
        }
    }
}