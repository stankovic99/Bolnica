using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.ViewModels
{
    public class DoktorViewModel
    {
        [Required]
        [Display(Name = "Име")]
        public string Ime { get; set; }

        [Required]
        [Display(Name = "Презиме")]
        public string Prezime { get; set; }

        [Display(Name = "Звање")]
        public string? Zvanje { get; set; }

        [Display(Name = "Возраст")]
        public int? Vozrast { get; set; }

        [Display(Name = "Дата на вработување")]
        public DateTime? DataVrabotuvanje { get; set; }

        [Display(Name = "Плата")]
        public int? Plata { get; set; }

        [Display(Name = "Профилна слика")]
        public IFormFile ProfilnaSlika { get; set; }
    }
}
