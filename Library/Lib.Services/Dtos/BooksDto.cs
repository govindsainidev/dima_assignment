using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lib.Services.Dtos
{
    public class BooksBaseDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public int? GenereId { get; set; }
       
    }

    public class AddUpdateBooksDto : BooksBaseDto
    {
        
    }

    public class BooksDto : BooksBaseDto
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Genere { get; set; }
    }
}
