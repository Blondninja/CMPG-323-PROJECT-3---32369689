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
    public class ZonesController : Controller
    {
        private readonly IZonesRepository _zonesRepository;
        public ZonesController(IZonesRepository zonesRepository)
        {
            _zonesRepository = zonesRepository;
        }


        //  get methods for zones 
        public async Task<IActionResult> Index()
        {
            await _zonesRepository.SaveChanges();
            return View(_zonesRepository.GetAll()); 
        }

        // GET method that details all zones  entries  from the database
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zone = _zonesRepository.GetById((Guid)id);
            if (zone == null)
            {
                return NotFound();
            }

            await _zonesRepository.SaveChanges();
            return View(zone);
        }

        // GET method that creates all zones  entries  from the database
        public IActionResult Create()
        {
            return View();
        }

        //  post  method that will create a new zone  entry on the database

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZoneId,ZoneName,ZoneDescription,DateCreated")] Zone zone)
        {
            zone.ZoneId = Guid.NewGuid();
            _zonesRepository.Add(zone);

            await _zonesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //  GET method that edits  all zones  entries  from the database
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zone = _zonesRepository.GetById((Guid)id);
            if (zone == null)
            {
                return NotFound();
            }

            await _zonesRepository.SaveChanges();
            return View(zone);
        }

        //  post  method that will edit a new zone  entry on the database

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ZoneId,ZoneName,ZoneDescription,DateCreated")] Zone zone)
        {
            if (id != zone.ZoneId)
            {
                return NotFound();
            }

            try
            {
                _zonesRepository.Update(zone);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(zone.ZoneId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await _zonesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        //  GET method that delete all zones  entries  from the database
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zone = _zonesRepository.GetById((Guid)id);
            if (zone == null)
            {
                return NotFound();
            }
            await _zonesRepository.SaveChanges();
            return View(zone);
        }

        //  post  method that will delete a new zone  entry on the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var zone = _zonesRepository.GetById((Guid)id);
            _zonesRepository.Remove(zone);

            await _zonesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ZoneExists(Guid id)
        {
            return _zonesRepository.Exists(id);
        }
    }
}
