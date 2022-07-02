using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping // sumana
        private ECartDbEntities objEcartDbEntities;
        private List<ShoppingCartModel> listOfShoppingCartModel;
        public ShoppingController()
        {
            objEcartDbEntities = new ECartDbEntities();
            listOfShoppingCartModel = new List<ShoppingCartModel>();
        }
        public ActionResult Index()
        {
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objEcartDbEntities.Items
                                                                       join
                                                                       objCate in objEcartDbEntities.Categories
                                                                       on objItem.CategoryId equals objCate.CategoryId
                                                                       select new ShoppingViewModel()
                                                                       {
                                                                           ImagePath = objItem.ImagePath,
                                                                           ItemPrice = objItem.ItemPrice,
                                                                           ItemName = objItem.ItemName,
                                                                           ItemId = objItem.ItemId,
                                                                           Description = objItem.Description,
                                                                           Category = objCate.CategoryName,
                                                                           ItemCode = objItem.ItemCode,
                                                                       }
                                                                       ).ToList();
                                                                         
            return View(listOfShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string ItemId)
        {
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Item objItem = objEcartDbEntities.Items.Single(model => model.ItemId.ToString() == ItemId);

            if (Session["CartCounter"] !=null)
            {
                listOfShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            }

            if (listOfShoppingCartModel.Any(model => model.ItemId == ItemId))
                {
                objShoppingCartModel = listOfShoppingCartModel.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;

            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                listOfShoppingCartModel.Add(objShoppingCartModel);

            }
            Session["CartCounter"] = listOfShoppingCartModel.Count;
            Session["CartItem"] = listOfShoppingCartModel;


            

            return Json(new { Success = true, Counter = listOfShoppingCartModel.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModel);
        }

        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderId = 0;
            listOfShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };
            objEcartDbEntities.Orders.Add(orderObj);
            objEcartDbEntities.SaveChanges();
            OrderId = orderObj.OrderId;

            foreach (var item in listOfShoppingCartModel)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objEcartDbEntities.OrderDetails.Add(objOrderDetail);
                objEcartDbEntities.SaveChanges();
            }

            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }
    }
}