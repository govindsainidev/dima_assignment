using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services.Dtos
{
    public class SelectListItemDto
    {
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
