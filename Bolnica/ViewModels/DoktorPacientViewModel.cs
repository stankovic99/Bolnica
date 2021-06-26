using Bolnica.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.ViewModels
{
    public class DoktorPacientViewModel
    {
        public Doktor Doktor { get; set; }
        public IEnumerable<int> SelectedPacients { get; set; }
        public IEnumerable<SelectListItem> PacientList { get; set; }
    }
}
