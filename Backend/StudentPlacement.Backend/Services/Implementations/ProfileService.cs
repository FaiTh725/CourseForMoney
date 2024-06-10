using Azure.Core;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Domain.Enums;
using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Profile;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IAllocationRequestRepository allocationRequestRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IFileService fileService;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContext;

        public ProfileService(IUserRepository userRepository,
                IOrganizationRepository organizationRepository,
                IAllocationRequestRepository allocationRequestRepository,
                IStudentRepository studentRepository,
                IFileService fileService,
                LinkGenerator linkGenerator,
                IHttpContextAccessor httpContext)
        {
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;
            this.allocationRequestRepository = allocationRequestRepository;
            this.studentRepository = studentRepository;
            this.fileService = fileService;
            this.linkGenerator = linkGenerator;
            this.httpContext = httpContext;
        }

        public async Task<DataResponse<AddAllocationResponse>> AddAllocationRequest(AddAllocationRequestRequest request)
        {
            try
            {
                var organization = await organizationRepository.GetOrganizationById(request.IdOrganization);

                if(organization == null)
                {
                    return new DataResponse<AddAllocationResponse>
                    {
                        Data = new(),
                        StatusCode = StatusCode.NotFoundOrganization,
                        Description = "Организация не найдена"
                    };
                }

                var createdAllocationRequest = await allocationRequestRepository
                        .CreateAllocationRequest(new AllocationRequest
                        {
                            Adress = request.Adress,
                            CountPlace = request.CountFreePlace,
                            Specialist = request.Specialist,
                            Organization = organization,
                            OrganizationId = organization.Id
                        });


                organization.AllocationRequests.Add(createdAllocationRequest);

                var newOrganization = await organizationRepository.UpdateOrganizationById(organization.Id, organization);

                return new DataResponse<AddAllocationResponse>
                {
                    Description = "Добавили запрос на распределение",
                    StatusCode = StatusCode.Ok,
                    Data = new AddAllocationResponse
                    {
                        IdRequest = createdAllocationRequest.Id,
                        CountPlace = createdAllocationRequest.CountPlace,
                        Specialist = createdAllocationRequest.Specialist,
                        NameRequest = createdAllocationRequest.Adress,
                        UrlOrderFile = createdAllocationRequest.OrderFilePath
                    }
                };
            }
            catch
            {
                return new DataResponse<AddAllocationResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new()
                };
            }
        }

        public async Task<BaseResponse> DeleteAllocationRequest(DeleteAllocationRequest request)
        {
            try
            {
                var allocation = await allocationRequestRepository.GetAllocationRequestById(request.IdRequest);
                var organization = await organizationRepository.GetOrganizationById(request.IdOrganization);

                if (allocation == null || organization == null)
                {
                    return new BaseResponse
                    {
                        Description = "Запрос или орагинизация не найден",
                        StatusCode = StatusCode.NotFountRequest
                    };
                }

                organization.AllocationRequests.Remove(allocation);
                await organizationRepository.UpdateOrganizationById(organization.Id, organization);

                await studentRepository.DeleteRequestInStudents(allocation);

                var isDeleteFile = fileService.DeleteFile(Path.Combine("RequestOrders", $"{allocation.Id}.docx"));

                if (!isDeleteFile)
                {
                    return new BaseResponse
                    {
                        Description = "Не удалось найти файл приказа",
                        StatusCode = StatusCode.NotFoundFile
                    };
                }

                return new BaseResponse
                {
                    Description = "Удалили заявку",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                };
            }
        }

        public async Task<DataResponse<IEnumerable<GetAllocationRequestResponse>>> GetAllRequestsOrganization(int idOrganization)
        {
            try
            {
                var organization = await organizationRepository.GetOrganizationById(idOrganization);

                if (organization == null) 
                {
                    return new DataResponse<IEnumerable<GetAllocationRequestResponse>>
                    {
                        StatusCode = StatusCode.NotFoundOrganization,
                        Description = "Не удалось найти организацию",
                        Data = new List<GetAllocationRequestResponse>()
                    };
                }

                return new DataResponse<IEnumerable<GetAllocationRequestResponse>>
                {
                    Description = "Получили все запросы организации",
                    StatusCode = StatusCode.Ok,
                    Data = organization.AllocationRequests.Select(x => new GetAllocationRequestResponse
                    {
                        IdRequest = x.Id,
                        Adress = x.Adress,
                        Specialist = x.Specialist,
                        CountPlace = x.Students.Count,
                        UrlOrderFile = x.OrderFilePath
                    }).ToList()
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GetAllocationRequestResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<GetAllocationRequestResponse>()
                };
            }
        }

        public async Task<byte[]> GetOrderAllocationRequest(int allocationRequestId)
        {
            var allocationRequest = await allocationRequestRepository.GetAllocationRequestById(allocationRequestId);

            if (allocationRequest == null)
            {
                return new byte[0];
            }

            var bytes = await fileService.GetByteFile(Path.Combine("RequestOrders", $"{allocationRequest.Id}.docx"));

            return bytes;
        }

        public async Task<DataResponse<GetOrganizationProfileResponse>> GetOrganizationProfile(int idUser)
        {
            try
            {
                var organization = await organizationRepository.GetOrganizationByIdUser(idUser);

                if(organization == null)
                {
                    return new DataResponse<GetOrganizationProfileResponse>
                    {
                        Data = new (),
                        Description = "Организация не найдена",
                        StatusCode = StatusCode.NotFoundOrganization
                    };
                }

                return new DataResponse<GetOrganizationProfileResponse>
                {
                    Description = "Получили информацию организации",
                    StatusCode = StatusCode.Ok,
                    Data = new GetOrganizationProfileResponse
                    {
                        Contacts = organization.Contacts,
                        IdOrganization = organization.Id,
                        NameOrganization = organization.Name,
                        Requests = organization.AllocationRequests.Select(x => new RequestProfileView
                        {
                            IdRequest = x.Id,
                            CountPlace = x.CountPlace,
                            NameRequest = x.Adress,
                            Specialist = x.Specialist,
                            UrlOrderFile = x.OrderFilePath
                        }).ToList()
                    }
                };
            }
            catch
            {
                return new DataResponse<GetOrganizationProfileResponse>
                {
                    StatusCode = StatusCode.ServerError,
                    Description = "Ошибка сервера",
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<GetStudentProfileResponse>> GetStudentProfile(int idUser)
        {
            try
            {
                var student = await studentRepository.GetStudentByUserId(idUser);

                if (student == null) 
                {
                    return new DataResponse<GetStudentProfileResponse>
                    {
                        Description = "Студен не найден",
                        StatusCode = StatusCode.NotFoundStudent,
                        Data = new()
                    };
                }

                return new DataResponse<GetStudentProfileResponse>
                {
                    Description = "Получили профиль студента",
                    StatusCode = StatusCode.Ok,
                    Data = new GetStudentProfileResponse
                    {
                        IsMarried = student.IsMarried,
                        AdressStudent = student.Adress,
                        AverageScore = student.AverageScore,
                        ExtendedFamily = student.ExtendedFamily,
                        FullName = student.FullName,
                        Group = student.GroupId
                    }
                };
            }
            catch
            {
                return new DataResponse<GetStudentProfileResponse>
                {
                    Data = new(),
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<DataResponse<GetStudentRequestResponse>> GetStudentRequest(int idUser)
        {
            try
            {
                var student = await studentRepository.GetStudentRequest(idUser);

                return new DataResponse<GetStudentRequestResponse>
                {
                    Data = student,
                    Description = "Получили заявку",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new DataResponse<GetStudentRequestResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<HomeProfileResponse>> GetUserHomeProfile(int idUser)
        {
            try
            {
                var user = await userRepository.GetById(idUser);

                if (user == null)
                {
                    return new DataResponse<HomeProfileResponse>
                    {
                        Data = new(),
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUser,
                    };
                }

                return new DataResponse<HomeProfileResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Получили логин и картинку пользователя",
                    Data = new HomeProfileResponse
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Image = user.ImageUserStringFormat
                    }
                };
            }
            catch
            {
                return new DataResponse<HomeProfileResponse>
                {
                    Data = new(),
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<DataResponse<GetUserProfileResponse>> GetUserProfile(int idUser)
        {
            try
            {
                var user = await userRepository.GetById(idUser);

                if(user == null)
                {
                    return new DataResponse<GetUserProfileResponse>
                    {
                        Data = new(),
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUser,
                    };
                }

                return new DataResponse<GetUserProfileResponse> 
                { 
                    Description = "Получили основные данные пользователя",
                    StatusCode = StatusCode.Ok,
                    Data = new GetUserProfileResponse
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Email = user.Email,
                        Image = user.ImageUserStringFormat,
                        Role = (int)user.Role
                    }
                };
            }
            catch
            {
                return new DataResponse<GetUserProfileResponse>
                {
                    Data = new(),
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                };
            }
        }

        public async Task<BaseResponse> UpdateProfileOrganization(ChangeProfileRequest request)
        {
            
            var organization  = await organizationRepository.GetOrganizationByLogin(request.LoginUser);

            if(organization == null)
            {
                return new BaseResponse
                {
                    Description = "Организация не найдена",
                    StatusCode = StatusCode.NotFoundOrganization,
                };
            }

            organization.Contacts = request.Contact;
            organization.Name = request.OrganizationName;

            await organizationRepository.UpdateOrganizationById(organization.Id, organization);

            return new BaseResponse
            {
                Description = "Обновили профиль организации",
                StatusCode = StatusCode.Ok,
            };
        }

        public async Task<BaseResponse> UpdateRequest(ChangeRequest request)
        {
            try
            {
                var allocationRequest = await allocationRequestRepository.GetAllocationRequestById(request.IdRequest);

                if(allocationRequest == null)
                {
                    return new BaseResponse
                    {
                        Description = "Запрос не найден",
                        StatusCode = StatusCode.NotFountRequest,
                    };
                }

                allocationRequest.Adress = request.Adress;
                allocationRequest.Specialist = request.Specialist;
                allocationRequest.CountPlace = request.CountPlace;

                await allocationRequestRepository.UpdateAllocationRequest(allocationRequest.Id, allocationRequest);

                return new BaseResponse
                {
                    Description = "Обновили запрос",
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

        public async Task<DataResponse<UploadFileRequestResponse>> UploadFileRequest(AddOrderFileRequest request)
        {
            try
            {
                var allocationRequest = await allocationRequestRepository.GetAllocationRequestById(request.IdAllocationRequest);

                if (allocationRequest == null)
                {
                    return new DataResponse<UploadFileRequestResponse>
                    {
                        Description = "Запрос не найден",
                        StatusCode = StatusCode.NotFountRequest,
                        Data = new UploadFileRequestResponse()
                    };
                }

                var path = Path.Combine("RequestOrders", $"{allocationRequest.Id}.docx");

                await fileService.AddFile(path, request.OrderRequest);

                var filePath = linkGenerator.GetUriByAction(
                                httpContext: httpContext.HttpContext,
                                action: "GetOrderFile",
                                controller: "File",
                                values: new { allocationRequestId = allocationRequest.Id });

                allocationRequest.OrderFilePath = filePath;

                await allocationRequestRepository.UpdateAllocationRequest(allocationRequest.Id, allocationRequest);

                return new DataResponse<UploadFileRequestResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Добавили файл приказа",
                    Data = new UploadFileRequestResponse()
                    {
                        UrlOrderFile = filePath
                    }
                };
            }
            catch
            {
                return new DataResponse<UploadFileRequestResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new UploadFileRequestResponse()
                };
            }
        }
    }
}
