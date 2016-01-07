using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiEksamenForberedelse.Models;

namespace WebApiEksamenForberedelse.Controllers
{
    public class AuktionsvarerController : ApiController
    {
        private static List<Auktionsvarer> auktionsvarere = new List<Auktionsvarer>()
        {
            new Auktionsvarer() { Varenummer = "001", Vurdering = 100, BudPris = 100 },
            new Auktionsvarer() { Varenummer = "002", Vurdering = 230, BudPris = 10 },
            new Auktionsvarer() { Varenummer = "003", Vurdering = 10, BudPris = 0 }
        };

        private object _auktionsvarerlock = new object();

        // Fiddler: 
        // http://localhost:42175/api/auktionsvarer/GetAll
        public HttpResponseMessage GetAll()
        {
            var allauktionsvarere = auktionsvarere.ToList();

            lock(_auktionsvarerlock)
            {
                if (allauktionsvarere.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, allauktionsvarere);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }   
            }
        }

        // Fiddler: 
        // http://localhost:42175/api/auktionsvarer/Get?varenummer=001
        public HttpResponseMessage Get(string varenummer)
        {
            var allauktionsvarere = auktionsvarere.ToList();

            lock(_auktionsvarerlock)
            {
                var auktionsvarer = allauktionsvarere.FirstOrDefault(x => x.Varenummer == varenummer);

                if (auktionsvarer != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, auktionsvarer);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Kan ikke finde auktionsvarer " + varenummer);
                }
            }
        }

        // Fiddler: 
        // http://localhost:42175/api/auktionsvarer/Bid?varenummer=001&pris=20000&kundenavn=kurt&kundetelefon=10000
        [HttpPost]
        public HttpResponseMessage Bid(string varenummer, int pris, string kundenavn, string kundetelefon)
        {
            var allauktionsvarere = auktionsvarere.ToList();

            lock(_auktionsvarerlock)
            {
                var auktionsvarer = allauktionsvarere.FirstOrDefault(x => x.Varenummer == varenummer);

                if (auktionsvarer != null)
                {
                    if (auktionsvarer.BudPris < pris)
                    {

                        auktionsvarer.BudPris = pris;
                        auktionsvarer.BudKundeNavn = kundenavn;
                        auktionsvarer.BudKundeTelefon = kundetelefon;
                        auktionsvarer.BudTidspunkt = DateTime.Now;

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Bud under budpris (" + auktionsvarer.BudPris + ")");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Kan ikke finde auktionsvarer " + varenummer);
                }
            }
        }
    }
}
