using AuthorizationLab.Resources;
using System.Collections.Generic;

namespace AuthorizationLab.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<Document> Get();
        Document GetById(int id);
    }
}
