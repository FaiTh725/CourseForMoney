namespace StudentPlacement.Backend.Models.Jwt
{
    public class JwtConfiguration
    {
        public string Audience {  get; set; }

        public string Issuerr { get; set; }

        public string SecretKey { get; set; }
    }
}
