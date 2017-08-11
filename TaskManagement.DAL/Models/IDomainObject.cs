using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DAL.Models
{
    public interface IDomainObject
    {
        [Required]
        int Id { get; set; }

        [Required]
        DateTime Created { get; set; }

        [Required]
        DateTime Updated { get; set; }
    }
}
