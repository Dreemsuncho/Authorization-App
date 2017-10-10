using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationLab.Resources;

namespace AuthorizationLab.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private List<Document> _documents = new List<Document>
        {
            new Document { Id = 1, Author ="Elvis" },
            new Document { Id = 2, Author ="Go6e" }
        };

        public IEnumerable<Document> Get()
        {
            return _documents;
        }

        public Document GetById(int id)
        {
            return _documents.FirstOrDefault(d => d.Id == id);
        }
    }
}
