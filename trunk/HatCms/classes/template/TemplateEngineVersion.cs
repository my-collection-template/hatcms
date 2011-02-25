using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace HatCMS
{
    /// <summary>
    /// This enumeration tracks the current template engine version that is in use.
    /// </summary>
    public enum CmsTemplateEngineVersion { v1 = 1, v2 = 2 };
}
