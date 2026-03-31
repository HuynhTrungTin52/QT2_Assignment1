using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class PaymentController : Controller
    {
        private TicketingContext db = new TicketingContext();

        public ActionResult PaymentForm(int ticketId)
        {
            var ticket = db.Tickets.Include("Destination").FirstOrDefault(t => t.TicketID == ticketId);
            if (ticket == null) return HttpNotFound();

            return View(ticket); 
        }

        [HttpPost]
        public ActionResult ProcessPayment(int ticketId, string paymentMethod)
        {
            var ticket = db.Tickets.Find(ticketId);
            if (ticket == null) return HttpNotFound();

            Transaction trans = new Transaction
            {
                TicketID = ticketId,
                Amount = ticket.FinalPrice,
                PaymentMethod = paymentMethod
            };
            db.Transactions.Add(trans);

            // MOCK VALIDATION: Assume 90% success rate for the demo
            bool isPaymentValid = new Random().Next(1, 10) > 1;

            if (isPaymentValid)
            {
                trans.MarkAsSuccess();
                db.SaveChanges();

                return RedirectToAction("PrintTicket", new { ticketId = ticket.TicketID });
            }
            else
            {
                trans.MarkAsFailed();
                db.SaveChanges();

                TempData["ErrorMessage"] = "Payment declined. Please try again.";
                return RedirectToAction("PaymentForm", new { ticketId = ticket.TicketID });
            }
        }
        public ActionResult PrintTicket(int ticketId)
        {
            var ticket = db.Tickets.Include("Destination").FirstOrDefault(t => t.TicketID == ticketId);
            return View(ticket); // We will build the physical ticket View later
        }
    }
}