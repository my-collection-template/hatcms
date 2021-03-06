using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HatCMS.Placeholders;
using System.Collections.Generic;
using Hatfield.Web.Portal;
using System.Text;
using System.Globalization;
using System.Threading;

namespace HatCMS.Placeholders.Procurement
{
    public class ProcurementDetails : BaseCmsPlaceholder
    {
        public override CmsDependency[] getDependencies()
        {
            List<CmsDependency> ret = new List<CmsDependency>();

            // -- CKEditor dependencies
            ret.AddRange(CKEditorHelpers.CKEditorDependencies);

            // -- database tables
            ret.Add(new CmsDatabaseTableDependency(@"
                CREATE TABLE `ProcurementAggregator` (
                  `PageId` int(10) unsigned NOT NULL,
                  `Identifier` int(10) unsigned NOT NULL,
                  `LangCode` varchar(5) NOT NULL,
                  `DefaultYearToDisplay` int(11) NOT NULL,
                  `Deleted` datetime DEFAULT NULL,
                  PRIMARY KEY (`PageId`,`Identifier`,`LangCode`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;
            "));
            ret.Add(new CmsDatabaseTableDependency(@"
                CREATE TABLE `ProcurementDetails` (
                  `PageId` int(10) unsigned NOT NULL,
                  `Identifier` int(10) unsigned NOT NULL,
                  `LangCode` varchar(5) NOT NULL,
                  `DateOfProcurement` datetime DEFAULT NULL,
                  `Deleted` datetime DEFAULT NULL,
                  PRIMARY KEY (`PageId`,`Identifier`,`LangCode`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;
            "));

            // -- REQUIRED config entries
            ret.Add(new CmsConfigItemDependency("Procurement.ReadArticleText"));
            ret.Add(new CmsConfigItemDependency("Procurement.NoProcurementText"));
            ret.Add(new CmsConfigItemDependency("Procurement.NoProcurementForText"));

            return ret.ToArray();
        }

        public override RevertToRevisionResult RevertToRevision(CmsPage oldPage, CmsPage currentPage, int[] identifiers, CmsLanguage language)
        {
            return RevertToRevisionResult.NotImplemented; // this placeholder doesn't implement revisions
        }

        public override void RenderInEditMode(HtmlTextWriter writer, CmsPage page, int identifier, CmsLanguage langToRenderFor, string[] param)
        {
            // CmsContext.setCurrentCultureInfo(langToRenderFor);
            ProcurementDb db = new ProcurementDb();
            ProcurementDb.ProcurementDetailsData entity = new ProcurementDb.ProcurementDetailsData(page, identifier, langToRenderFor);
            string dateString = "";
            string editId = "ProcurementDetails_" + page.Id.ToString() + "_" + identifier.ToString() + "_" + langToRenderFor.shortCode;

            // ------- CHECK THE FORM FOR ACTIONS
            string action = PageUtils.getFromForm(editId + "_Action", "");
            if (action.Trim().ToLower() == "update")
            {
                dateString = PageUtils.getFromForm("dateOfProcurement_" + editId, "");
                try
                {
                    entity.DateOfProcurement = Convert.ToDateTime(dateString);
                }
                catch { }
                db.updateProcurementDetails(page, identifier, langToRenderFor, entity);
            }
            else
            {
                entity = db.fetchProcurementDetails(page, identifier, langToRenderFor, true);
                dateString = entity.DateOfProcurement.ToString("d");
            }

            // ------- START RENDERING
            StringBuilder arg0 = new StringBuilder();
            arg0.Append("<div style=\"width: 100%\">");
            arg0.Append("<p>Date of Procurement (" + CmsLanguage.CurrentShortDateFormat + "): ");
            arg0.Append(PageUtils.getInputTextHtml("dateOfProcurement_" + editId,"dateOfProcurement_" + editId, dateString, 10, 10));
            arg0.Append("</p>");

            arg0.Append("<input type=\"hidden\" name=\"" + editId + "_Action\" value=\"update\">");
            arg0.Append("</div>");

            writer.WriteLine(arg0.ToString());
        }

        public override void RenderInViewMode(HtmlTextWriter writer, CmsPage page, int identifier, CmsLanguage langToRenderFor, string[] param)
        {
            // CmsContext.setCurrentCultureInfo(langToRenderFor);
            StringBuilder html = new StringBuilder();

            ProcurementDb db = new ProcurementDb();
            ProcurementDb.ProcurementDetailsData Procurement = db.fetchProcurementDetails(page, identifier, langToRenderFor, true);

            html.Append("<h2>");
            html.Append(Procurement.DateOfProcurement.ToString("MMM d yyyy"));
            html.Append("</h2>");
            writer.Write(html.ToString());
        }

        public override Rss.RssItem[] GetRssFeedItems(CmsPage page, CmsPlaceholderDefinition placeholderDefinition, CmsLanguage langToRenderFor)
        {
            Rss.RssItem rssItem = CreateAndInitRssItem(page, langToRenderFor);
            rssItem.Description = page.renderPlaceholderToString(placeholderDefinition, langToRenderFor, CmsPage.RenderPlaceholderFilterAction.RunAllPageAndPlaceholderFilters);
            return new Rss.RssItem[] { rssItem };
        }
    }
}
