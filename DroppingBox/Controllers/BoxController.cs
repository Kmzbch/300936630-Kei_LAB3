using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DroppingBox.Models;
using DroppingBox.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using File = DroppingBox.Models.File;

namespace DroppingBox.Controllers
{
    public class BoxController : Controller
    {
        // field
        string accessId = "AKIAWY236QUIWBQ5VJMF";
        string secretKey = "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";
        private S3Manager s3Manager;
        public string bucketName = "comp306003lab3bucket";

        private IUserRepository iuserRepository;

        // constructers
        public BoxController(IUserRepository iUserRepository)
        {
            this.iuserRepository = iUserRepository;

            // create S3 bucket
            this.s3Manager = new S3Manager(accessId, secretKey, Amazon.RegionEndpoint.USEast1);

            bool isSuccess;
            Task.Run(async () =>
            {
                isSuccess = await s3Manager.CreateBucket(bucketName);
            }).Wait();
        }

        //
        public async Task<ActionResult >Index()
        {

            User user = await iuserRepository.GetByEmail(
                HttpContext.Session.GetString("loggedInUser")
                );

            return View(user.Files);
        }

        //
        public ActionResult Details(int id)
        {
            // File file = IFileRepository.getById(id);

            File file = new File { FileId = "1", FileName = "File 1", FileLink = null, Comment = "comment 1" };

            return View(file);
        }

        // upload actions
        public ActionResult Upload()
        {
            UploadModel model = new UploadModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(UploadModel model)
        {

            if (model.FormFile?.Length > 0)
            {
                var fileName = Path.GetFileName(model.FormFile.FileName);

                //byte[] fileBytes;
                //using (var fileStream = model.FormFile.OpenReadStream())
                //using (var ms = new MemoryStream())
                //{
                //    fileStream.CopyTo(ms);
                //    fileBytes = ms.ToArray();
                //}

                //var fileName = Path.GetFileName(model.FormFile.FileName);
                //var fileMimeType = model.FormFile.ContentType;
                //var fileContent = fileBytes;

                try
                {
                    // upload to S3
                    //                    s3Manager.AddObject();

                    File newFile = new File()
                    {
                        FileId = Guid.NewGuid().ToString(),
                        FileName = model.FormFile.FileName,
                        Comment = model.Comment
                    };

                    User user = await iuserRepository.GetByEmail(
                        HttpContext.Session.GetString("loggedInUser")
                        );

                    user.Files.Add(newFile);

                    await iuserRepository.Create(user);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "Invalid file or file name");

            return View(model);
        }

        // edit actions
        public ActionResult Edit(int id)
        {
            return View();
        }

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



        // delete actions
        public async Task<ActionResult> Delete(String id)
        {
            try
            {
                User user = await iuserRepository.GetByEmail(
    HttpContext.Session.GetString("loggedInUser")
    );

                user.Files.RemoveAt(user.Files.FindIndex(f => f.FileId == id));
                await iuserRepository.Update(user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //// delete actions
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{

        //    try
        //    {
        //        //            s3Manager.remove object
        //        // iFileRepolsitory.delete(id)

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
