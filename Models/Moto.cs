using System;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Backend.Models
{
    public class Moto
    {
        [Required, StringLength(8)]
        public string Placa { get; set; } = default!;
        public int Id { get; set; }
        public string Identificador { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Modelo { get; set; } = string.Empty;

   

    }
}
