using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lib.Services.Dtos
{
    public class SubscribersBaseDto
    {
        public Guid? Id { get; set; }
        [Required]
        [DisplayName("Firstname")]
        public string Firstname { get; set; }
        [Required]
        [DisplayName("Lastname")]
        public string Lastname { get; set; }

    }

    public class AddUpdateSubscribersDto : SubscribersBaseDto
    {
        
    }

    public class SubscribersDto : SubscribersBaseDto
    {
        [DisplayName("Created At")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("Updated At")]
        public DateTime? UpdatedAt { get; set; }
        [DisplayName("Genere")]
        public int Totalbook { get; set; }


    }
}
