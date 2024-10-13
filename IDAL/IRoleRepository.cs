using static TrabalhoBackEnd.Models.Roles;

namespace TrabalhoBackEnd.IDAL
{
    public interface IRoleRepository
    {
        Role Insert(Role role);
        List<Role> GetAll();
        Role GetById(long id);
        Role FindByName(string roleName);
        List<Role> FindAll();
    }

}
