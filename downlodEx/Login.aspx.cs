using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace downlodEx
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            string a = txtUsername.Text;
            string b = txtPassword.Text;

            // Store values in session
            Session["a"] = a;
            Session["b"] = b;

            if (a == "FindocEX" && b == "Findoc123")
            {
                Response.Redirect("FOEX.aspx", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript",
                "alert('Please enter a valid username and password.');", true);
            }
        }
    }
}