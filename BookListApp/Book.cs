using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListApp
{
    internal class Book
    {
        public string title { get; set; }
        public string author { get; set; }

        public int year { get; set; }
        public Book(string title, string author, int year)
        {
            this.title = title;
            this.author = author;
            this.year = year;
        }
        
    }
}
