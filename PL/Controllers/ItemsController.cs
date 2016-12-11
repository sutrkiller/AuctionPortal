using System;
using System.Linq;
using System.Web.Mvc;
using BL.Facades;
using PL.Models;

namespace PL.Controllers
{
    public class ItemsController : Controller
    {
        public AuctionFacade AuctionFacade { get; set; }

        public ActionResult Details(long id)
        {
            try
            {
                var item = AuctionFacade.GetItem(id);
                var images = AuctionFacade.GetImagesForItem(item.ID).ToList();
                var model = new ItemViewModel()
            {
                Item = item,
                Images = images
            };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Auctions");
            }
           
        }
    }
}