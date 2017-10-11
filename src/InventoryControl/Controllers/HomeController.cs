using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using InventoryControl.Models;
using InventoryControl.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryControl.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IAuthorizationService _authorizationService;
        IAlbumRepository _albumRepository;

        public HomeController(
            IAuthorizationService authorizationService,
            IAlbumRepository albumRepository)
        {
            _authorizationService = authorizationService;
            _albumRepository = albumRepository;
        }

        public IActionResult Index()
        {
            return View(_albumRepository.Get());
        }

        public IActionResult Details(Guid id)
        {
            var album = _albumRepository.Get(id);
            if (album == null)
            {
                return new NotFoundResult();
            }

            return View(album);
        }

        [Authorize(Policy = Policies.AdministratorsOnly)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var album = _albumRepository.Get(id);
            if (album == null)
                return new NotFoundResult();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, album, Policies.CanEditAlbum);
            if (authorizationResult.Succeeded)
                return View(album);
            else
                return new ForbidResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AdministratorsOnly)]
        public async Task<IActionResult> Edit(Album album)
        {
            var existingAlbum = _albumRepository.Get(album.Id);
            if (existingAlbum == null)
                return new NotFoundResult();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, existingAlbum, Policies.CanEditAlbum);
            if (authorizationResult.Succeeded)
            {
                _albumRepository.Update(album);
                return View(album);
            }
            else
            {
                return RedirectToAction("Details", new { id = album.Id });
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}