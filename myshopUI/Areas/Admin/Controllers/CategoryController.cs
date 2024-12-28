using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess;
using myshop.Entities.Models;
using myshop.Entities.Repositiries;

namespace myshopUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Complete();
            TempData["Create"] = "Data has been created succesfully";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var catergoryInDb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(catergoryInDb);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                //_context.SaveChanges();
                TempData["Update"] = "Data has been Updated succesfully";
                return RedirectToAction("Index");

            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryInDb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(categoryInDb);

        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var categoryInDb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            if (categoryInDb == null)
            {
                NotFound();
            }
            _unitOfWork.Category.Remove(categoryInDb);
            _unitOfWork.Complete();
            //_context.SaveChanges();
            TempData["Delete"] = "Data has been deleted succesfully";
            return RedirectToAction("Index");


        }
    }

}
