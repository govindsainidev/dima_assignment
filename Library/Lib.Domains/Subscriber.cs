using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Domains
{
    public partial class Subscriber
    {
        public Subscriber()
        {
            BooksLoans = new HashSet<BooksLoan>();
        }

        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<BooksLoan> BooksLoans { get; set; }
    }
}
