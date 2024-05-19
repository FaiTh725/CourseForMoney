using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Domain.Enums;
using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Account;
using StudentPlacement.Backend.Models.Enter;
using StudentPlacement.Backend.Services.Interfaces;
using Tokens = StudentPlacement.Backend.Models.Enter.EnterResponse;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IJwtProviderService jwtProviderService;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IAllocationRequestRepository allocationRequestRepository;
        private readonly IStudentRepository studentRepository;

        public AccountService(IUserRepository userRepository,
                    IJwtProviderService jwtProviderService,
                    IGroupRepository groupRepository,
                    IOrganizationRepository organizationRepository,
                    IAllocationRequestRepository allocationRequestRepository,
                    IStudentRepository studentRepository)
        {
            this.userRepository = userRepository;
            this.jwtProviderService = jwtProviderService;
            this.groupRepository = groupRepository;
            this.organizationRepository = organizationRepository;
            this.allocationRequestRepository = allocationRequestRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<BaseResponse> ChangeUser(ChangeUserRequest request)
        {
            try
            {
                var user = await userRepository.GetById(request.Id);

                if (user == null)
                {
                    return new BaseResponse
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUser
                    };
                }

                var userByLogin = await userRepository.GetUserByLogin(user.Login);

                if (userByLogin != null && user.Login != userByLogin.Login)
                {
                    return new BaseResponse
                    {
                        Description = "Логин уже занят",
                        StatusCode = StatusCode.InvaludDataforUpdate
                    };
                }

                if (userByLogin.Login != request.Login || userByLogin.Password != request.Password)
                {
                    await userRepository.Update(user.Id, new User
                    {
                        Login = request.Login,
                        Password = request.Password,
                        Role = user.Role
                    });
                }


                if (request.Role == 0)
                {
                    var student = await userRepository.GetUserByLogin(request.Login);

                    var newStudent = new Student
                    {
                        FullName = request.FullName,
                        Adress = request.AdressStudent,
                        AverageScore = request.AverageScore,
                        ExtendedFamily = request.ExtendedFamily,
                        IsMarried = request.IsMarried,
                        Group = await groupRepository.GetGroupById(request.Group),
                        GroupId = request.Group
                    };

                    await studentRepository.UpdateStudentByLogin(request.Login, newStudent);
                }
                else if (request.Role == 3)
                {
                    var organization = await organizationRepository.GetOrganizationByLogin(request.Login);

                    var newOrganization = new Organization
                    {
                        Name = request.NameOrganization,
                        Contacts = request.Contacts,

                    };

                    await organizationRepository.UpdateOrganizationByLogin(request.Login, newOrganization);
                }


                return new BaseResponse
                {
                    Description = "Обновили данные",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse> CreateAccount(CreateAccountRequest request)
        {
            try
            {
                var user = new User
                {
                    Login = request.Login,
                    Password = request.Password,
                    Role = (Role)request.Role
                };

                var checkedUserLogin = await userRepository.GetUserByLogin(request.Login);

                if (checkedUserLogin != null)
                {
                    return new BaseResponse
                    {
                        Description = "Логин уже занят",
                        StatusCode = StatusCode.UserExist
                    };
                }

                var createdUser = await userRepository.Createuser(user);

                if (request.Role == 0)
                {
                    var checkedUser = await studentRepository.FindStudentByLoginAndFullName(request.Login, request.FullName);

                    if (checkedUser != null)
                    {
                        return new BaseResponse
                        {
                            Description = "Студент уже добавлен",
                            StatusCode = StatusCode.StudentExist
                        };
                    }

                    var group = await groupRepository.GetGroupById(request.Group);

                    var student = new Student
                    {
                        FullName = request.FullName,
                        Adress = request.AdressStudent,
                        AverageScore = request.AverageScore,
                        ExtendedFamily = request.ExtendedFamily,
                        IsMarried = request.IsMarried,
                        StatusRequest = StatusAllocationRequest.NoRequest,
                        User = createdUser,
                        UserId = createdUser.Id,
                        Group = group,
                        GroupId = group.Id,
                    };


                    var createdStudent = await studentRepository.CreateStudent(student);

                    group.Students.Add(createdStudent);

                    await groupRepository.UpdateGroup(group.Id, group);
                }
                else if (request.Role == 3)
                {

                    var checkedOrganization = await organizationRepository.FindOrganizationByLoginAndName(request.Login, request.NameOrganization);

                    if (checkedOrganization != null)
                    {
                        return new BaseResponse
                        {
                            Description = "Организация уже добавлена",
                            StatusCode = StatusCode.OrganizatinExist
                        };
                    }

                    /*var allocationRequest = await allocationRequestRepository.CreateAllocationRequest(new AllocationRequest
                    {
                        Adress = request.AdressAllocationRequest,
                        CountPlace = request.CountPlace
                    });*/

                    await organizationRepository.CreateOrganization(new Organization
                    {
                        Name = request.NameOrganization,
                        Contacts = request.Contacts,
                        /*AllocationRequest = allocationRequest,
                        AllocationRequestId = allocationRequest.Id,*/
                        User = createdUser,
                        UserId = createdUser.Id,
                    });
                }


                return new BaseResponse
                {
                    Description = "Добавили данные",
                    StatusCode = StatusCode.Ok
                };

            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse> DeleteUser(DeleteUserRequest request)
        {
            try
            {
                var user = await userRepository.GetById(request.IdUser);

                if (user == null)
                {
                    return new BaseResponse
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUser
                    };
                }

                await userRepository.DeleteUser(user);

                return new BaseResponse
                {
                    Description = "Удалили пользователя",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<DataResponse<EnterResponse>> Enter(EnterRequest request)
        {
            try
            {
                var user = await userRepository.FindByData(new User { Login = request.Login, Password = request.Password });

                if (user == null)
                {
                    return new DataResponse<EnterResponse>
                    {
                        Description = "Пользователь не зарегистрирован",
                        StatusCode = StatusCode.UnAuthorize,
                        Data = new()
                    };
                }

                var token = jwtProviderService.GenerateTocken(user);

                user.Token = token.refreshToken;
                user.TimeEndToken = DateTime.Now.AddDays(3);
                var newUser = await userRepository.Update(user.Id, user);

                if (newUser == null)
                {
                    return new DataResponse<EnterResponse>
                    {
                        Description = "Ошибка при обновлении пользователя",
                        StatusCode = StatusCode.UnAuthorize,
                        Data = new()
                    };
                }

                return new DataResponse<EnterResponse>
                {
                    Description = "Успешный вход",
                    StatusCode = StatusCode.Ok,
                    Data = new EnterResponse
                    {
                        Token = token.token,
                        RefreshToken = token.refreshToken
                    }
                };
            }
            catch
            {
                return new DataResponse<EnterResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = Domain.Enums.StatusCode.ServerError,
                    Data = new EnterResponse()

                };
            }
        }

        public async Task<DataResponse<IEnumerable<GetAllUsersResponse>>> GetAllUsers()
        {
            try
            {
                var usersInfo = await userRepository.GetAllUsersWithInfo();

                return new DataResponse<IEnumerable<GetAllUsersResponse>>
                {
                    Description = "Получили всех пользователей",
                    StatusCode = StatusCode.Ok,
                    Data = usersInfo
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GetAllUsersResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<GetAllUsersResponse>()
                };
            }
        }

        public async Task<DataResponse<IEnumerable<GroupView>>> GetStudentSetting()
        {
            try
            {
                var groups = (await groupRepository.GetGroups()).Select(x => new GroupView { Id = x.Id, Name = x.Number });

                return new DataResponse<IEnumerable<GroupView>>
                {
                    Description = "Получили все токены",
                    StatusCode = StatusCode.Ok,
                    Data = groups
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GroupView>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<GroupView>()
                };
            }
        }

        public async Task<DataResponse<GetUserResponse>> GetUser(int idUser)
        {
            try
            {
                var user = await userRepository.GetUser(idUser);

                return new DataResponse<GetUserResponse> 
                {
                    Description = "Получили пользователя",
                    StatusCode = StatusCode.Ok,
                    Data = user
                };
            }
            catch
            {
                return new DataResponse<GetUserResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.Ok,
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<Tokens>> RefreshToken(string token)
        {
            try
            {

                var user = await userRepository.GetUserByRefreshToken(token);

                if (user == null)
                {
                    return new DataResponse<Tokens>
                    {
                        Description = "Пользователь с таким токеном не существует",
                        StatusCode = StatusCode.NotFoundUser,
                        Data = new()
                    };
                }
                if (user.TimeEndToken < DateTime.UtcNow)
                {
                    return new DataResponse<Tokens>
                    {
                        Description = "Срок действия большого токена истек",
                        StatusCode = StatusCode.InvalidToken,
                        Data = new()
                    };
                }

                var newTokens = jwtProviderService.GenerateTocken(user);
                user.Token = newTokens.refreshToken;

                await userRepository.Update(user.Id, user);

                return new DataResponse<Tokens>
                {
                    Description = "Получили новые токены",
                    StatusCode = StatusCode.Ok,
                    Data = new Tokens
                    {
                        Token = newTokens.token,
                        RefreshToken = newTokens.refreshToken
                    }
                };
            }
            catch
            {
                return new DataResponse<Tokens>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new Tokens()
                };
            }
        }
    }
}
