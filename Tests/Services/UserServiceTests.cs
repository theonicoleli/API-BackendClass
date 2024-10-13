using Moq;
using Xunit;
using TrabalhoBackEnd.BLL;
using TrabalhoBackEnd.Errors;
using TrabalhoBackEnd.Models;
using TrabalhoBackEnd.ISecurity;
using static TrabalhoBackEnd.Models.Roles;
using static TrabalhoBackEnd.Models.Users;
using TrabalhoBackEnd.IDAL;

namespace TrabalhoBackEnd.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly Mock<IJwtService> _mockJwt;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IRoleRepository>(); 
            _mockJwt = new Mock<IJwtService>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockJwt.Object,
                _mockLogger.Object
            );
        }

        // Teste para verificar se uma exceção é lançada quando o usuário já existe
        [Fact]
        public void Insert_ShouldThrowException_WhenUserAlreadyExists()
        {
            var user = new User { Id = 1, Email = "test@example.com" };

            _mockUserRepository.Setup(repo => repo.FindByEmail(user.Email)).Returns(user);

            Assert.Throws<BadRequestException>(() => _userService.Insert(new User { Email = user.Email }));
        }

        // Teste para verificar se um usuário válido é inserido corretamente
        [Fact]
        public void Insert_ShouldInsertUser_WhenValidUser()
        {
            var user = new User { Id = 0, Email = "newuser@example.com" };

            _mockUserRepository.Setup(repo => repo.FindByEmail(user.Email)).Returns((User)null);

            _mockUserRepository.Setup(repo => repo.Save(It.IsAny<User>())).Returns(user);

            var result = _userService.Insert(user);

            Assert.NotNull(result);
            _mockUserRepository.Verify(repo => repo.Save(It.IsAny<User>()), Times.Once);
        }

        // Teste para listar usuários em ordem decrescente
        [Fact]
        public void List_ShouldReturnUsersSortedById_WhenSortDirIsDesc()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "User 1" },
                new User { Id = 2, Name = "User 2" }
            };

            _mockUserRepository.Setup(repo => repo.FindAll()).Returns(users);

            var result = _userService.List(SortDir.DESC, null);

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].Id);
        }

        // Teste para garantir que a exclusão do último administrador não é permitida
        [Fact]
        public void Delete_ShouldThrowException_WhenDeletingLastAdmin()
        {
            var adminUser = new User
            {
                Id = 1,
                Name = "Admin",
                Roles = new List<Role> { new Role { Name = "ADMIN" } }
            };

            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns(adminUser);
            _mockUserRepository.Setup(repo => repo.FindByRole("ADMIN")).Returns(new List<User> { adminUser });

            Assert.Throws<BadRequestException>(() => _userService.Delete(1));
        }

        // Teste para adicionar uma role a um usuário existente
        [Fact]
        public void AddRole_ShouldAddRole_WhenUserAndRoleExist()
        {
            var user = new User { Id = 1, Roles = new List<Role>() };
            var role = new Role { Name = "ADMIN" };

            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns(user);
            _mockRoleRepository.Setup(repo => repo.FindByName("ADMIN")).Returns(role);

            var result = _userService.AddRole(1, "ADMIN");

            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.Save(It.IsAny<User>()), Times.Once);
        }

        // Teste para verificar o login com credenciais válidas
        [Fact]
        public void Login_ShouldReturnLoginResponse_WhenCredentialsAreValid()
        {
            var user = new User { Id = 1, Email = "test@example.com", Password = "password" };
            var token = "jwt-token";

            _mockUserRepository.Setup(repo => repo.FindByEmail("test@example.com")).Returns(user);
            _mockJwt.Setup(jwt => jwt.CreateToken(user)).Returns(token); 

            var result = _userService.Login("test@example.com", "password");

            Assert.NotNull(result);
            Assert.Equal(token, result.Token);
        }

        // Teste para verificar o login com credenciais inválidas
        [Fact]
        public void Login_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            var user = new User { Id = 1, Email = "test@example.com", Password = "password" };

            _mockUserRepository.Setup(repo => repo.FindByEmail("test@example.com")).Returns(user);

            var result = _userService.Login("test@example.com", "wrongpassword");

            Assert.Null(result);
        }

        // Teste para atualizar o nome de um usuário existente
        [Fact]
        public void Update_ShouldUpdateUser_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "Old Name" };

            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns(user);
            _mockUserRepository.Setup(repo => repo.Save(It.IsAny<User>())).Returns(user);

            var result = _userService.Update(1, "New Name");

            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        // Teste para garantir que a atualização falha se o usuário não existir
        [Fact]
        public void Update_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns((User)null);

            Assert.Throws<NotFoundException>(() => _userService.Update(1, "New Name"));
        }

        // Teste para deletar um usuário com sucesso
        [Fact]
        public void Delete_ShouldDeleteUser_WhenUserExists()
        {
            var user = new User { Id = 1 };

            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns(user);

            var result = _userService.Delete(1);

            Assert.NotNull(result);
            _mockUserRepository.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Once);
        }

        // Teste para deletar um usuário inexistente
        [Fact]
        public void Delete_ShouldReturnNull_WhenUserDoesNotExist()
        {
            _mockUserRepository.Setup(repo => repo.FindById(1)).Returns((User)null);

            var result = _userService.Delete(1);

            Assert.Null(result);
        }
    }
}
