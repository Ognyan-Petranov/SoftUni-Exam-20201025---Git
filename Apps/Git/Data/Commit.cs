using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Git.Data
{
    public class Commit
    {
        public Commit()
        {
            this.CommitId = Guid.NewGuid().ToString();
        }

        [Key]
        public string CommitId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }

        public string RepositoryId { get; set; }
        public Repository Repository { get; set; }
    }
}
