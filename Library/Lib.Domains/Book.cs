using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Domains
{
    public partial class Book
    {
        public Book()
        {
            BooksLoans = new HashSet<BooksLoan>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int? GenereId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Gener Genere { get; set; }
        public virtual ICollection<BooksLoan> BooksLoans { get; set; }
    }
}
