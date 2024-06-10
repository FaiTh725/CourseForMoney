using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Migrations;
using StudentPlacement.Backend.Models.Account;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;


        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> Createuser(User user)
        {
            var createdUser = await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            return createdUser.Entity;
        }

        public async Task<User> FindByData(User user)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Login == user.Login && x.Password == user.Password);
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<GetAllUsersResponse>> GetAllUsersWithInfo()
        {
            var query = from u in context.Users
                        join o in context.Organizations on u.Id equals o.UserId into ou
                        from o in ou.DefaultIfEmpty()
                        join s in context.Students on u.Id equals s.UserId into su
                        from s in su.DefaultIfEmpty()
                        select new GetAllUsersResponse
                        {
                            Id = u.Id,
                            Login = u.Login,
                            Password = u.Password,
                            Role = (int)u.Role,
                            Email = u.Email,
                            Image = u.ImageUserStringFormat,
                            FullName = s.FullName,
                            AdressStudent = s.Adress,
                            AverageScore = s.AverageScore,
                            IsMarried = s.IsMarried,
                            ExtendedFamily = s.ExtendedFamily,
                            Group = s.GroupId,
                            NameOrganization = o.Name,
                            Contacts = o.Contacts
                        };

            return await query.ToListAsync();

        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Token == refreshToken);
        }

        public async Task<User> Update(int idUser, User newUser)
        {
            var user = await GetById(idUser);

            if (user == null)
            {
                return null;
            }

            user.Password = newUser.Password;
            user.Login = newUser.Login;
            user.Token = newUser.Token;
            user.TimeEndToken = newUser.TimeEndToken;
            user.ImageUserStringFormat = newUser.ImageUserStringFormat;
            user.Email = newUser.Email;

            await context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(User user)
        {
            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task<GetUserResponse> GetUser(int idUser)
        {
            var query = from user in context.Users
                        where user.Id == idUser
                        join request in context.AllocationRequests.Include(x => x.Organization) on user.Id equals request.Organization.UserId into ou
                        from request in ou.DefaultIfEmpty()
                        join student in context.Students.Include(x => x.Group).Include(x => x.AllocationRequest) on user.Id equals student.Id into su
                        from student in su.DefaultIfEmpty()
                        select new GetUserResponse
                        {
                            Id = user.Id,
                            Login = user.Login,
                            Password = user.Password,
                            Role = (int)user.Role,
                            Image = user.ImageUserStringFormat,
                            Email = user.Email,
                            FullName = student.FullName,
                            AdressStudent = student.Adress,
                            AverageScore = student.AverageScore,
                            IsMarried = student.IsMarried,
                            ExtendedFamily = student.ExtendedFamily,
                            Group = student.GroupId,
                            GroupName = student.Group.Number,
                            NameOrganization = request.Organization.Name,
                            Contacts = request.Organization.Contacts,
                            IdAllocationRequest = request.Id,
                            CountPlace = request.CountPlace,
                            Specialist = request.Specialist,
                            UrlOrderFile = request.OrderFilePath,
                            NameAdressAllocationrequestRequest = request.Adress
                        };
            return await query.FirstOrDefaultAsync(x => x.Id == idUser);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
