using FirstCoreProject.CommonHelper;
using FirstCoreProject.DAL;
using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using FirstCoreProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =WebRole.Role_Admin)]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryViewModel category_vm = new CategoryViewModel();

            category_vm.categories = _unitOfWork.Category.GetAll(); 
            return View(category_vm);
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
            CategoryViewModel category_vm = new CategoryViewModel();
            if (id == null || id == 0)
            {
                return View(category_vm);
            }
            else
            {
                category_vm.Category = _unitOfWork.Category.GetT(x => x.Id == id);

                if (category_vm.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category_vm);
                }
            }
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryViewModel category_vm)
        {
            if (ModelState.IsValid)
            {
                if(category_vm.Category.Id==0)
                {
                    _unitOfWork.Category.Add(category_vm.Category);
                    TempData["success"] = "Data has been saved.";
                }
                else
                {
                    _unitOfWork.Category.Update(category_vm.Category);
                    TempData["success"] = "Data has been updated.";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category =_unitOfWork.Category.GetT(x=>x.Id==id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();

            TempData["success"] = "Data has been deleted.";

            return RedirectToAction("Index");
        }
    }
}
