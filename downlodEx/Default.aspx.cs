using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace downlodEx
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlData.SelectedValue == "1")
            {
                Response.Redirect("uploadex.aspx");
            }

            else if (ddlData.SelectedValue == "2")
            {
                Response.Redirect("MCXdata_EX.aspx");
            }
            else
            {

            }
        }
    }
}


                
        

