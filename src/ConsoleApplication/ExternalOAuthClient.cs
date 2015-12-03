using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;

namespace ExactOnline.Client.OAuth
{
    public class ExternalOAuthClient : OAuthClient
    {
        private readonly Uri _redirectUri;
        
        public ExternalOAuthClient(AuthorizationServerDescription serverDescription, string clientId, string clientSecret, Uri redirectUri)
            : base(serverDescription, clientId, clientSecret, redirectUri)
		{
            _redirectUri = redirectUri;
		}

        public bool AuthorizeExternal(ref IAuthorizationState authorization, string refreshToken, string AuthorizationCode)
        {
            if ((authorization == null))
            {
                authorization = new AuthorizationState
                {
                    Callback = _redirectUri,
                    RefreshToken = refreshToken
                };
            }

            bool refreshFailed = false;
            if (AccessTokenHasToBeRefreshed(authorization))
            {
                try
                {
                    refreshFailed = !RefreshAuthorization(authorization);
                }
                catch (ProtocolException)
                {
                    //The refreshtoken is not valid anymore
                }
            }

            if (authorization.AccessToken == null || refreshFailed)
            {
                if (string.IsNullOrEmpty(AuthorizationCode))
                {
                    var uri = GetAuthorizationUri(authorization);
                    int retval = ShellExecute(IntPtr.Zero, "open", uri.ToString(), "", "", ShowCommands.SW_NORMAL).ToInt32();
                    return retval > 32;
                }
                else
                {
                    var uri = new UriBuilder(_redirectUri);
                    uri.AppendQueryArgument("code", AuthorizationCode);
                    ProcessUserAuthorization(uri.Uri, authorization);
                }
            }
            return !refreshFailed;
        }

        private enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);
    }
}
