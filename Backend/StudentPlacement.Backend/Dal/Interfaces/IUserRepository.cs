using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Account;
using System.Runtime.CompilerServices;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> FindByData(User user);

        public Task<User> GetById(int id);

        public Task<User> GetUserByLogin(string login);

        public Task<User> GetUserByEmail(string email);

        public Task<User> Update(int idUser, User newUser);

        public Task<User> Createuser(User user);

        public Task<User> GetUserByRefreshToken(string refreshToken);

        public Task<IEnumerable<GetAllUsersResponse>> GetAllUsersWithInfo();

        public Task<GetUserResponse> GetUser(int idUser);

        public Task DeleteUser(User user);
    }
}
