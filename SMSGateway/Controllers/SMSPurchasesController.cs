using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSGateway.Controllers
{
    public class SMSPurchasesController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        // GET: SMSPurchases
        public ActionResult _SMSPurchaseList()
        {
            var model = db.SMSPurchases.OrderByDescending(x => x.PurchaseDate).ToList();
            
            foreach (var item in model)
            {
                if (item.CancelledDate != null)
                {
                    DateTime formattedDate = DateTime.Parse(item.CancelledDate);
                    //item.CancelledDate ?? DateTime.Now;
                    item.CancelledDateFormatted = formattedDate;
                }
            }

            return PartialView(model);
        }

        public ActionResult _AddSMSPurchase()
        {
            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            return PartialView();
        }

        public ActionResult _EditSMSPurchase(int recID)
        {
            var model = db.SMSPurchases.Where(x => x.Recid == recID).SingleOrDefault();
            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierId", "SupplierName", model.Supplier);
            ViewBag.Subtotal = model.Quantity * model.Rate;
            ViewBag.Total = (model.Quantity * model.Rate) + (((model.Quantity * model.Rate) * 13) / 100);
            return PartialView(model);
        }

        public ActionResult _CancelSMSPurchase(int recID)
        {
            ViewBag.RecID = recID;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSMSPurchase(SMSPurchases smsPurchases)
        {
            smsPurchases.EntryBy = Session["username"].ToString();
            smsPurchases.EntryDate = DateTime.Now;

            db.SMSPurchases.Add(smsPurchases);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message != null ? ex.Message : ex.InnerException.Message + " </text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>SMS Purchase has been made successfully!</text>", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSMSPurchase(SMSPurchases smsPurchase)
        {
            db.Entry(smsPurchase).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message == null ? ex.InnerException.Message : ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>SMS Purchase has been edited!</text>", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CancelSMSPurchase(int recID)
        {
            var row = db.SMSPurchases.Where(x => x.Recid == recID).Single();

            row.CancelledBy = Session["username"].ToString();

            row.CancelledDate = DateTime.Now.ToString("yyyy-MM-dd");

            db.Entry(row).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message == null ? ex.InnerException.Message : ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>SMS Purchase has been canceled!</text>", JsonRequestBehavior.AllowGet);
        }
    }
}