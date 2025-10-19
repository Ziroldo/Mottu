using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mottu.Backend.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public int DelivererId { get; set; }

        [Required]
        public int MotoId { get; set; }

        [Required]
        [Range(7, 50)]
        public int Plan { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DailyPrice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate   { get; set; }
        public DateTime? ReturnDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalCost { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Deliverer? Deliverer { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Moto? Moto { get; set; }
    }
}
