using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        //Get
        //One action method to handle both Insert and Update => Upsert
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
				product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

			if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update product
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(u=>u.Id == id);
                return View(productVM);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
				#region Uploading file
				string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.product.ImageUrl != null) //deleting image if already exists
                    {
                        var oldImagePath = Path.Combine(wwwRootPath,obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                   using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"\images\products\" + fileName + extension;
                }
				#endregion
				
                if(obj.product.Id==0)
                {
                    _unitOfWork.Product.Add(obj.product);

                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);

				}
				_unitOfWork.Save();
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    
        
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

		// POST
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
			if (obj == null)
			{
                return Json(new { success = false, message = "Error while deleting" });
			}

			var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}
			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save(); //this post to database and saves all the changes
			return Json(new { success = true, message = "Delete successful" });

		}

		#endregion
	}
}
