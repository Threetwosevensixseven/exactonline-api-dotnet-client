using DotNetOpenAuth.OAuth2;
using System;

namespace ExactOnline.Client.OAuth
{
    public static class ExternalUserAuthorizations
    {
        private static AuthorizationServerDescription _serverDescription;

        public static void Authorize(UserAuthorization authorization, string website, string clientId, string clientSecret, Uri redirectUri)
        {
            Authorize(authorization, website, clientId, clientSecret, redirectUri, null);
        }

        public static void Authorize(UserAuthorization authorization, string website, string clientId, string clientSecret, Uri redirectUri, string AuthorizationCode)
        {

            if (_serverDescription == null)
            {
                _serverDescription = new AuthorizationServerDescription
                {
                    AuthorizationEndpoint = new Uri(string.Format("{0}/api/oauth2/auth", website)),
                    TokenEndpoint = new Uri(string.Format("{0}/api/oauth2/token", website))
                };
            }
            var oAuthClient = new ExternalOAuthClient(_serverDescription, clientId, clientSecret, redirectUri);
            var authorizationState = authorization.AuthorizationState;
            oAuthClient.AuthorizeExternal(ref authorizationState, authorization.RefreshToken, AuthorizationCode);
            authorization.AuthorizationState = authorizationState;
        }
    }
}