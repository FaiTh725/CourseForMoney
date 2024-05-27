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
        private readonly IWebHostEnvironment environment;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;    

        public AccountService(IUserRepository userRepository,
                    IJwtProviderService jwtProviderService,
                    IGroupRepository groupRepository,
                    IOrganizationRepository organizationRepository,
                    IAllocationRequestRepository allocationRequestRepository,
                    IStudentRepository studentRepository,
                    IWebHostEnvironment environment,
                    LinkGenerator linkGenerator,
                    IHttpContextAccessor httpContextAccessor,
                    IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.jwtProviderService = jwtProviderService;
            this.groupRepository = groupRepository;
            this.organizationRepository = organizationRepository;
            this.allocationRequestRepository = allocationRequestRepository;
            this.studentRepository = studentRepository;
            this.environment = environment;
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
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

                var userByLogin = await userRepository.GetUserByLogin(request.Login);
                var userByEmail = await userRepository.GetUserByEmail(request.Email);

                if (userByLogin != null && user.Login != userByLogin.Login)
                {
                    return new BaseResponse
                    {
                        Description = "Логин уже занят",
                        StatusCode = StatusCode.InvaludDataforUpdate
                    };
                }

                if (userByEmail != null && user.Email !=  userByEmail.Email) 
                {
                    return new BaseResponse
                    {
                        Description = "Почта уже занят",
                        StatusCode = StatusCode.InvalidEmail
                    };
                }

                var updatedUser = await userRepository.Update(user.Id, new User
                {
                    Login = request.Login,
                    Password = request.Password,
                    Role = user.Role,
                    Email = request.Email,
                    ImageUserStringFormat = user.ImageUserStringFormat
                });



                if (request.Image != null)
                {

                    string? newImageUser = null;

                    if (File.Exists(environment.WebRootPath + $"/StorageUserImage/{request.Id} - {request.Login}.png"))
                    {
                        File.Delete(environment.WebRootPath + $"/StorageUserImage/{request.Id} - {request.Login}.png");
                    }

                    using (var file = new FileStream(environment.WebRootPath + $"/StorageUserImage/{request.Id} - {request.Login}.png", FileMode.Create))
                    {
                        await request.Image.CopyToAsync(file);
                    }

                    newImageUser = linkGenerator.GetUriByAction(
                                httpContext: httpContextAccessor.HttpContext,
                                action: "GetUserImage",
                                controller: "Image",
                                values: new { userId = user.Id });

                    updatedUser.ImageUserStringFormat = newImageUser;

                    await userRepository.Update(user.Id, updatedUser);
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
                        Name = request.OrganizationName,
                        Contacts = request.Contact,

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
                    Role = (Role)request.Role,
                    Email = request.Email
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

                var userWithCurrentEmail = await userRepository.GetUserByEmail(request.Email);

                if(userWithCurrentEmail != null)
                {
                    return new BaseResponse
                    {
                        Description = "Почта уже занята",
                        StatusCode = StatusCode.InvalidEmail
                    };
                }

                try
                {
                    string message = $@"<div style=""color: #fff; font-size: 18px;background-color: #008054d9;padding: 15px 20px; border-radius: 14px; "">
                                            <div style='margin-bottom: 10px'>
                                              <label>Логин</label>
                                              <p style=""margin-top: 5px; margin-left: 10px"">{request.Login}</p>
                                            </div>
                                            <div>
                                              <label>Пароль</label>
                                              <p style=""margin-top: 5px; margin-left: 10px"">{request.Password}</p>
                                            </div>
                                            <footer style=""text-align: center; color: #ffffffb0"">Данные от личного кабинета</footer>
                                        </div>";

                    await emailService.SendDefaultEmail(request.Email, "Распределение, личный кабинет", message);
                }
                catch (MailKit.Net.Smtp.SmtpCommandException)
                {
                    return new BaseResponse
                    {
                        Description = "Email не действителен",
                        StatusCode = StatusCode.InvalidEmail
                    };
                }

                var createdUser = await userRepository.Createuser(user);

                if (request.Image != null)
                {
                    var path = "/StorageUserImage/" + $"{createdUser.Id} - {user.Login}.png";

                    using (var file = new FileStream(environment.WebRootPath + path, FileMode.Create))
                    {
                        await request.Image.CopyToAsync(file);
                    }
                }

                string? urlImage = null;
                var pathUserImage = environment.WebRootPath + $"/StorageUserImage/{user.Id} - {user.Login}.png";
                if (File.Exists(pathUserImage))
                {
                    urlImage = linkGenerator.GetUriByAction(
                        httpContext: httpContextAccessor.HttpContext,
                        action: "GetUserImage",
                        controller: "Image",
                        values: new { userId = user.Id }
                        );
                }

                createdUser.ImageUserStringFormat = urlImage;

                await userRepository.Update(createdUser.Id, createdUser);

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

                    await organizationRepository.CreateOrganization(new Organization
                    {
                        Name = request.NameOrganization,
                        Contacts = request.Contacts,
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

                // Сомнительное место еще не тестил
                await userRepository.DeleteUser(user);

                if (File.Exists(environment.WebRootPath + $"/StorageUserImage/{user.Id} - {user.Login}.png"))
                {
                    File.Delete(environment.WebRootPath + $"/StorageUserImage/{user.Id} - {user.Login}.png");
                }

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

        public async Task<byte[]?> GetImageUser(int idUser)
        {
            var user = await userRepository.GetById(idUser);

            var path = "/StorageUserImage/" + $"{user.Id} - {user.Login}.png";


            if (File.Exists(environment.WebRootPath + path))
            {
                return File.ReadAllBytes(environment.WebRootPath + path);
            }

            return null;
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
