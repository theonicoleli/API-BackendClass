using static TrabalhoBackEnd.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoBackEnd.Models.Requests
{
    public class CreateRoleRequest
    {
        [RegularExpression("^[A-Z][0-9A-Z]*$", ErrorMessage = "The name must start with an uppercase letter and contain only alphanumeric characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public Role ToRole()
        {
            return new Role
            {
                Name = Name,
                Description = Description
            };
        }
    }
}
