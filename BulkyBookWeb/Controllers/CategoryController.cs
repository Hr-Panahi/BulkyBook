
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _db;
        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.GetAll();
            return View(objCategoryList);
        }

        //Get action method
        public IActionResult Create() 
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot match the Name.");
            }
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.Save(); //this post to database and saves all the changes
                TempData["sucess"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
		public IActionResult Edit(int? id)
		{
            if(id== null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);

            if(categoryFromDbFirst == null)
            {
                return NotFound();
            }
			return View(categoryFromDbFirst);
		}

		// POST
		[HttpPost]
		[ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
		public IActionResult Edit(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot match the Name.");
			}
			if (ModelState.IsValid)
			{
				_db.Update(obj);
				_db.Save(); //this post to database and saves all the changes
                TempData["sucess"] = "Category updated successfully."; //it stays for 
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
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        // POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] //to help and prevent cross-site request forgery attack
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Remove(obj);
            _db.Save(); //this post to database and saves all the changes
            TempData["sucess"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
