using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DroppingBox.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DroppingBox.Controllers
{
    public class BoxController : Controller
    {
        public string BucketName = "";
        private string loggedInEmail = "";
        private List<File> files;

        private AmazonDynamoDBClient client;
        private DynamoDBContext context;


        public BoxController()
        {
            //client = new AmazonDynamoDBClient();
            //context = new DynamoDBContext(client);

            this.loggedInEmail = "cdfray@gmail.com";

            files = new List<File>
            {
                new File{ FileId = "1", FileName = "File 1", FileLink = null, Comment = "comment 1" },
                new File{ FileId = "2", FileName = "File 2", FileLink = null, Comment = "comment 2" }
            };
        }

        public ActionResult Index()
        {
            return View(files);
        }

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
