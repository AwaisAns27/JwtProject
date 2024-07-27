namespace JwtProjectEx.ApiResponses
{
    public class LogInUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Flag { get; set; }
    }
}
