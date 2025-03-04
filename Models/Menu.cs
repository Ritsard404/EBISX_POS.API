﻿using EBISX_POS.API.Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection.Metadata;

namespace EBISX_POS.API.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public required string MenuName { get; set; }
        public required decimal MenuPrice { get; set; }
        public string? MenuImagePath  { get; set; }
        public string? Size { get; set; }
        public bool MenuIsAvailable { get; set; } = true;
        public bool HasDrink { get; set; } = true;
        public bool HasAddOn { get; set; } = true;
        public bool IsAddOn { get; set; } = false;
        public DrinkType? DrinkType { get; set; }
        public required Category Category { get; set; }
    }
}
