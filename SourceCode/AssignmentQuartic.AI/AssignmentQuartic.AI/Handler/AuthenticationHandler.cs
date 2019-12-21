using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MDMWebApi.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> authValues;
            IEnumerable<string> authToken;
            string _userName = null;
            string _issuedToken = null;


            if (!request.Headers.TryGetValues("Authorization", out authValues))
                return base.SendAsync(request, cancellationToken);

            //if (!request.Headers.TryGetValues("token", out authToken))
            //    return base.SendAsync(request, cancellationToken);

            var Credential = authValues.FirstOrDefault();
            // var token = authToken.FirstOrDefault();

            if (!string.IsNullOrEmpty(Credential))
            {
                if (Authenticate(Credential, out _userName, out _issuedToken))
                {
                    Thread.CurrentPrincipal =
                     HttpContext.Current.User =
                     new GenericPrincipal(new GenericIdentity(_userName), new[] { "User" });
                }
            }

            return base.SendAsync(request, cancellationToken);
        }

        protected static bool Authenticate(string authValues, out string _userName, out string _issuedToken)
        {
            bool _result = false;
            string _authUserName = null;
            string _authPasswd = null;
            string _base64Encoded = "";
            byte[] _base64Encodedbits;
            string _decodedValue = "";
            string[] _decodedAuthValues;


            _authUserName = System.Configuration.ConfigurationManager.AppSettings["auth_UserName"];
            _authPasswd = System.Configuration.ConfigurationManager.AppSettings["auth_Password"];

            try
            {
                _base64Encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_authUserName + ":" + _authPasswd));
                _result = Convert.ToString(authValues).Replace("Basic ", "") == _base64Encoded ? true : false;

                _base64Encodedbits = Convert.FromBase64String(Convert.ToString(authValues).Replace("Basic ", ""));
                _decodedValue = Encoding.UTF8.GetString(_base64Encodedbits);

                _decodedAuthValues = _decodedValue.Split(':');

                _userName = _decodedAuthValues.Length > 1 ? Convert.ToString(_decodedAuthValues[0]) : "Anonymous";
                _issuedToken = "";

                return _result;
            }
            catch (Exception)
            {
                _userName = "Anonymous";
                _issuedToken = "";
                return false;
            }
        }
    }
}
