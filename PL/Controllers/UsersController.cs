using System;
using System.Web.Mvc;
using BL.DTOs.Users;
using BL.Facades;
using BL.Utils.Claims;
using PL.Models;
using X.PagedList;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Admin)]
    public class UsersController : Controller
    {
        public UserFacade UserFacade { get; set; }

        public ActionResult List(int page = 1)
        {
            var users = UserFacade.ListUsers(page);
            var model = InitializeListModel(users);

            return View("List", model);
        }

        private UserListViewModel InitializeListModel(UserListQueryResultDTO users)
        {
            return new UserListViewModel
            {
                Users =
                    new StaticPagedList<UserDTO>(users.ResultPage, users.RequestedPage, UserFacade.UserPageSize,
                        users.TotalResultCount)
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            try
            {
                UserFacade.DeleteUser(id);
                TempData["Message"] = $"User with id {id} deleted.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Deletion failed";
            }
            
            return RedirectToAction("List");
        }
    }
}