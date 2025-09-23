
using System.Diagnostics.CodeAnalysis;

namespace ConexaTest.Domain.Dto
{
    
    public class AddUserDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
