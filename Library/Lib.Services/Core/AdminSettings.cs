using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services.Core
{

    public class AdminSettings
    {
        public bool IsUniqueBookName { get; set; }
        public bool IsUniqueSubscriberName { get; set; }
        public int MaxLoanedAmount { get; set; }
    }
}
