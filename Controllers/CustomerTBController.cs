using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjaxDataTable.Data;
using AjaxDataTable.Models;
using Microsoft.AspNetCore.Mvc;

namespace AjaxDataTable.Controllers
{
    public class CustomerTBController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerTBController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.CustomerTBs.ToList());
        }

        public JsonResult GetAllCustomers()
        {
            List<CustomerTB> customerTBs = new List<CustomerTB>();
            customerTBs = _context.CustomerTBs.Select(c => c).ToList();
            return Json(new { data = customerTBs });
            
        }


        public IActionResult ShowTable()
        {
            return View();
        }


        [HttpPost]
        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in _context.CustomerTBs select tempcustomer);
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {  
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                    switch (sortColumn)
                    {
                        case "CustomerName":
                            customerData = sortColumnDirection == "asc"? customerData.OrderBy(c => c.CustomerName) :  customerData.OrderByDescending(c => c.CustomerName);
                            break;
                        case "Address":
                            customerData = sortColumnDirection == "asc" ? customerData.OrderBy(c => c.Address) : customerData.OrderByDescending(c => c.Address);
                            break;
                        case "City":
                            customerData = sortColumnDirection == "asc" ? customerData.OrderBy(c => c.City) : customerData.OrderByDescending(c => c.City);
                            break;
                        case "Country":
                            customerData = sortColumnDirection == "asc" ? customerData.OrderBy(c => c.Country) : customerData.OrderByDescending(c => c.Country);
                            break;
                        case "PhoneNo":
                            customerData = sortColumnDirection == "asc" ? customerData.OrderBy(c => c.PhoneNo) : customerData.OrderByDescending(c => c.PhoneNo);
                            break;
                        default :                        
                            break;
                    }
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.CustomerName.Contains(searchValue) ||
                    m.Address.Contains(searchValue) || m.City.Contains(searchValue) ||
                    m.Country.Contains(searchValue) || m.PhoneNo.Contains(searchValue));
                }
                //total number of rows counts
                recordsTotal = customerData.Count();
                //Paging
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Return Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });

            }
            catch (Exception)
            {

                throw;
            }

            
        }
    }
}
