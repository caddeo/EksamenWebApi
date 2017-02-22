using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiEksamenForberedelse.Models;

namespace WebApiEksamenForberedelse.Controllers
{
    public class AuctionsController : ApiController
    {
        private static readonly List<Auction> Auctions = new List<Auction>()
        {
            new Auction(100) { ID = "001", Rating = 100},
            new Auction(10) { ID = "002", Rating = 230},
            new Auction(0) { ID = "003", Rating = 10},
        };

        private object _threadLock = new object();

        // Fiddler: 
        // http://localhost:42175/api/Auctions/GetAll
        public HttpResponseMessage GetAll()
        {
            var auctions = Auctions.ToList();

            lock(_threadLock)
            {
                if (auctions.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }

                return Request.CreateResponse(HttpStatusCode.OK, auctions);
            }
        }

        // Fiddler: 
        // http://localhost:42175/api/Auctions/Get?id=001
        public HttpResponseMessage Get(string id)
        {
            var auctions = Auctions.ToList();

            lock(_threadLock)
            {
                var auction = auctions.FirstOrDefault(x => x.ID == id);

                if (auction == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"\"{id}\" Not Found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, auction);
            }
        }

        // Fiddler: 
        // http://localhost:42175/api/Auctions/Bid?id=001&bid=20000&name=kurt&phone=10000
        [HttpPost]
        public HttpResponseMessage Bid(string id, int bid, string name, string phone)
        {
            var auctions = Auctions.ToList();

            lock(_threadLock)
            {
                var auction = auctions.FirstOrDefault(x => x.ID == id);

                if (auction == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"{id} Not Found");
                }

                if (auction.Bid.Price >= bid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, $"Bid below the lowest bid (your bid: {bid} lowest: {auction.Bid.Price}");
                }

                auction.Bid.Price = bid;
                auction.Bid.Name = name;
                auction.Bid.Phone = phone;
                auction.Bid.Time = DateTime.Now;

                return Request.CreateResponse(HttpStatusCode.OK, "Bid accepted");
            }
        }
    }
}
