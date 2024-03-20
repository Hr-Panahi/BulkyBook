using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    // [Area("Admin")] : no need to explicitly write this.
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        
        //Get
        //One action method to handle both Insert and Update => Upsert
        public IActionResult Upsert(int? id)
        {
            Product product = new();

			#region DropDown for Category List and CoverType List
			IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

			IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
	            u => new SelectListItem
	            {
		            Text = u.Name,
		            Value = u.Id.ToString()
	            });
			#endregion

			if (id == null || id == 0)
            {
                //create product
                ViewBag.CategoryList = CategoryList;
                ViewBag.CoverTypeList = CoverTypeList;

                return View(product);
            }
            else
            {
                //update product
            }

            return View(product);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
        public IActionResult Upsert(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save(); //this post to database and saves all the changes
                TempData["sucess"] = "Cover Type updated successfully."; //it stays for 
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDbFirst);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save(); //this post to database and saves all the changes
            TempData["sucess"] = "Cover Type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
