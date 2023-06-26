using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.Models
{
    internal interface Actions
    {
        void Show();
        void ArchiveCrimes();
        void DeleteCrimes();
        void GeneralTableAdding();
    }
}
