using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Git.Data
{
    public class Repository
    {
        public Repository()
        {
            this.RepositoryId = Guid.NewGuid().ToString();
        }

        [Key]
        public string RepositoryId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Commit> Commits { get; set; }
    }
}
