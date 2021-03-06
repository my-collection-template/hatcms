using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace HatCMS.Placeholders
{
    public class PageFilesItemData
    {
        public int Id = -1;

        /// <summary>
        /// just the filename, without any path information
        /// </summary>
        public string Filename = "";
        public string Title = "";
        public string Author = "";
        public string CreatorUsername = "";
        public string Abstract = "";
        public string AbstractHtml
        {
            get
            {
                string html = Abstract;
                html = html.Replace(Environment.NewLine, "<p>");
                return html;
            }
        }

        public int DetailsPageId = Int32.MinValue;
        public int Identifier = Int32.MinValue;
        public CmsLanguage Lang = CmsConfig.Languages[0];

        public static string getFilenameOnDisk(CmsPage page, int identifier, CmsLanguage language, string userFilename)
        {
            string prependToFilename = "";
            if (CmsConfig.getConfigValue("DMSFileStorageLocationVersion", "V1") == "V1")
                prependToFilename = page.Id.ToString() + identifier.ToString();

            string baseUrl = GetFileStorageDirectoryUrl(page, identifier, language);

            string fn = baseUrl + prependToFilename + userFilename;
            string fnOnDisk = System.Web.Hosting.HostingEnvironment.MapPath(fn);
            return fnOnDisk;
        }

        public static string GetFileStorageDirectoryUrl(CmsPage page, int identifier, CmsLanguage language)
        {
            return FileLibraryDetailsData.getFileStorageFolderUrl(page, identifier, language);
        }

        public string getFilenameOnDisk(CmsPage page, int identifier, CmsLanguage language)
        {
            return getFilenameOnDisk(page, identifier, language, this.Filename);
        }

        public string getDownloadUrl()
        {
            return getDownloadUrl(CmsUrlFormat.RelativeToRoot);
        }

        public string getDownloadUrl(CmsUrlFormat urlFormat)
        {
            CmsPage page = CmsContext.getPageById(this.DetailsPageId);
            string baseUrl = GetFileStorageDirectoryUrl(page, this.Identifier, this.Lang);

            string url = baseUrl + System.IO.Path.GetFileName(getFilenameOnDisk(page, this.Identifier, this.Lang));

            switch (urlFormat)
            {
                case CmsUrlFormat.RelativeToRoot:
                    break;
                case CmsUrlFormat.FullIncludingProtocolAndDomainName:
                    if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Server == null)
                        throw new Exception("getUrlByPagePath() requires a running web request!");

                    System.Web.HttpRequest r = System.Web.HttpContext.Current.Request;


                    string rootUrl = r.Url.Scheme + "://" + r.Url.Host;
                    if (!r.Url.IsDefaultPort)
                        rootUrl += ":" + r.Url.Port.ToString();
                    url = rootUrl + url;
                    break;
                default:
                    throw new ArgumentException("unknown CmsUrlFormat specified!!");
            }


            return url;

        }


        public long FileSize = 0;
        public DateTime lastModified = DateTime.MinValue;

    }
}
