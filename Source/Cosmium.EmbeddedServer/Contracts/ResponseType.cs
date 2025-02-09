namespace Cosmium.EmbeddedServer.Contracts
{
    public enum ResponseType
    {
        Success = 0,
        Unknown = 100,
        FailedToParseConfiguration = 101,
        FailedToLoadState = 102,
        FailedToParseRequest = 103,
        ServerInstanceAlreadyExists = 104,
        ServerInstanceNotFound = 105,
        FailedToStartServer = 106,
        RepositoryNotFound = 200,
        RepositoryConflict = 201,
        RepositoryBadRequest = 202,
    }
}
