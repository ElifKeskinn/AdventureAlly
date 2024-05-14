using System.Collections.Generic;

namespace CleanArchitecture.Core.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersViewModel
    {
        public string Name { get; set; }
        public string Place { get; set; }
        public List<string> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
