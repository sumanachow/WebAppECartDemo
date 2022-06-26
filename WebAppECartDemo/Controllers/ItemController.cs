using System;
using System.Collections.Generic;
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
            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }

        
    }
}