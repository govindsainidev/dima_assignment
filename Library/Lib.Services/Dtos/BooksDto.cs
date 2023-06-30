using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services.Dtos
{
    public class BooksBaseDto
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int? GenereId { get; set; }
        
        
       
    }
    public class AddBookDto : BooksBaseDto
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class BooksDto : BooksBaseDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Genere { get; set; }
    }
}
