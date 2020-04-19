using System;
using System.ComponentModel.DataAnnotations;

namespace SodalisDatabase.Entities {
    public class User {
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
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}