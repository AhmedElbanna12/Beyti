using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Net;

namespace Beyti.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }

        public Address Address { get; set; } = null!;

        public Wallet Wallet { get; set; } = null!;
    }

    public enum UserRole
    {
        Customer,
        Chef,
        Supplier,
        Delivery
    }

}
