using Bolnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.ViewModels
{
    public class DoktorImeViewModel
    {
        public IList<Doktor> Doktors { get; set; }
        public string SearchString { get; set; }

    }
}
