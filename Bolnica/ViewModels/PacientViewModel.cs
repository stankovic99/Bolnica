using Bolnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.ViewModels
{
    public class PacientViewModel
    {
        public IList<Pacient> Pacienti { get; set; }
        public string SearchString { get; set; }
    }
}
