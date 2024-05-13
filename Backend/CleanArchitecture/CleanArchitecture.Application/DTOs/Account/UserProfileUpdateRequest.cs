namespace CleanArchitecture.Core.DTOs.User
{
    public class UserProfileUpdateRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        // Diðer güncellenebilecek özellikler...
    }
}
