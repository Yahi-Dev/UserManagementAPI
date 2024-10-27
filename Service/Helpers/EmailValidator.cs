using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Layer_Service.Helpers
{
    public static class EmailValidator
    {
        public static bool IsValidEmailFormat(string email)
        {
            string domain = @"^[a-zA-Z0-9._%+-]+@bhd\.com$";
            return Regex.IsMatch(email, domain);
        }
    }
}
