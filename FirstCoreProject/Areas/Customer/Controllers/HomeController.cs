using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using FirstCoreProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FirstCoreProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");

            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int? ProductId)
        {
            Cart cart = new Cart()
            {
                Product = _unitOfWork.Product.GetT(x => x.Id == ProductId, includeProperties: "Category"),

                Count = 1, // Default Me 1 Set Hogi
                ProductId = (int)ProductId

            };
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]              // ye Current State ko Validate nhi krta Model state ko krta h
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claims.Value;


                // Agar Cart Me Product h To Qty+ Hogi
                var cartItem = _unitOfWork.Cart.GetT(x => x.ProductId == cart.ProductId &&
                x.ApplicationUserId == claims.Value);

                if (cartItem == null)
                {
                    // Add
                    _unitOfWork.Cart.Add(cart);
                    _unitOfWork.Save();

                    // Cart me add hone ke baad ye incremented count show ho jaye  jo upr cart ke number show hota h wahi
                    HttpContext.Session.SetInt32("SessionCart", _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value).ToList().Count);

                }
                else
                {
                    // Update Qty  

                    _unitOfWork.Cart.IncrementCartItem(cartItem,cart.Count);
                    _unitOfWork.Save();
                }

                _unitOfWork.Save();

                TempData["success"] = "Item added to cart.";
            }

            return RedirectToAction("Index");
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}