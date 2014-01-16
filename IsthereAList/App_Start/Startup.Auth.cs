using IsThereAList.Properties;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IsThereAList
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var msOptions = new MicrosoftAccountAuthenticationOptions()
            {
                ClientId = Settings.Default.MicrosoftClientId,
                ClientSecret = Settings.Default.MicrosoftClientSecret,
                Provider = new MicrosoftAccountAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(new Claim("urn:microsoft:name", context.Identity.FindFirstValue(ClaimTypes.Name)));
                            return Task.FromResult(0);
                        }
                }
            };

            msOptions.Scope.Add("wl.basic");
            msOptions.Scope.Add("wl.emails");
            msOptions.Scope.Add("wl.birthday");
            app.UseMicrosoftAccountAuthentication(msOptions);

            var twOptions = new TwitterAuthenticationOptions
            {
                ConsumerKey = Settings.Default.TwitterKey,
                ConsumerSecret = Settings.Default.TwitterSecret,
                Provider = new TwitterAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                        {
                        context.Identity.AddClaim(new Claim("urn:twitter:name", context.Identity.FindFirstValue(ClaimTypes.Name)));
                        return Task.FromResult(0);
                        }
                }
            };

            app.UseTwitterAuthentication(twOptions);

            var fbOptions = new FacebookAuthenticationOptions
            {
                AppId = Settings.Default.FaceboookAppId,
                AppSecret = Settings.Default.FacebookAppSecret
            };

            fbOptions.Scope.Add("email");
            fbOptions.Scope.Add("user_birthday");
            app.UseFacebookAuthentication(fbOptions);

            var gOptions = new GoogleAuthenticationOptions
            {
                Provider = new GoogleAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("urn:google:givenName", context.Identity.FindFirstValue(ClaimTypes.GivenName)));
                        context.Identity.AddClaim(new Claim("urn:google:surname", context.Identity.FindFirstValue(ClaimTypes.Surname)));
                        context.Identity.AddClaim(new Claim("urn:google:email", context.Identity.FindFirstValue(ClaimTypes.Email)));
                        return Task.FromResult(0);
                    }
                }
            };

            app.UseGoogleAuthentication(gOptions);
        }
    }
}