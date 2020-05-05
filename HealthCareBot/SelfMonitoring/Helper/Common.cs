using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SelfMonitoring.Helper
{
    public class Common
    {
        //check if email address is valid 
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}