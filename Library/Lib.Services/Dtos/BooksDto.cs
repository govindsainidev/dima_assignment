using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lib.Services.Dtos
{
    public class BooksBaseDto
    {
        public Guid? Id { get; set; }
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Author Name")]
        public string AuthorName { get; set; }
        [DisplayName("Genered")]
        public int? GenereId { get; set; }
       
    }

    public class AddUpdateBooksDto : BooksBaseDto
    {
        public IEnumerable<SelectListItemDto> Geners { get; set; }
    }

    public class BooksDto : BooksBaseDto
    {
        [DisplayName("Created At")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("Updated At")]
        public DateTime? UpdatedAt { get; set; }
        [DisplayName("Genere")]
        public string Genere { get; set; }
    }
}
