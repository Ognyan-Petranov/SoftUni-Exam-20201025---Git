using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoriesService repositoriesService;

        public RepositoriesController(IRepositoriesService repositoriesService)
        {
            this.repositoriesService = repositoriesService;
        }

        [HttpGet]
        public HttpResponse All()
        {
            var allRepositories = this.repositoriesService.GetRepositories();
            return this.View(allRepositories);
        }

        [HttpGet]
        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string repositoryType)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 10)
            {
                return this.Error("Invalid repository name!");
            }
            string userId = this.GetUserId();

            this.repositoriesService.CreateRepository(name, repositoryType, userId);
            return this.Redirect("/Repositories/All");
        }

    }
}
