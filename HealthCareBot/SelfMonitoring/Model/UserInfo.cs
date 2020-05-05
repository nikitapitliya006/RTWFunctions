using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoring.Model
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string TeamsAddress { get; set; }
        public string TwilioAddress { get; set; }
    }
}