using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models;
using System.Web.Mvc;
using System.Web;
using DataAccessLayer;
using System.Web.Http.Filters;
namespace Controllers
{
    
    public class Error
    {
        public List<string> Errors { get; set; }
    }
    public class ClientData
    {
        public bool IsValid { get; set; }
        public object Data { get; set; }
    }
    [AllowCrossSite]
    public class CustomerController : ApiController
    {
        // Insert
        public Object Post(Customer obj)
        {
            ClientData Data = new ClientData();

            if (ModelState.IsValid)
            {
                // insert the customer object to database
                // EF DAL
                Dal Dal = new Dal();
                Dal.Customers.Add(obj); // in mmemory
                Dal.SaveChanges(); // physical commit
            }
            else
            {
                var Err = new Error();
                Err.Errors = new List<string>();
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Err.Errors.Add(error.ErrorMessage);
                    }
                }
                Data.IsValid = false;
                Data.Data = Err;
                return Data;
                
            }
            Dal dal = new Dal();
            List<Customer> customerscoll = dal.Customers.ToList<Customer>();
            Data.IsValid = true;
            Data.Data = customerscoll;
            return Data;

        }
        
        // Select
        public List<Customer> Get() // All the record
        {
            // Read the query string
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();

            string CustomerCode = allUrlKeyValues.
                            SingleOrDefault(x => x.Key == "CustomerCode").Value;
            string CustomerName = allUrlKeyValues.
                            SingleOrDefault(x => x.Key == "CustomerName").Value;

            Dal dal = new Dal();
            List<Customer> customerscoll = new List<Customer>();
            if(CustomerName != null)
            {
                customerscoll = (from t in dal.Customers
                                 where t.CustomerName == CustomerName
                                 select t).ToList<Customer>();
            }
            else if (CustomerCode != null)
            {
                customerscoll = (from t in dal.Customers
                                 where t.CustomerCode == CustomerCode
                                 select t).ToList<Customer>();
            }
            else
            {
                customerscoll = dal.Customers.ToList<Customer>();
            }
             
            return customerscoll;
        }
      

        // Update PUT
        public List<Customer> Put(Customer obj)
        {
            // Select the record ( LINQ )
            Dal dal = new Dal();
            Customer custupdate = (from temp in dal.Customers
                                   where temp.CustomerCode == obj.CustomerCode
                                   select temp).ToList<Customer>()[0];
            custupdate.CustomerAmount = obj.CustomerAmount;
            custupdate.CustomerName = obj.CustomerName;
            List<Customer> customerscoll = dal.Customers.ToList<Customer>();

            return customerscoll;

            // Update the record
           
        }
        // Delete Delete
        public List<Customer> Delete(Customer obj)
        {
            // Delete
            Dal Dal = new Dal();
            Customer custdelete = (from temp in Dal.Customers
                                   where temp.CustomerCode == obj.CustomerCode
                                   select temp).ToList<Customer>()[0];
            Dal.Customers.Remove(custdelete);
            Dal.SaveChanges();
            List<Customer> customerscoll = Dal.Customers.ToList<Customer>();

            return customerscoll;
        }
    }
    
    public class AllowCrossSite : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }
    }

}
