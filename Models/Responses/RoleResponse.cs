using static TrabalhoBackEnd.Models.Roles;

namespace TrabalhoBackEnd.Models.Responses
{
    public class RoleResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public RoleResponse(Role role)
        {
            Name = role.Name;
            Description = role.Description;
        }
    }
}
