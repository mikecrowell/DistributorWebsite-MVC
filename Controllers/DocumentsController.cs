using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class DocumentsController : Controller
    {
        //#region "FUNCTION: ViewStaticFile"
        ///// <summary>
        ///// View the static file
        ///// </summary>
        ///// <param name="documentType"></param>
        ///// <returns></returns>
        //[HttpPost]
        ////[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[SkipNocacheFilter]
        //[ValidateAntiForgeryToken]
        //public ActionResult ViewStaticFile(String subDir, String fileName, String baseDir = "Reports", Boolean byEntity = false, Boolean byClient = false, Boolean byLanguage = false)
        //{
        //    return GetStaticFileResponse(subDir, fileName, baseDir, byEntity, byClient, byLanguage);
        //}
        //#endregion

        //#region "FUNCTION: ViewStaticFile"
        ///// <summary>
        ///// View the static file (added to support get requests from Android devices)
        ///// </summary>
        ///// <param name="documentType"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[SkipNocacheFilter]
        //public ActionResult ViewStaticFile(String subDir, String fileName, String baseDir = "Reports", Boolean byEntity = false, Boolean byClient = false, Boolean byLanguage = false, String __RequestVerificationToken = "")
        //{
        //    //-- MANUALLY VALIDATE THE ANTI FORGERY TOKEN ON GET REQUESTS --
        //    AntiForgery.Validate(Helpers.CookieInfo.AntiForgeryToken, __RequestVerificationToken);

        //    //-- BUILD THE FILE RESPONSE --
        //    return GetStaticFileResponse(subDir, fileName, baseDir, byEntity, byClient, byLanguage);
        //}
        //#endregion

        //#region "FUNCTION: GetStaticFileResponse"
        ///// <summary>
        ///// Build the file result stream to be returned to the client
        ///// </summary>
        ///// <param name="subDir"></param>
        ///// <param name="fileName"></param>
        ///// <param name="baseDir"></param>
        ///// <param name="byEntity"></param>
        ///// <param name="byClient"></param>
        ///// <param name="byLanguage"></param>
        ///// <returns></returns>
        //private FilePathResult GetStaticFileResponse(String subDir, String fileName, String baseDir = "Reports", Boolean byEntity = false, Boolean byClient = false, Boolean byLanguage = false)
        //{
        //    Models.FileViewModel curFileDefinition = null;
        //    eDirectory baseDirectory;

        //    //-- GET THE BASE DIRECTORY INFORMATION --
        //    if (String.IsNullOrWhiteSpace(baseDir) || !Enum.TryParse<eDirectory>(baseDir, out baseDirectory))
        //    {
        //        throw new HttpException(404, String.Format("Invalid base directory: {0}", baseDir)) { HelpLink = "NOHEADER" };
        //    };

        //    //-- DETERMINE WHICH DOCUMENT THE USER IS TRYING TO VIEW --
        //    curFileDefinition = new Models.FileViewModel("",
        //                                                                baseDirectory,
        //                                                                subDir,
        //                                                                fileName,
        //                                                                Helpers.Documents.GetFileTypeFromExtension(System.IO.Path.GetExtension(fileName)),
        //                                                                eSecurityItem.Unknown,
        //                                                                null,
        //                                                                byLanguage,
        //                                                                byEntity,
        //                                                                byClient);

        //    //-- MAKE SURE THE DOCUMENT EXISTS --
        //    if ((curFileDefinition == null) || (!curFileDefinition.Exists))
        //    {
        //        throw new HttpException(404, "Not Found") { HelpLink = "NOHEADER" };
        //    }

        //    //-- RETURN THE FILE --
        //    Response.Clear();

        //    if (!Helpers.Common.UseInlineDownload)
        //    {
        //        //-- RETURN FILE AS ATTACHMENT CONTENT TYPE --
        //        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", curFileDefinition.FileName.ToUpper()));
        //        return File(curFileDefinition.AbsolutePath, curFileDefinition.ContentType);
        //    }
        //    else
        //    {
        //        //-- RETURN FILE AS INLINE CONTENT TYPE (NOT SUPPORTED ON ANDROID OS) --
        //        Response.AddHeader("Content-Disposition", String.Format("inline; filename=\"{0}\"", curFileDefinition.FileName.ToUpper()));
        //        return File(curFileDefinition.AbsolutePath, curFileDefinition.ContentType);
        //    }
        //}
        //#endregion

        //#region "FUNCTION: ViewDynamicFile"
        ///// <summary>
        ///// View the static file
        ///// </summary>
        ///// <param name="documentType"></param>
        ///// <returns></returns>
        //[SkipNocacheFilter]
        //public ActionResult ViewDynamicFile()
        //{
        //    //-- RETURN THE FILE --
        //    Response.AppendHeader("Content-Disposition", String.Format("inline; filename={0}", "testmemo1.pdf"));
        //    return File(@"\\devglascow\reports\Memos\English\MEMO2654.pdf", "application/pdf");
        //}
        //#endregion
    }
}