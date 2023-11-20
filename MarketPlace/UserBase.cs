using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{

    public class UserBase
    {
        public string Name { get; set; }

        public UserBase()
        {

        }

        public UserBase(string name)
        {
            Name = name;
        }


    }
}
