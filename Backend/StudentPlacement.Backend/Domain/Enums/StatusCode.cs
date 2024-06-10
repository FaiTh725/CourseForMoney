namespace StudentPlacement.Backend.Domain.Enums
{
    public enum StatusCode
    {
        Ok,
        ServerError,
        InvalidEmail,

        UnAuthorize,
        UserExist,

        DepartmentExist,
        NotFoundDepartment,

        SpecializationExist,
        NotFoundSpecialization,

        GroupExist,
        NotFoundGroup,

        NotFoundUser,
        InvaludDataforUpdate,

        InvalidToken,

        StudentExist,
        NotFoundStudent,

        OrganizatinExist,
        NotFoundOrganization,

        NotFountRequest,
        LimitStudent,

        NotFoundFile
    }
}
