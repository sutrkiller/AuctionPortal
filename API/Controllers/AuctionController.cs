using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using BL.DTOs.Auctions;
using BL.DTOs.Filters;
using BL.DTOs.Items;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class AuctionController : ApiController
    {
        public AuctionFacade AuctionFacade { get; set; }

        public IHttpActionResult Get()
        {
            return Ok(AuctionFacade.GetAuctions(null).ResultPage);
        }

        [HttpGet,Route("~/api/Auction/Current")]
        public IHttpActionResult GetCurrentAuctions()
        {
            return Ok(AuctionFacade.GetAuctions(new AuctionFilter {OnlyCurrentAuctions = true}).ResultPage);
        }

        [Route("~/api/Auction/{id:long:min(1)}")]
        public IHttpActionResult Get(long id)
        {
            var result = AuctionFacade.GetAuction(id);
            return result == null ? (IHttpActionResult) NotFound() : Ok(result);
        }

        [HttpGet, Route("~/api/Auction/User/{id:long:min(1)}")]
        [ActionName("Get")]
        public IHttpActionResult GetAuctionsByUser(long id)
        {
            var result = AuctionFacade.GetAuctions(new AuctionFilter() {SellerId = id}).ResultPage;
            return result == null ? (IHttpActionResult) NotFound() : Ok(result);
        }

        public IHttpActionResult Post([FromBody] dynamic value)
        {
            try
            {
                var auction = JsonConvert.DeserializeObject<AuctionCreateDTO>(value.ToString());
                AuctionFacade.CreateAuction(auction,new List<ItemCreateDTO>());
                return Content(HttpStatusCode.Created, AuctionFacade.GetAuctions(null).ResultPage);
            }
            catch (JsonException)
            {
                Debug.WriteLine($"Auction API - Post(...) - failed to deserialize value: {value}");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return NotFound();
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }


        public IHttpActionResult Delete(long id)
        {
            try
            {
                AuctionFacade.DeleteAuction(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return NotFound();
            }
        }

       



    }
}