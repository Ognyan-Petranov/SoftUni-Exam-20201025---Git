using Git.Data;
using Git.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext dbContext;

        public RepositoriesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateRepository(string name, string repositoryType, string userId)
        {
            var repository = new Repository()
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                IsPublic = true,
                OwnerId = userId,
            };

            if (repositoryType == "Private")
            {
                repository.IsPublic = false;
            }

            this.dbContext.Repositories.AddAsync(repository);
            this.dbContext.SaveChangesAsync();
        }

        public ICollection<RepositoryViewModel> GetRepositories()
        {
            return this.dbContext.Repositories.Where(x => x.IsPublic == true)
                .Select(x => new RepositoryViewModel()
                {
                    Id = x.RepositoryId,
                    Name = x.Name,
                    Owner = x.Owner.Username,
                    CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    CommitsCount = x.Commits.Count

                }).ToList();
        }

        public ICollection<RepositoryViewModel> GetRepositoriesByUser(string userId)
        {
            return this.dbContext.Repositories.Where(x => x.IsPublic == true && x.OwnerId == userId)
                .Select(x => new RepositoryViewModel()
                {
                    Name = x.Name,
                    Owner = x.Owner.Username,
                    CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    CommitsCount = x.Commits.Count

                }).ToList();
        }

        public Repository GetRepositoryById(string id)
        {
            return this.dbContext.Repositories.FirstOrDefault(x => x.RepositoryId == id);
        }
    }
}
