using System.ComponentModel.DataAnnotations;

namespace Mottu.Backend.Models
{
    public class MotoCreatedLog
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; } = default!;

        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}

