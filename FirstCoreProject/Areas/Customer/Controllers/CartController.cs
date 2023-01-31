using FirstCoreProject.CommonHelper;
using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.DAL.Infrastructure.Repository;
using FirstCoreProject.Models;
using FirstCoreProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstCoreProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CartViewModel cartViewModel { get; set; }    

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Get Current User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            cartViewModel = new CartViewModel()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),


                OrderHeader = new Models.OrderHeader()

            };

          


            foreach (var item in cartViewModel.ListOfCart)
            {
                cartViewModel.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(cartViewModel);
        }

        public IActionResult Summary()
        {
            // Get Current User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            cartViewModel = new CartViewModel()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),


                OrderHeader = new Models.OrderHeader()

            };

            // For Summary Page

            cartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetT(x => x.Id == claims.Value);

            cartViewModel.OrderHeader.Name = cartViewModel.OrderHeader.ApplicationUser.Name;
            cartViewModel.OrderHeader.Phone = cartViewModel.OrderHeader.ApplicationUser.PhoneNumber;

            cartViewModel.OrderHeader.Address = cartViewModel.OrderHeader.ApplicationUser.Address;
            cartViewModel.OrderHeader.State = cartViewModel.OrderHeader.ApplicationUser.State;
            cartViewModel.OrderHeader.City = cartViewModel.OrderHeader.ApplicationUser.City;
            cartViewModel.OrderHeader.PostalCode = cartViewModel.OrderHeader.ApplicationUser.PinCode;


            foreach (var item in cartViewModel.ListOfCart)
            {
                cartViewModel.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(cartViewModel);
        }


        [HttpPost]

        public IActionResult Summary(CartViewModel cartViewModel)
        {
            // Get Current User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
            // Get List Of cart of User
            cartViewModel.ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product");
          
            // Set Default Value
            
            cartViewModel.OrderHeader.OrderStatus = OrderStatus.StatusPending;
            cartViewModel.OrderHeader.PaymentStatus = PaymentStatus.StatusPending;
            cartViewModel.OrderHeader.DateOfOrder = DateTime.Now;
            cartViewModel.OrderHeader.ApplicationUserId = claims.Value;

            // Get Total Order AMount
            foreach (var item in cartViewModel.ListOfCart)
            {
                cartViewModel.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            _unitOfWork.OrderHeader.Add(cartViewModel.OrderHeader);
            _unitOfWork.Save();

            // Get Cart details and addto order details

            foreach(var item in cartViewModel.ListOfCart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = cartViewModel.OrderHeader.Id,
                    Count = item.Count,
                    Price = item.Product.Price

                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();

            }

            // Payment Gateway Ka Code Yha hoga


            // End Gateway


            // Paymnt ke baad status update

            _unitOfWork.OrderHeader.UpdateStatus(cartViewModel.OrderHeader.Id,OrderStatus.StatusApproved,PaymentStatus.StatusApproved);

            // After Order Delete Cart Items 
            _unitOfWork.Cart.DeleteRange(cartViewModel.ListOfCart);
            _unitOfWork.Save();

            return RedirectToAction("Index","Home");
        }


        //public IActionResult ordersuccess(int id)
        //{
        //    var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == id);

        //    // get payment details  // Video No 16


        //}



        public IActionResult plus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);

            _unitOfWork.Cart.IncrementCartItem(cart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);

            if(cart.Count<=1)
            {
                _unitOfWork.Cart.Delete(cart);

                // Update Count/Noof cart item from session cart

                var count = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).ToList().Count-1;

                HttpContext.Session.SetInt32("SessionCart", count);

            }
            else
            {
                _unitOfWork.Cart.DecrementCartItem(cart, 1);

            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult delete(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);

            _unitOfWork.Cart.Delete(cart);
            _unitOfWork.Save();



            // Update Count/Noof cart item from session cart

            var count = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).ToList().Count;

            HttpContext.Session.SetInt32("SessionCart", count);



            return RedirectToAction(nameof(Index));
        }

    }
}
