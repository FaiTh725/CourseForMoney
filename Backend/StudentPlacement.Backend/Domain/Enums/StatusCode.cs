namespace StudentPlacement.Backend.Domain.Enums
{
    public enum StatusCode
    {
        Ok,
        ServerError,
        InvalidEmail,

        UnAuthorize,
        UserExist,

        NotFoundUser,
        InvaludDataforUpdate,

        InvalidToken,

        StudentExist,
        NotFoundStudent,

        OrganizatinExist,

        NotFountRequest,
        LimitStudent
    }
}
