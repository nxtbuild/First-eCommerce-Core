using FirstCoreProject.DAL.Infrastructure.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace FirstCoreProject.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get Current User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claims!=null)
            {
                if(HttpContext.Session.GetInt32("SessionCart")!=null)
                {
                    return View(HttpContext.Session.GetInt32("SessionCart"));

                }
                else
                {
                    HttpContext.Session.SetInt32("SessionCart", _unitOfWork.Cart.
                        GetAll(x => x.ApplicationUserId == claims.Value).ToList().Count);


                    return View(HttpContext.Session.GetInt32("SessionCart"));

                }

            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            

        }


    }
}
