namespace StudentPlacement.Backend.Domain.Enums
{
    public enum StatusCode
    {
        Ok,
        ServerError,

        UnAuthorize,
        UserExist,

        NotFoundUser,
        InvaludDataforUpdate,

        InvalidToken,

        StudentExist,

        OrganizatinExist
    }
}
