using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{
    internal class User
    {
        public string Name;

        public User(string name)
        {
            Name = name;
        }

        static User[] TestData = new User[]
        {
            new User("Alpha"),
            new User("Luna"),
        };
    }

    
}
