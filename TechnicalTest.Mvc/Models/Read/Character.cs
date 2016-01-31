﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalTest.Mvc.Models.Read
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Race { get; set; }
        public string Faction { get; set; }
        public string Class { get; set; }
        public bool IsActive { get; set; }
    }
}