using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SelfDiscoveryWebAPI.Infrastructure
{
    public class AuthenticationTokenDelegatingHandler : DelegatingHandler
    {
        private const string HTTP_HEADER_TOKEN = "Authentication-Token";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains(HTTP_HEADER_TOKEN))
            {
                string token = request.Headers.GetValues(HTTP_HEADER_TOKEN).FirstOrDefault();
                GoogleAuthenticate(token);
            }
#if FAKE_TOKEN
            SetPrincipal(new CustomPrincipal("fake_username"));
#endif
            return base.SendAsync(request, cancellationToken);
        }

        private static bool GoogleAuthenticate(string token)
        {
            var url = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", token);
            using (var cli = new WebClient())
            {
                try
                {
                    var json = cli.DownloadString(url);
                    // 		response	"{\n \"issued_to\": \"608941808256-43vtfndets79kf5hac8ieujto8837660.apps.googleusercontent.com\",\n \"audience\": 
                    // \"608941808256-43vtfndets79kf5hac8ieujto8837660.apps.googleusercontent.com\",\n \"user_id\": \"106441154994762710016\",\n 
                    // \"scope\": \"https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile\",\n 
                    // \"expires_in\": 3513,\n \"email\": \"kobelko@gmail.com\",\n \"verified_email\": true,\n \"access_type\": \"online\"\n}\n"	string

                    var jobject = JObject.Parse(json);

                    var audience = jobject.GetValue("audience").Value<string>();
                    var email = jobject.GetValue("email").Value<string>();
                    var verifiedEmail = jobject.GetValue("verified_email").Value<bool>();

                    if (verifiedEmail)
                        SetPrincipal(new CustomPrincipal(email));

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }

    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
#if FAKE_TOKEN
            return true;
#else
            return false;
#endif
        }

        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
            this.UserId = email;
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}