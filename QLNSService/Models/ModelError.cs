﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelError
    {
        public int CountErrorLastHours { get; set; }
        public int CountErrorCurrentHours { get; set; }
        public int TotalError { get; set; }
        public int Id { get; set; }
        public string ErrorName { get; set; }
    }
}