using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DAL.Models
{
    public enum Statuses
    {
        [Display(Name = "Назначена")]
        Appointed,
        [Display(Name = "Выполняется")]
        IsPerformed,
        [Display(Name = "Приостановлена")]
        Suspended,
        [Display(Name = "Завершена")]
        Completed
    }
}
