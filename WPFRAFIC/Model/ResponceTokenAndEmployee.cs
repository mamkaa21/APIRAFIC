using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFRAFIC.Model
{
    public class ResponceTokenAndEmployee
    {
        public string Token { get; set; }
        public string Role { get; set; }

        public Employee Employee { get; set; }
    }
}
