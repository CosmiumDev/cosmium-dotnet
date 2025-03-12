using Cosmium.EmbeddedServer.Contracts;

namespace Cosmium.EmbeddedServer.Constants
{
    internal static class ResponseMessages
    {
        private const string Success = "Success";
        private const string Unknown = "Unknown";
        private const string FailedToParseConfiguration = "Failed to parse configuration";
        private const string FailedToLoadState = "Failed to load state";
        private const string FailedToParseRequest = "Failed to parse request";
        private const string ServerInstanceAlreadyExists = "Server instance already exists";
        private const string ServerInstanceNotFound = "Server instance not found";
        private const string FailedToStartServer = "Failed to start server";
        private const string CurrentDataStoreDoesNotSupportStateLoading = "Current Data Store does not support state loading";
        private const string DataStoreNotFound = "Repository not found";
        private const string DataStoreConflict = "Repository conflict";
        private const string DataStoreBadRequest = "Repository bad request";

        internal static string GetMessage(ResponseType responseType)
        {
            return responseType switch
            {
                ResponseType.Success => Success,
                ResponseType.Unknown => Unknown,
                ResponseType.FailedToParseConfiguration => FailedToParseConfiguration,
                ResponseType.FailedToLoadState => FailedToLoadState,
                ResponseType.FailedToParseRequest => FailedToParseRequest,
                ResponseType.ServerInstanceAlreadyExists => ServerInstanceAlreadyExists,
                ResponseType.ServerInstanceNotFound => ServerInstanceNotFound,
                ResponseType.FailedToStartServer => FailedToStartServer,
                ResponseType.CurrentDataStoreDoesNotSupportStateLoading => CurrentDataStoreDoesNotSupportStateLoading,
                ResponseType.DataStoreNotFound => DataStoreNotFound,
                ResponseType.DataStoreConflict => DataStoreConflict,
                ResponseType.DataStoreBadRequest => DataStoreBadRequest,
                _ => Unknown,
            };
        }
    }
}
