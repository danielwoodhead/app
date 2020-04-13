using IdentityModel.Client;
using MyHealth.Mobile.Core.Models.Identity;

namespace MyHealth.Mobile.Core.Extensions
{
    public static class TokenResponseExtensions
    {
        public static UserToken ToUserToken(this TokenResponse tokenResponse)
        {
            return new UserToken
            {
                AccessToken = tokenResponse.AccessToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                IdentityToken = tokenResponse.IdentityToken,
                RefreshToken = tokenResponse.RefreshToken,
                TokenType = tokenResponse.TokenType
            };
        }
    }
}
