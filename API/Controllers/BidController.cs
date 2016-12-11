using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.DTOs.Bids;
using BL.DTOs.Filters;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class BidController : ApiController
    {
        public AuctionFacade AuctionFacade { get; set; }

        public IHttpActionResult Post([FromBody] dynamic value)
        {
            try
            {
                BidDTO bid = JsonConvert.DeserializeObject<BidDTO>(value.ToString());
                var success = AuctionFacade.BidOnAuction(bid);
                return success
                    ? (IHttpActionResult)
                    Content(HttpStatusCode.Created, AuctionFacade.GetBids(new BidFilter() {AuctionId = bid.AuctionId}).ResultPage)
                    : BadRequest(
                        $"Bid value ({bid.Value}) is lower than possible minimum for this auction ({AuctionFacade.GetMinimumBid(bid.AuctionId)}).");
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, ActionName("Get"), Route("~/api/Bid/Auction/{id:long:min(1)}")]
        public IHttpActionResult GetAuctionBids(long id)
        {
            return Ok(AuctionFacade.GetBids(new BidFilter() { AuctionId = id }).ResultPage);
        }
    }
}
