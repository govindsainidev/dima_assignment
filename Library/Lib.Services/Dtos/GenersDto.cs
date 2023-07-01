using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lib.Services.Dtos
{
    public class GenersBaseDto
    {
        public int? Id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        
        [DisplayName("Description")]
        public string Description { get; set; }
       
    }

    public class AddUpdateGenersDto : GenersBaseDto
    {
        
    }

    public class GenersDto : GenersBaseDto
    {
        
    }
}
