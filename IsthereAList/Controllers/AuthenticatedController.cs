using IsThereAList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsThereAList.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace IsThereAList.Controllers
{
    [Authorize]
    public class AuthenticatedController : Controller
    {
        public ApplicationUser CurrentUser
        {
            get
            {
                return this.Session.GetOrStore<ApplicationUser>(
                                                "currentUser",
                                                () =>
                                                 {
                                                    ApplicationUser clone = null;
                                                    using(var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                                                    {
                                                        var dbUser = um.FindByName(User.Identity.GetUserName());
                                                        if(dbUser != null)
                                                        {
                                                            clone = new ApplicationUser()
                                                            {
                                                                DobDay = dbUser.DobDay,
                                                                DobMonth = dbUser.DobMonth,
                                                                EmailAddress = dbUser.EmailAddress,
                                                                FirstName = dbUser.FirstName,
                                                                Id = dbUser.Id,
                                                                LastName = dbUser.LastName,
                                                                SendEmails = dbUser.SendEmails,
                                                                UserName = dbUser.UserName
                                                            };
                                                        }
                                                    }

                                                    return clone;
                                                }
                );
            }
        }
    }
}