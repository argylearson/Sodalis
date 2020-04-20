using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SodalisDatabase.Enums;

namespace SodalisDatabase.Entities {
    public class Goal {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [StringLength(128)]
        public string Title { get; set; }
        [StringLength(2048)]
        public string Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        public GoalStatus Status { get; set; }
        public bool IsPublic { get; set; }
    }
}