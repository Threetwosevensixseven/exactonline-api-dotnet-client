using ExactOnline.Client.OAuth;
using System;

namespace ConsoleApplication
{
    public class ExactConnector
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Uri _callbackUrl;
        private readonly UserAuthorization _authorization;
        public string AuthorizationCode { get; set; }

        public virtual string EndPoint
        {
            get
            {
                return "https://start.exactonline.co.uk";
            }
        }

        public ExactConnector(string clientId, string clientSecret, Uri callbackUrl)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _callbackUrl = callbackUrl;
            _authorization = new UserAuthorization();
        }

        public bool RequestAuthorizationCode()
        {
            ExternalUserAuthorizations.Authorize(_authorization, EndPoint, _clientId, _clientSecret, _callbackUrl);
            return true;
        }

        public string GetAccessToken()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthorizationCode)) 
                    UserAuthorizations.Authorize(_authorization, EndPoint, _clientId, _clientSecret, _callbackUrl);
                else ExternalUserAuthorizations.Authorize(_authorization, EndPoint, _clientId, _clientSecret, _callbackUrl, AuthorizationCode);
                return _authorization.AccessToken;
            }
            finally
            {
                AuthorizationCode = null;
            }
        }
    }
}
