using FirstCoreProject.CommonHelper;
using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using FirstCoreProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region APICALL

        public IActionResult AllOrders(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            // For Sort order by query string

            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == claims.Value);

            }

            switch (status)
            {
                case "pending":

                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == PaymentStatus.StatusPending);
                    break;

                case "approved":

                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == PaymentStatus.StatusApproved);
                    break;

                case "shipped":

                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == OrderStatus.StatusShipped);
                    break;

                case "underprocess":

                    orderHeaders = orderHeaders.Where(x => x.OrderStatus == OrderStatus.StatusInProcess);
                    break;


                default:
                    break;
            }
           
            return Json(new
            {
                data = orderHeaders
            });

        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult OrderDetails(int id)
        {
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                OrderHeader=_unitOfWork.OrderHeader.GetT(x=>x.Id==id,includeProperties:"ApplicationUser"),
                OrderDetails=_unitOfWork.OrderDetail.GetAll(x=>x.OrderHeaderId==id,includeProperties:"Product")

            };

            return View(orderViewModel);
        }
        [Authorize(Roles = WebRole.Role_Admin+","+WebRole.Role_Employee)]
        [HttpPost]
        public IActionResult OrderDetails(OrderViewModel orderViewModel)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x=>x.Id==orderViewModel.OrderHeader.Id);
            orderHeader.Name = orderViewModel.OrderHeader.Name; 
            orderHeader.Phone = orderViewModel.OrderHeader.Phone;
            orderHeader.Address = orderViewModel.OrderHeader.Address;
            orderHeader.State = orderViewModel.OrderHeader.State;
            orderHeader.City = orderViewModel.OrderHeader.City;
            orderHeader.PostalCode = orderViewModel.OrderHeader.PostalCode;

            if(orderViewModel.OrderHeader.Carrier!=null)
            {
                orderHeader.Carrier = orderViewModel.OrderHeader.Carrier;
            }

            if (orderViewModel.OrderHeader.TrackingNumber != null)
            {
                orderHeader.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;
            }


            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            TempData["success"] = "Infor updated.";

            return RedirectToAction("Orderdetails","Order",new {id=orderViewModel.OrderHeader.Id});
        }

        [Authorize(Roles = WebRole.Role_Admin + "," + WebRole.Role_Employee)]
        public IActionResult InProcess(OrderViewModel orderViewModel)
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderViewModel.OrderHeader.Id,OrderStatus.StatusInProcess);
            _unitOfWork.Save();

            TempData["success"] = "Order status in-process updated.";

            return RedirectToAction("Orderdetails", "Order", new { id = orderViewModel.OrderHeader.Id });
        }
        [Authorize(Roles = WebRole.Role_Admin + "," + WebRole.Role_Employee)]
        public IActionResult Shipped(OrderViewModel orderViewModel)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == orderViewModel.OrderHeader.Id);
            orderHeader.Carrier = orderViewModel.OrderHeader.Carrier;
            orderHeader.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;

            orderHeader.OrderStatus = OrderStatus.StatusShipped;
            orderHeader.DateOfShipping = DateTime.Now;
            


            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            TempData["success"] = "Order status shipped updated.";

            return RedirectToAction("Orderdetails", "Order", new { id = orderViewModel.OrderHeader.Id });
        }
        [Authorize(Roles = WebRole.Role_Admin + "," + WebRole.Role_Employee)]
        public IActionResult CancelOrder(OrderViewModel orderViewModel)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == orderViewModel.OrderHeader.Id);
            if(orderHeader.PaymentStatus==PaymentStatus.StatusApproved)
            {
                // CReate Refund Stripe Gateway

                //var refund = new RefundCreateOptions
                //{
                //    AuthorizationFailureReason = RedunReasons.RequestByCustomer,
                //    PaymentIntent == orderHeader.PaymentIntentId
                //};

                //var service = new RefundService();

                //Refund refund = service.Create(refund);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, OrderStatus.StatusCancelled,OrderStatus.StatusRefund);

            }else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, OrderStatus.StatusCancelled, OrderStatus.StatusRefund);
            }

            _unitOfWork.Save();

            TempData["success"] = "Order status cancelled.";

            return RedirectToAction("Orderdetails", "Order", new { id = orderViewModel.OrderHeader.Id });
        }


        //public IActionResult PayNow(OrderViewModel orderViewModel)
        //{

        //var OrderHeader=_unitOfWork.OrderHeader.GetT(x=>x.Id==orderViewModel.OrderHeader.id,includeProperties:"ApplicationUser");
        //  var      OrderDetails=_unitOfWork.OrderDetail.GetAll(x=>x.OrderHeaderId==orderViewModel.OrderHeader.id,includeProperties:"Product");


        // Same code hoga payment gateway wala jo cart summary me likhayega... Listofcart ki jagh OrderDetail foreach me
        //}

    }
}
