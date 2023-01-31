using FirstCoreProject.CommonHelper;
using FirstCoreProject.DAL;
using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using FirstCoreProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace FirstCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebRole.Role_Admin)]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        #region APICALL

        public IActionResult AllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return Json(new
            {
                data = products
            });

        }

        #endregion

        public IActionResult Index()
        {
            //ProductViewModel product_vm = new ProductViewModel();

            //product_vm.products = _unitOfWork.Product.GetAll(); 
            return View();
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        _unitOfWork.Category.Add(category);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Data has been saved.";

        //        return RedirectToAction("Index");
        //    }

        //    return View(category);
        //}



        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if(id==null || id==0)
        //    {
        //        return NotFound();
        //    }
        //    var category = _unitOfWork.Category.GetT(x=>x.Id==id); 

        //    if(category==null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Category.Update(category);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Data has been updated.";

        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductViewModel product_vm = new ProductViewModel()
            {
                Product = new(),
                Categories = _unitOfWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text=x.Name,
                    Value=x.Id.ToString()
                })

            };
            if (id == null || id == 0)
            {
                return View(product_vm);
            }
            else
            {
                product_vm.Product = _unitOfWork.Product.GetT(x => x.Id == id);

                if (product_vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product_vm);
                }
            }
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductViewModel product_vm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName =String.Empty;
                if (file!=null)
                {
                    string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadDir,fileName);

                    if(product_vm.Product.ImageUrl!=null)
                    {
                        var OldImagePath= Path.Combine(_hostEnvironment.WebRootPath,product_vm.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                  //  product_vm.Product.ImageUrl = filePath;
                    product_vm.Product.ImageUrl = @"\ProductImage\"+fileName;


                }


                if (product_vm.Product.Id==0)
                {
                    _unitOfWork.Product.Add(product_vm.Product);
                    TempData["success"] = "Data has been saved.";
                }
                else
                {
                    _unitOfWork.Product.Update(product_vm.Product);
                    TempData["success"] = "Data has been updated.";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product =_unitOfWork.Product.GetT(x=>x.Id==id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // [HttpPost,ActionName("Delete")]

        #region DeleteAPICALL

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.GetT(x => x.Id == id);

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error in fatching data."

                }); 
            }
            else
            {
                var OldImagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(OldImagePath))
                {
                    System.IO.File.Delete(OldImagePath);
                }

                _unitOfWork.Product.Delete(product);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "Data has been deleted."

                });
            }

        }


        #endregion
    }
}
