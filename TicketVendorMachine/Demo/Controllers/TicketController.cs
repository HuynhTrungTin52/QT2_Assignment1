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
        public ActionResult CreateTicket(int destinationId, bool isStudent, int ticketQuantity = 1)
        {
            var destination = db.Destinations.Find(destinationId);
            if (destination == null) return HttpNotFound();

            decimal effectiveFare = destination.GetEffectiveBaseFare();

            Ticket newTicket = new Ticket
            {
                DestinationID = destinationId,
                IsStudentTicket = isStudent
            };

            newTicket.CalculateFinalPrice(effectiveFare);
            newTicket.FinalPrice = newTicket.FinalPrice * ticketQuantity;

            newTicket.GenerateBarcode();

            db.Tickets.Add(newTicket);
            db.SaveChanges();

            return RedirectToAction("PaymentForm", "Payment", new { ticketId = newTicket.TicketID });
        }
        [HttpPost]
        public ActionResult Confirm(int destinationId)
        {
            var destination = db.Destinations.Find(destinationId);
            if (destination == null) return HttpNotFound();

            var allStations = db.Destinations.Where(d => d.DestinationID != destinationId).ToList();
            var random = new Random();
            var randomDeparture = allStations[random.Next(allStations.Count)];

            ViewBag.DepartureStation = randomDeparture.StationName;

            return View(destination); 
        }
    }
}