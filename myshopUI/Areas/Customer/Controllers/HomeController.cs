using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using myshop.Entities.Models;
using myshop.Entities.Repositiries;
using System.Diagnostics;
using System.Security.Claims;


namespace myshopUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Details(int productID)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductID = productID,
                Product = _unitOfWork.Product.GetFirstorDefault(x => x.Id == productID, IncludeWord: "Category"),
                Count = 1
            };
           
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserID = claim.Value;

            ShoppingCart CartObj = _unitOfWork.ShoppingCart.GetFirstorDefault(
                x => x.ApplicationUserID== claim.Value && x.ProductID == shoppingCart.ProductID
            );

            if(CartObj == null)
            {
               _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(CartObj, shoppingCart.Count);
            }
            
            _unitOfWork.Complete();
            return Redirect("Index");
        }

    }
}
