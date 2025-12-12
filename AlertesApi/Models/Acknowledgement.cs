using System;

namespace AlertesApi.Models
{
    public class Acknowledgement
    {
        public int Id { get; set; }
        public int AlerteId { get; set; }
        public int PosteId { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcknowledgedAt { get; set; }
    }
}