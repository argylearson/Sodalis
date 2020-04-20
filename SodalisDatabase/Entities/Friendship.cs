using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SodalisDatabase.Entities {
    public class Friendship {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool Accepted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
    }
}