using System;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Backend.Models
{
    public class Deliverer
    {
        public int Id { get; set; }

        [Required]
        public string Identifier { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;

        [Required, StringLength(20)]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ must have 14 digits.")]
        public string CNPJ { get; set; } = default!;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, StringLength(20)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CNH number must have 11 digits.")]
        public string CNHNumber { get; set; } = default!;

        [Required]
        [RegularExpression(@"^(A|B|A\+B)$", ErrorMessage = "CNH type must be A, B or A+B.")]
        public string CNHType { get; set; } = default!;

        public string CNHImagePath { get; set; } = default!;
    }
}

