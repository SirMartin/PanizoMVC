﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace PanizoMVC.Models
{

    public class ColumnModel
    {
        public String UrlImage { get; set; }

        public String Titulo { get; set; }

        public String Texto { get; set; }

        public String TextoAbajo { get; set; }

        public String Action { get; set; }

        public String Controller { get; set; }
    }   
}