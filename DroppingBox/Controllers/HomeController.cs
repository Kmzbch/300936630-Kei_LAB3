using System;
using System.IO;
using System.Threading.Tasks;
using DroppingBox.Models;
using DroppingBox.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = DroppingBox.Models.File;

namespace DroppingBox.Controllers
{
    public class HomeController : Controller
    {
        // field
        string accessId = "AKIAWY236QUIWBQ5VJMF";
        string secretKey = "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";
        private S3Manager s3Manager;
        public string bucketName = "comp306003lab3bucket";

        private IUserRepository iuserRepository;

        // constructers
        public HomeController(IUserRepository iUserRepository)
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

        // upload actions
        public ActionResult Upload()
        {
            UploadViewModel model = new UploadViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(UploadViewModel model)
        {

            if (model.FormFile?.Length > 0)
            {

                try
                {
                    var fileName = Path.GetFileName(model.FormFile.FileName);

                    string url = await s3Manager.UploadFileAsync(model.FormFile);



                    File newFile = new File()
                    {
                        FileId = Guid.NewGuid().ToString(),
                        FileName = model.FormFile.FileName,
                        FileLink = url,
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
        public async Task<ActionResult> Edit(string id)

        {
            User user = await iuserRepository.GetByEmail(
                HttpContext.Session.GetString("loggedInUser")
                );

            File file = user.Files.Find(f => f.FileId == id);

            EditViewModel model = new EditViewModel() { 
                FileId = file.FileId,
                Comment = file.Comment,
                FileLink = file.FileLink
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await iuserRepository.GetByEmail(
                        HttpContext.Session.GetString("loggedInUser")
                        );

                    user.Files.Find(f=>f.FileId == model.FileId)
                        .Comment = model.Comment;

                    await iuserRepository.Update(user);

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


    }
}
