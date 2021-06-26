using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.Models
{
    public class LekuvanPacient
    {
        public int Id { get; set; }
        public string Lek { get; set; }
        public int PacientId { get; set; }
        public Pacient Pacient { get; set; }
        public int DoktorId { get; set; }
        public Doktor Doktor { get; set; }
    }
}
