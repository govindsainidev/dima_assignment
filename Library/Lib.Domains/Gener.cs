using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Domains
{
    public partial class Gener
    {
        public Gener()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
