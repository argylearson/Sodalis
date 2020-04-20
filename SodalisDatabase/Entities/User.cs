using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SodalisDatabase.Entities {
    public class User {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(128)]
        public string EmailAddress { get; set; }
        [StringLength(128)]
        public string FirstName { get; set; }
        [StringLength(128)]
        public string LastName { get; set; }
        [MaxLength(64)]
        public byte[] PasswordHash { get; set; }
        [MaxLength(128)]
        public byte[] PasswordSalt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}