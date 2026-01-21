using Beyti.Models;
using System.ComponentModel.DataAnnotations;

namespace Beyti.ViewModel
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }


        [Required]
        public string Role { get; set; } = null!;

        // Address fields
        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string BuildingNo { get; set; } = null!;
        [Required]
        public string Floor { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
