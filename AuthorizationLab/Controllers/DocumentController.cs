using AuthorizationLab.AuthorizationHandlers;
using AuthorizationLab.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthorizationLab.Controllers
{
    public class DocumentController : Controller
    {
        private IDocumentRepository _documentRepository;
        private IAuthorizationService _authorizationService;

        public DocumentController(
            IDocumentRepository documentRepository,
            IAuthorizationService authorizationService)
        {
            _documentRepository = documentRepository;
            _authorizationService = authorizationService;
        }

        public ActionResult Index()
        {
            return View(_documentRepository.Get());
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            ActionResult response;

            var document = _documentRepository.GetById(id);

            if (document == null)
                response = new NotFoundResult();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, document, new EditDocumentRequirement());
            if (authorizationResult.Succeeded)
                response = View(document);
            else
                response = new ForbidResult();

            return response;
        }
    }
}
