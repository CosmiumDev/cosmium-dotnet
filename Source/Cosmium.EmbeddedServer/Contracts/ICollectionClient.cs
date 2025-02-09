using System.Collections.Generic;

namespace Cosmium.EmbeddedServer.Contracts
{
    public interface ICollectionClient
    {
        IEnumerable<T> GetAll<T>() where T : class;
        T GetById<T>(string id) where T : class;
        bool UpdateDocument<T>(string id, T document) where T : class;
        bool CreateDocument<T>(string id, T document) where T : class;
        bool DeleteDocument(string id);
    }
}