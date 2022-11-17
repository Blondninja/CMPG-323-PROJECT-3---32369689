using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using DeviceManagement_WebApp.Repository;


namespace DeviceManagement_WebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _categoriesRepository;
        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }


        //  GET method that retrieves all categories entries  from the database
        public async Task<IActionResult> Index()
        {
            return View( _categoriesRepository.GetAll());
        }

        // GET method that retrieves all categories details  entries from the database
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoriesRepository.GetById((Guid)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET method that create all categories  entries from the database
        public IActionResult Create()
        {
            return View();
        }

        //  post  method that will create   a new category entry on the database

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            category.CategoryId = Guid.NewGuid();
             _categoriesRepository.Add(category);
            _categoriesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET method that edits all categories  entries from the database
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _categoriesRepository.GetById((Guid)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // post  method that will edit   a new category entry on the database

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            try
            {
                 _categoriesRepository.Update(category);
                 await _categoriesRepository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET method that delete all categories  entries from the database
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _categoriesRepository.GetById((Guid)id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoriesRepository.SaveChanges();

            return View(category);
        }

        // post  method that will delete  a new category entry on the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = _categoriesRepository.GetById((Guid)id);
             _categoriesRepository.Remove(category);

            await _categoriesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return  _categoriesRepository.Exists(id);
        }
    }
}
