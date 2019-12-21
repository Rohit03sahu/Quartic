using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDMWebApi.Handlers
{
    public class ApiSecurity
    {
        public static bool VaidateUser(string username, string password)
        {
            if (CheckUserNamePassword(username, password))
            { return true; }
            else
            { return false; }
        }

        static bool CheckUserNamePassword(string username, string password)
        {
            string UserName = System.Configuration.ConfigurationManager.AppSettings["auth_UserName"];
            string Password = System.Configuration.ConfigurationManager.AppSettings["auth_Password"];

            if (username == UserName && Password == password) { return true; }
            else { return false; }

        }
    }
}