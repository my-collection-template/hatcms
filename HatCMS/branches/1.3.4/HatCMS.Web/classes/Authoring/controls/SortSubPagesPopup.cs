using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using Hatfield.Web.Portal;

namespace HatCMS.Controls
{
    public class SortSubPagesPopup : BaseCmsControl
    {

        public override CmsDependency[] getDependencies()
        {
            List<CmsDependency> ret = new List<CmsDependency>();
            ret.Add(CmsFileDependency.UnderAppPath("js/_system/sortSelectList.js", CmsDependency.ExistsMode.MustNotExist)); // now an embedded resource            

            return ret.ToArray();
        }

        public override string RenderToString(CmsControlDefinition controlDefnToRender, CmsLanguage langToRenderFor)
        {
            // -- get the target page
            int targetPageId = PageUtils.getFromForm("target", Int32.MinValue);
            if (targetPageId < 0)
            {
                return("Error: invalid target page!");                
            }
            CmsPage page = CmsContext.getPageById(targetPageId);

            if (!page.currentUserCanWrite)
            {
                return("Access Denied");                
            }

            string html = "<Strong>Sort Sub-Pages</Strong><br>";

            // -- process the submitted form
            string action = PageUtils.getFromForm("action", "");
            if (action.ToLower() == "dosort")
            {
                string[] newOrderIds = PageUtils.getFromForm("order");
                // writer.WriteLine(String.Join(",",newOrderIds)+"<p>");
                for (int i = 0; i < newOrderIds.Length; i++)
                {
                    int id = Convert.ToInt32(newOrderIds[i]);
                    CmsPage tempPage = CmsContext.getPageById(id);
                    if (tempPage.ID != -1)
                    {
                        tempPage.setSortOrdinal(i);
                    }
                } // for

                html = html + "<script>" + Environment.NewLine;
                html = html + "function go(url){" + Environment.NewLine;
                html = html + "opener.location.href = url;" + Environment.NewLine;
                html = html + "window.close();\n}";
                html = html + "</script>" + Environment.NewLine;
                html = html + "<p><center>Sub-Pages Successfully Sorted<p>";
                html = html + "<input type=\"button\" value=\"close this window\" onclick=\"go('" + page.Url + "')\">";
                html = html + "</center>";

                return (html);                

            } // if action = doSort


            // -- render the form
            CmsPage currentPage = CmsContext.currentPage;

            currentPage.HeadSection.AddEmbeddedJavascriptFile(JavascriptGroup.ControlOrPlaceholder, typeof(SortSubPagesPopup).Assembly, "sortSelectList.js");


            string formId = "sortSubPagesForm";
            html = html + currentPage.getFormStartHtml(formId, "selectall('order');");
            html = html + "<table align=\"center\" border=\"0\">";
            html = html + "<tr>";
            html = html + "<td valign=\"top\">";

            int size = Math.Min((int)page.ChildPages.Length, 20);
            html = html + "<select name=\"order\" size=\"" + size.ToString() + "\" multiple=\"multiple\" id=\"order\" onmousewheel=\"mousewheel(this);\" ondblclick=\"selectnone(this);\">";
            // <option value="13" id="a01">0Red 1</option>
            foreach (CmsPage childPage in page.ChildPages)
            {
                html = html + "<option value=\"" + childPage.ID + "\">" + childPage.Title + "</option>" + Environment.NewLine;
            }

            html = html + "</select>";
            html = html + "</td>";
            html = html + "<td valign=\"middle\">";
            html = html + "<input type=\"button\" value=\"Move to Top\" onclick=\"top('order');\" style=\"width: 100px;\" /><br><br>";
            html = html + "<input type=\"button\" value=\"Move Up\" onclick=\"up('order');\"  style=\"width: 100px;\" /><br>";
            html = html + "<input type=\"button\" value=\"Move Down\" onclick=\"down('order');\"  style=\"width: 100px;\" /><br><br>";
            html = html + "<input type=\"button\" value=\"Move to Bottom\" onclick=\"bottom('order');\"  style=\"width: 100px;\" /><br><br>";
            html = html + "</td>";
            html = html + "</tr>";
            html = html + "</table>";
            html = html + "<input type=\"hidden\" name=\"action\" value=\"doSort\">";
            html = html + "<input type=\"hidden\" name=\"target\" value=\"" + targetPageId.ToString() + "\">";

            html = html + "<input type=\"submit\" value=\"Save Order\">";
            html = html + currentPage.getFormCloseHtml(formId);


            return html;
        }
        
    }
}
