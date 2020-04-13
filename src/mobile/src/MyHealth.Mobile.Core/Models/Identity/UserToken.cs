namespace MyHealth.Mobile.Core.Models.Identity
{
    public class UserToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}
