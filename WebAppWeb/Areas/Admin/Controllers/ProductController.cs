using Microsoft.AspNetCore.Mvc;
using MyApp.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;
using MyApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #region APICall
        [HttpGet]
        public IActionResult AllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "category");
            return Json(new { data = products });
        } 

        #endregion

        public IActionResult Index()
        {
            //ProductVM productVM = new ProductVM();
            //productVM.Products = _unitOfWork.Product.GetAll();
            return View();
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitOfWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };

            if (id == null || id == 0)
            {
                return View(vm);
            }
            //Edit
            else
            {
                vm.Product = _unitOfWork.Product.Get(x => x.Id == id);
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImage");
                    filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filepath = Path.Combine(uploadDir, filename);
                    if(vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = @"\ProductImage\" + filename;
                }

                if (vm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(vm.Product);
                    TempData["Success"] = "Product Created Done!";
                }
                else
                {
                    _unitOfWork.Product.Update(vm.Product);
                    TempData["Success"] = "Product updated Done!";
                }
                _unitOfWork.save();

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
        //    var ProductFromDb = _unitOfWork.Product.Get(x => x.Id == id);
        //    if (ProductFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFromDb);
        //}

        #region Delete API
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductFromDb = _unitOfWork.Product.Get(x => x.Id == id);
            if (ProductFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, ProductFromDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.Delete(ProductFromDb);
                _unitOfWork.save();
                return Json(new { success = true, message = "Delete Successfully" });
            }
        }
        #endregion
    }
}
