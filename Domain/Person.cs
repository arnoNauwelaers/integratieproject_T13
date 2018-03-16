using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class Person : Item
    {
        public Person(int id, string name) : base(id, name)
        {
        }
    }
}
