using System.ComponentModel.DataAnnotations;

namespace SodalisCore.DataTransferObjects {
    public class UserDto {
        public int Id { get; set; }
        [StringLength(128)]
        public string EmailAddress { get; set; }
        [StringLength(128)]
        public string FirstName { get; set; }
        [StringLength(128)]
        public string LastName { get; set; }
        [StringLength(128)]
        public string Password { get; set; }
    }
}