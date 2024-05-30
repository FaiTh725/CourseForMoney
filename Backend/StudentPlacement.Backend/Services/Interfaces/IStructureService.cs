﻿using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Structure;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IStructureService
    {

        public Task<DataResponse<IEnumerable<GetDepartmentsWithSpecResponse>>> GetAllCpecialities();
        public Task<DataResponse<IEnumerable<GetAllOptionsResponse>>> GetAllDepartments();

        public Task<BaseResponse> CreateDepartment(CreateDepartmentRequest request);

        public Task<BaseResponse> CreateSpeciality(CreateSpecialityRequest request);

        public Task<BaseResponse> CreateGroup(CreateGroupRequest request);
    }
}
