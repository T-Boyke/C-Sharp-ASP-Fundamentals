using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using _05_NewsApplication.Models;
using _05_NewsApplication.Application.UseCases;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Controllers {
    public class HomeController : Controller {
        private readonly GetArticlesUseCase _getArticlesUseCase;
        private readonly GetAuthorsUseCase _getAuthorsUseCase;
        private readonly CreateArticleUseCase _createArticleUseCase;

        public HomeController(
            GetArticlesUseCase getArticlesUseCase,
            GetAuthorsUseCase getAuthorsUseCase,
            CreateArticleUseCase createArticleUseCase) {
            _getArticlesUseCase = getArticlesUseCase;
            _getAuthorsUseCase = getAuthorsUseCase;
            _createArticleUseCase = createArticleUseCase;
        }

        public async Task<IActionResult> Index() {
            var articles = await _getArticlesUseCase.ExecuteAsync();
            var vm = new HomeIndexViewModel { Articles = articles };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            var vm = new ArticleCreateViewModel();
            await LoadAuthorsToViewBagAsync(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateViewModel vm) {
            if (!ModelState.IsValid) {
                await LoadAuthorsToViewBagAsync(vm);
                return View(vm);
            }

            try {
                var headline = new Headline(vm.Headline); // Value Object validates
                var article = new Article {
                    Headline = headline,
                    Content = vm.Content,
                    AuthorId = vm.AuthorId
                };

                await _createArticleUseCase.ExecuteAsync(article);
                return RedirectToAction(nameof(Index));
            } catch (ArgumentException ex) {
                ModelState.AddModelError("Headline", ex.Message);
                await LoadAuthorsToViewBagAsync(vm);
                return View(vm);
            }
        }

        private async Task LoadAuthorsToViewBagAsync(ArticleCreateViewModel vm) {
            var authors = await _getAuthorsUseCase.ExecuteAsync();
            vm.AuthorsList = new SelectList(authors, nameof(Author.Id), nameof(Author.Fullname));
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
