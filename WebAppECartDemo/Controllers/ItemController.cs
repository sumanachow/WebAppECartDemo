using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    public class ItemController : Controller
    {
       private ECartDbEntities objEcartDbEntities;
        public ItemController()
        {
            objEcartDbEntities = new ECartDbEntities();
        }
            
        // GET: Item
        public ActionResult Index()
        {
            ItemViewModel objitemViewModel = new ItemViewModel();

            objitemViewModel.CategorySelectedListItems = (from objcat in objEcartDbEntities.Categories
                                                          select new SelectListItem()
                                                          {
                                                              Text = objcat.CategoryName,
                                                              Value = objcat.CategoryId.ToString(),
                                                              Selected = true


                                                          });
            return View(objitemViewModel);
        }

        [HttpPost]
        public JsonResult Index(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(filename: Server.MapPath("~/Images/" + NewImage));

            Item objItem = new Item();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemId = Guid.NewGuid();
            objItem.ItemPrice = objItemViewModel.ImagePrice;
            objEcartDbEntities.Items.Add(objItem);
            objEcartDbEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }


        
    }
}