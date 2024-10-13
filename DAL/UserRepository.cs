using Microsoft.EntityFrameworkCore;
using static TrabalhoBackEnd.Models.Users;

namespace TrabalhoBackEnd.DAL
{
    public class UserRepository
    {
        private readonly ProjectDbContext _context;

        public UserRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public User Save(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                _context.Users.Update(user);
            }
            _context.SaveChanges();
            return user;
        }

        public List<User> FindAll()
        {
            return _context.Users.Include(u => u.Roles).ToList();
        }

        public List<User> FindByRole(string role)
        {
            return _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.Name == role))
                .OrderBy(u => u.Name)
                .ToList();
        }
        public User FindById(long id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
        }

        public User FindByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == email);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
