using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Account;
using StudentPlacement.Backend.Models.Enter;

using Tokens = StudentPlacement.Backend.Models.Enter.EnterResponse;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<DataResponse<EnterResponse>> Enter(EnterRequest request);

        public Task<DataResponse<IEnumerable<GroupView>>> GetStudentSetting();

        public Task<BaseResponse> CreateAccount(CreateAccountRequest request);

        public Task<DataResponse<IEnumerable<GetAllUsersResponse>>> GetAllUsers();

        public Task<DataResponse<GetUserResponse>> GetUser(int idUser);

        public Task<DataResponse<Tokens>> RefreshToken(string token);

        public Task<BaseResponse> DeleteUser(DeleteUserRequest request);

        public Task<BaseResponse> ChangeUser(ChangeUserRequest request);
    }
}
