using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PanizoMVC.Models.Security
{
    public class AuthorizationAttributes
    {
        public class UserAuthorize : FilterAttribute
        {
            // Does nothing, just used for decoration
        }
        public class AdminAuthorize : FilterAttribute
        {
            // Does nothing, just used for decoration
        }
    }
}