using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class TicketController : Controller
    {
        private TicketingContext db = new TicketingContext();

        public ActionResult Index()
        {
            var destinations = db.Destinations.ToList();
            return View(destinations); 
        }

        [HttpPost]
        public ActionResult CreateTicket(int destinationId, bool isStudent)
        {
            var destination = db.Destinations.Find(destinationId);
            if (destination == null) return HttpNotFound();

            decimal effectiveFare = destination.GetEffectiveBaseFare(); //Will add other parameters in the future

            Ticket newTicket = new Ticket
            {
                DestinationID = destinationId,
                IsStudentTicket = isStudent
            };
            newTicket.CalculateFinalPrice(effectiveFare);
            newTicket.GenerateBarcode();

            db.Tickets.Add(newTicket);
            db.SaveChanges();

            return RedirectToAction("PaymentForm", "Payment", new { ticketId = newTicket.TicketID });
        }
    }
}