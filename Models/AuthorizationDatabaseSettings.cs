namespace AuthorizationApi.Models
{
    public class AuthorizationDatabaseSettings : IAuthorizationDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string UsersTokenCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IAuthorizationDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string UsersTokenCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}