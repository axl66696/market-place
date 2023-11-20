using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{
    public class ListingBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public string Category { get; set; }

        public ListingBase()
        {

        }


        public ListingBase(int id, string name, string title, string description, int price, DateTime creationTime, string category)
        {
            Id = id;
            UserName = name;
            Title = title;
            Description = description;
            Price = price;
            CreationTime = creationTime;
            Category = category;
        }

    }
}
