using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
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

            if (paymentMethod == "QR Code")
            {
                trans.MarkAsSuccess();
                db.SaveChanges();

                string bankBin = "970423";
                string accountNo = "0000 3596 611";
                string accountName = Uri.EscapeDataString("SAIGON METRO KIOSK");
                string paymentInfo = Uri.EscapeDataString($"TICKET {ticketId}");
                string amount = ticket.FinalPrice.ToString("0");

                ViewBag.QrUrl = $"https://img.vietqr.io/image/{bankBin}-{accountNo}-compact2.png?amount={amount}&addInfo={paymentInfo}&accountName={accountName}";
                ViewBag.TicketId = ticketId;
                ViewBag.IsSuccess = true;

                return View("QRCodePayment");
            }


            // Only roll the 80/20 dice if they picked Credit Card
            bool isPaymentValid = new Random().Next(1, 10) > 2;

            if (isPaymentValid) trans.MarkAsSuccess();
            else trans.MarkAsFailed();

            db.SaveChanges();

            if (paymentMethod == "Credit Card")
            {
                ViewBag.TicketId = ticketId;
                ViewBag.IsSuccess = isPaymentValid;
                return View("WaitingForPayment");
            }

            return RedirectToAction("Result", new { ticketId = ticket.TicketID, isSuccess = isPaymentValid });
        }
        [HttpGet]
        public JsonResult CheckPaymentStatus(int ticketId)
        {
            var transaction = db.Transactions.FirstOrDefault(t => t.TicketID == ticketId);

            if (transaction != null && transaction.Status == "Success")
            {
                return Json(new { isPaid = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isPaid = false }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Result(int ticketId, bool isSuccess)
        {
            ViewBag.IsSuccess = isSuccess;

            return View(ticketId);
        }
        public ActionResult PrintTicket(int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);

            if (ticket == null) return HttpNotFound();

            return View(ticket);
        }
    }
}