using TrabalhoBackEnd.IDAL;
using static TrabalhoBackEnd.Models.Roles;

namespace TrabalhoBackEnd.DAL
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProjectDbContext _context;

        public RoleRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        public Role GetById(long id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        public Role Insert(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public Role Save(Role role)
        {
            if (role.Id == 0)
            {
                _context.Roles.Add(role);
            }
            else
            {
                _context.Roles.Update(role);
            }
            _context.SaveChanges();
            return role;
        }

        public Role FindByName(string roleName)
        {
            return _context.Roles.FirstOrDefault(r => r.Name == roleName);
        }

        public List<Role> FindAll()
        {
            return _context.Roles.ToList();
        }
    }
}
