using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.Models
{
    public class Pacient
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Име")]
        public string Ime { get; set; }

        [Required]
        [Display(Name = "Презиме")]
        public string Prezime { get; set; }

        [Display(Name = "Претходни болести")]
        public string? PrethodniBolesti { get; set; }

        [Display(Name = "Приемен датум")]
        public DateTime? PriemenDatum { get; set; }

        [Display(Name = "Возраст")]
        public int? Vozrast { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", Ime, Prezime); }
        }

        [Display(Name = "Доктори")]
        public ICollection<LekuvanPacient> Doktors { get; set; }
    }
}
