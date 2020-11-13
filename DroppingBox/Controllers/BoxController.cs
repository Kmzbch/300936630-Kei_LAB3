using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DroppingBox.Controllers
{
    public class BoxController : Controller
    {
        // GET: BoxController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BoxController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BoxController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoxController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BoxController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BoxController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BoxController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BoxController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
