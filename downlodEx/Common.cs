using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace downlodEx
{
    public class Common
    {
        public string Uid { get; set; }
        public string Pass { get; set; }

        public Common()
        {
            if (HttpContext.Current.Session["a"] != null & HttpContext.Current.Session["b"] != null)
            {
                Uid = HttpContext.Current.Session["a"].ToString();
                Pass = HttpContext.Current.Session["b"].ToString();
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
        }
    }
}