namespace Beyti.ViewModel.Profile
{
    public class ProfileVM
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Role { get; set; }

        // Address
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? BuildingNo { get; set; }
        public string? Floor { get; set; }
    }
}
