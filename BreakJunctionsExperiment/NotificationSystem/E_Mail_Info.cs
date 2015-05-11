using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.NotificationSystem
{
    public class E_Mail_Info
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string E_Mail_Address { get; set; }

        public E_Mail_Info(string __Name, string __Surname, string __E_Mail_Address)
        {
            Name = __Name;
            Surname = __Surname;
            E_Mail_Address = __E_Mail_Address;
        }
    }
}
