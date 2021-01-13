namespace ApiTestProject
{
    public class EndPoints
    {
        public static readonly string BasePath = "https://gorest.co.in/public-api";

        public static readonly string UserAll = BasePath + "/users";
        public static readonly string UserById = BasePath + "/users/{0}";
    }
}
