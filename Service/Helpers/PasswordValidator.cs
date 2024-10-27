using Layer_Entities.Error;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;


namespace Layer_Service.Helpers
{
    public static class PasswordValidator
    {
        public static bool ValidatePassword(string password, string passwordRegex)
        {
            return Regex.IsMatch(password, passwordRegex);
        }
    }
}
