using static TrabalhoBackEnd.Models.Roles;
using TrabalhoBackEnd.DAL;

namespace TrabalhoBackEnd.BLL
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void AddRole(Role role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                throw new ArgumentException("Role name cannot be null or empty.");
            }

            _roleRepository.Insert(role);
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }

        public Role GetRoleById(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id {id} not found.");
            }
            return role;
        }
    }
}
