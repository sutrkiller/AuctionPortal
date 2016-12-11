using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using BL.Services.Users;

namespace PL.Helpers.Auth
{
    public class SignInManager
    {
        private readonly AuthenticationWrapper _authenticationWrapper;

        #region Actions
        private readonly Action<ClaimsPrincipal, TimeSpan?, bool?> _issueTokenAction =
         (principal, tokenLifetime, persistentCookie) =>
         {

             if (principal == null) throw new ArgumentNullException(nameof(principal));

             if (tokenLifetime == null)
             {
                 var handler = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[typeof(SessionSecurityToken)] as SessionSecurityTokenHandler;
                 if (handler == null)
                 {
                     Tracing.Verbose("[SamAuthenticationService.IssueToken] SessionSecurityTokenHandler is not configured");
                     throw new Exception("SessionSecurityTokenHandler is not configured and it needs to be.");
                 }

                 tokenLifetime = handler.TokenLifetime;
             }

             if (persistentCookie == null)
             {
                 persistentCookie = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.PersistentCookiesOnPassiveRedirects;
             }

             var sam = FederatedAuthentication.SessionAuthenticationModule;
             if (sam == null)
             {
                 Tracing.Verbose("[SamAuthenticationService.IssueToken] SessionAuthenticationModule is not configured");
                 throw new Exception("SessionAuthenticationModule is not configured and it needs to be.");
             }

             var token = new SessionSecurityToken(principal, tokenLifetime.Value)
             {
                 IsPersistent = persistentCookie.Value,
                 IsReferenceMode = sam.IsReferenceMode
             };

             sam.WriteSessionTokenToCookie(token);

             Tracing.Verbose("[SamAuthenticationService.IssueToken] cookie issued: {0}", principal.Claims.GetValue(ClaimTypes.NameIdentifier));
         };

        private readonly Action _revokeTokenAction =
            () =>
            {

                var sam = FederatedAuthentication.SessionAuthenticationModule;
                if (sam == null)
                {
                    Tracing.Verbose("[SamAuthenticationService.RevokeToken] SessionAuthenticationModule is not configured");
                    throw new Exception("SessionAuthenticationModule is not configured and it needs to be.");
                }

                sam.SignOut();
            };
        #endregion

        public SignInManager(AuthenticationWrapper authWrapper)
        {
            _authenticationWrapper = authWrapper;

            _authenticationWrapper.InitializeIssueTokenAction(_issueTokenAction);
            _authenticationWrapper.InitializeRevokeTokenAction(_revokeTokenAction);
        }

        public void SignIn(Guid userId, bool rememberMe)
        {
            _authenticationWrapper.PerformSignIn(userId, rememberMe);
        }

        public void SignOut()
        {
            _authenticationWrapper.PerformSignOut();
        }
    }
}