using Moq;
using Xunit;
using TrabalhoBackEnd.BLL;
using static TrabalhoBackEnd.Models.Roles;
using TrabalhoBackEnd.IDAL;

namespace TrabalhoBackEnd.Tests.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();

            _roleService = new RoleService(_mockRoleRepository.Object);
        }

        [Fact]
        public void AddRole_ShouldAddRole_WhenValidRole()
        {
            var role = new Role { Id = 1, Name = "Admin" };

            _mockRoleRepository.Setup(repo => repo.Insert(It.IsAny<Role>())).Returns(role);

            _roleService.AddRole(role);

            _mockRoleRepository.Verify(repo => repo.Insert(It.IsAny<Role>()), Times.Once);
        }

        [Fact]
        public void GetAllRoles_ShouldReturnListOfRoles()
        {
            var roles = new List<Role>
        {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        };

            _mockRoleRepository.Setup(repo => repo.GetAll()).Returns(roles);

            var result = _roleService.GetAllRoles();

            Assert.Equal(2, result.Count);
            _mockRoleRepository.Verify(repo => repo.GetAll(), Times.Once);
        }
    }

}
