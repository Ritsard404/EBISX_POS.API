﻿using System.Text.Json.Serialization;

namespace EBISX_POS.API.Services.DTO.Order
{
    public class AddOrderDTO
    {
        public required int qty { get; set; }
        public required long entryId { get; set; }
        public int? menuId { get; set; }
        public int? drinkId { get; set; }
        public int? addOnId { get; set; }
        public decimal? drinkPrice { get; set; }
        public decimal? addOnPrice { get; set; }

        [JsonIgnore]
        public string? cashierEmail { get; set; }
    }
}
