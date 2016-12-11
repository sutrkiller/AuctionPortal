using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using DAL.Entities;
using UserAccount = Autentization.UserAccount;

namespace BL.Services.Users
{
    public class AuthenticationWrapper : AuthenticationService<UserAccount>
    {
        #region tokenActions
        private Action<ClaimsPrincipal, TimeSpan?, bool?> _issueTokenAction;

        private Action _revokeTokenAction;
        #endregion

        public AuthenticationWrapper(UserAccountService<UserAccount> userService)
            : base(userService) { }

        public AuthenticationWrapper(UserAccountService<UserAccount> userService, ClaimsAuthenticationManager claimsAuthenticationManager) : base(userService, claimsAuthenticationManager) { }

        #region TokensManagement
        public void InitializeIssueTokenAction(Action<ClaimsPrincipal, TimeSpan?, bool?> action)
        {
            if (_issueTokenAction == null)
            {
                _issueTokenAction = action;
            }
        }

        public void InitializeRevokeTokenAction(Action action)
        {
            if (_revokeTokenAction == null)
            {
                _revokeTokenAction = action;
            }
        }

        protected override ClaimsPrincipal GetCurrentPrincipal()
        {
            return ClaimsPrincipal.Current;
        }

        protected override void IssueToken(ClaimsPrincipal principal, TimeSpan? tokenLifetime = null, bool? persistentCookie = null)
        {
            if (_issueTokenAction == null)
            {
                throw new InvalidOperationException("Issue token action has not been initialized yet!");
            }
            _issueTokenAction.Invoke(principal, tokenLifetime, persistentCookie);
        }

        protected override void RevokeToken()
        {
            if (_revokeTokenAction == null)
            {
                throw new InvalidOperationException("Issue token action has not been initialized yet!");
            }
            _revokeTokenAction.Invoke();
        }
        #endregion

        #region SignInManagement
        public void PerformSignIn(Guid userId, bool rememberMe)
        {
            SignIn(userId, rememberMe);
        }

        public void PerformSignOut()
        {
            SignOut();
        }
        #endregion
    }
}
