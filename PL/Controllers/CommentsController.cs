using System;
using System.Diagnostics;
using System.Web.Mvc;
using BL.DTOs.Comments;
using BL.Facades;
using BL.Utils.Claims;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Authenticated + ", " + Claims.Admin)]
    public class CommentsController : Controller
    {
        public AuctionFacade AuctionFacade { get; set; }
        public UserFacade UserFacade { get; set; }

        public ActionResult CreateComment(long auctionId, long? parentId, long returnPage = 1)
        {
            var model = new CommentCreateDTO()
            {
                AuctionId = auctionId,
                ParentId = parentId,
                ReturnPage = returnPage
            };

            return View("CreateComment", model);
        }

        [HttpPost]
        public ActionResult CreateComment(CommentCreateDTO model)
        {
            model.Time = DateTime.Now;
            var user = UserFacade.GetUser(User.Identity);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return View("CreateComment", model);
            }


            model.AuthorId = user.ID;
            if (!ModelState.IsValid)
            {
                return View("CreateComment", model);
            }
            try
            {
                AuctionFacade.CreateComment(model);
                return RedirectToAction("Details", "Auctions", new {id = model.AuctionId, page = model.ReturnPage});
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
                return View("CreateComment", model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Claims.Admin)]
        public ActionResult DeleteComment(long auctionId, long id, int returnPage = 1)
        {
            try
            {
                AuctionFacade.DeleteComment(id);
                TempData["MessageTitle"] = "Comment deleted.";
            }
            catch (FormatException ex)
            {
                TempData["ErrorMessageTitle"] = "Deletion failed: ";
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (NullReferenceException ex)
            {
                TempData["ErrorMessageTitle"] = "Deletion failed: ";
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Details", "Auctions", new {id = auctionId, page = returnPage});
        }
    }
}