using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Domains
{
    public partial class BooksLoan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid SubscriberId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Subscriber Subscriber { get; set; }
    }
}
