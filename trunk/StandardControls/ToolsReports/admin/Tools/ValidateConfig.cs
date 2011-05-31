using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using HatCMS.setup;

namespace HatCMS.Admin
{
    public class ValidateConfig : BaseCmsAdminTool
    {
        public override CmsAdminToolInfo getToolInfo()
        {
            return new CmsAdminToolInfo(CmsAdminToolCategory.Report_Other, AdminMenuTab.Reports, "Validate CMS Config");
        }

        public override CmsDependency[] getDependencies()
        {
            List<CmsDependency> ret = new List<CmsDependency>();
            return ret.ToArray();
        }

        public override string Render()
        {
            StringBuilder html = new StringBuilder();
            setupPage.ConfigValidationMessage[] msgs = setupPage.VerifyConfig(HttpContext.Current);
            setupPage.ConfigValidationMessage[] errorMessages = setupPage.ConfigValidationMessage.getAllInvalidMessages(msgs);
            if (errorMessages.Length == 0)
            {
                html.Append("<p style=\"color: green;\">Configuration has been validated without errors</p>");
            }
            else
            {

                html.Append("<div style=\"color: red;\">Error validating configuration: </div>");
                html.Append("<ul>");

                foreach (setupPage.ConfigValidationMessage m in errorMessages)
                {
                    html.Append("<li>" + m.message + "</li>");
                }
                html.Append("</ul>");

            }
            return html.ToString();
        }



    }
}
