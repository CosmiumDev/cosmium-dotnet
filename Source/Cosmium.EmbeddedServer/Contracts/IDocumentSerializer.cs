using System.IO;

namespace Cosmium.EmbeddedServer.Contracts
{
    public interface IDocumentSerializer
    {
        T FromStream<T>(Stream stream);
        Stream ToStream<T>(T input);
    }
}
