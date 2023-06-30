using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lib.Services.Core
{
    public static class HelperExtesntions
    {
        public static string GetActualError(this Exception exception)
        {
            string message_ = string.Empty;
            if (exception != null)
            {
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                if (exception.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    string patern_ = "(\"dbo.)([A-Za-z0-9_.])+(\")";
                    Regex re = new Regex(patern_, RegexOptions.IgnoreCase);
                    Match m = re.Match(exception.Message);
                    if (re.IsMatch(exception.Message))
                    {
                        //message_ = $@"This record is associate with{m.Value.Replace("dbo.", string.Empty)}";
                        message_ = $@"This record is associate with REFERENCE TABLE";
                    }
                }
                else
                {
                    message_ = exception.Message;
                }
            }
            return message_;
        }
    }
}
