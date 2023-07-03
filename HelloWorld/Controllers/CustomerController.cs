using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DataAccessLayer;
using System.Threading;
namespace Controllers
{

    [Authorize]
    public class CustomerUIController : Controller
    {   

        public ActionResult EnterCustomer()
        {
         
            return View();
        }
        public ActionResult SearchCustomer()
        {
            return View();
        }
     

    }
}