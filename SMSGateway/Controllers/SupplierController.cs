using SMSGateway.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSGateway.Controllers
{
    public class SupplierController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();
        // GET: Supplier
        public ActionResult _SupplierList()
        {
            var model = db.Suppliers.ToList();
            return PartialView(model);
        }

        public ActionResult _AddSupplier()
        {
            return PartialView();
        }

        public ActionResult AddSupplier(Suppliers supplier)
        {
            supplier.Status = 1;
            db.Suppliers.Add(supplier);
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>Supplier has been added successfully!</text>", JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult _ConfirmRemove(int id)
        {
            ViewBag.Supplierid = id;
            return PartialView();
        }

        public string _RemoveSupplier(int id)
        {
            var row = db.Suppliers.Where(x => x.SupplierId == id).SingleOrDefault();
            db.Suppliers.Remove(row);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return "<text class=" + "text-success" + ">The selected supplier has been removed successfully</text>";
        }

        public ActionResult _EditSupplier(int id)
        {
            var model = db.Suppliers.Where(x => x.SupplierId == id).SingleOrDefault();

            return PartialView(model);
        }

        public ActionResult EditSupplier(Suppliers supplier)
        {
            if (supplier._Status == true)
            {
                supplier.Status = 1;
            }
            else
            {
                supplier.Status = 0;
            }
            db.Entry(supplier).State = EntityState.Modified;
            try
            {
                db.SaveChanges();

            }
            catch(Exception ex)
            {
                return Json("<text class='text-danger'>" + ex.Message + "</text>", JsonRequestBehavior.AllowGet);
            }
            return Json("<text class='text-success'>Supplier has been edited successfully!</text>", JsonRequestBehavior.AllowGet);

        }
           
    }
}