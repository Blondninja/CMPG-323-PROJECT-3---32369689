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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DeviceManagement_WebApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        public DevicesController(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        // GET: retrieves all Device entries from the database

        public async Task<IActionResult> Index()
        {
            var devices = _devicesRepository.GetAll();
            await _devicesRepository.SaveChanges();
            return View(devices);
        }

        //  GET method that creates all devices entries  from the database
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _devicesRepository.GetById(id);
            if (device == null)
            {
                return NotFound();
            }
            await _devicesRepository.SaveChanges();
            return View(device);
        }

        // GET method Create a new Device entry on the database

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_devicesRepository.GetCategory(), "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(_devicesRepository.GetZone(), "ZoneId", "ZoneName");
            return View();
        }

        // POST method  Create a new Device entry on the database
                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _devicesRepository.Add(device);
            await _devicesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET method  will update an existing Device entry on the database
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _devicesRepository.GetById(id);
            if (device == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_devicesRepository.GetCategory(), "CategoryId", "CategoryName", device.CategoryId);
            ViewData["ZoneId"] = new SelectList(_devicesRepository.GetZone(), "ZoneId", "ZoneName", device.ZoneId);
            await _devicesRepository.SaveChanges();
            return View(device);
        }

        // POST method  will update an existing Device entry on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }
            try
            {
                _devicesRepository.Update(device);
                await _devicesRepository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(device.DeviceId))
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

        //delete an existing Device entry on the database
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _devicesRepository.GetById(id);
            await _devicesRepository.SaveChanges();

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST method  create a new Device entry on the database
                [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var device = _devicesRepository.GetById(id);
            _devicesRepository.Remove(device);
            await _devicesRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(Guid id)
        {
            return _devicesRepository.Exists(id);
        }
    }
}