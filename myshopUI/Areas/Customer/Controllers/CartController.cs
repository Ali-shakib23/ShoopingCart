using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repositiries;
using myshop.Entities.ViewModels;
using System.Security.Claims;

namespace myshopUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCartVM { get; set; }


        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM()
            {
                CartLists = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserID == claim.Value, IncludeWord: "Product")
            };
            foreach (var item in shoppingCartVM.CartLists)
            {
                shoppingCartVM.TotalCarts += (item.Count * item.Product.Price);
            }
            return View(shoppingCartVM);
        }

        public IActionResult Plus(int cartid)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.ShoppingCartID == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingCart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("index");
        }

        public IActionResult Minus(int cartID)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.ShoppingCartID == cartID);

            if (shoppingCart.Count < 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.ShoppingCart.DecreaseCount(shoppingCart, 1);
            }
            
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartID)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.ShoppingCartID == cartID);

            _unitOfWork.ShoppingCart.Remove(shoppingCart);

            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
