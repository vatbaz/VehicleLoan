using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleLoan.Models;

namespace VehicleLoan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentDetailController : ControllerBase
    {
        VehicleLoanContext vc = new VehicleLoanContext();

        [HttpGet]
        [Route("ListEmp")]
        public IActionResult GetEmp()
        {
            var data = from EmploymentDetail in vc.EmploymentDetails select new { EmpId = EmploymentDetail.EmpId, TypeOfEmployement = EmploymentDetail.TypeOfEmployement, YearlySalary = EmploymentDetail.YearlySalary, ExistingEmi = EmploymentDetail.ExistingEmi, CustomerId = EmploymentDetail.CustomerId };
            return Ok(data);
        }

        [HttpGet]
        [Route("ListEmp/{id}")]
        public IActionResult GetEmp(int? id)
        {
            if (id == null)
            {
                return BadRequest("Emp ID cannot be null");
            }
            var data = (from EmploymentDetail in vc.EmploymentDetails where EmploymentDetail.EmpId==id select new { EmpId = EmploymentDetail.EmpId, TypeOfEmployement = EmploymentDetail.TypeOfEmployement, YearlySalary = EmploymentDetail.YearlySalary, ExistingEmi = EmploymentDetail.ExistingEmi, CustomerId = EmploymentDetail.CustomerId }).FirstOrDefault();

            if (data == null)
            {
                return NotFound($"Emp {id} not found/available");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("AddEmp")]
        public IActionResult PostVehicle(EmploymentDetail emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*db.Depts.Add(dept);
                      db.SaveChanges();*/
                    vc.Database.ExecuteSqlInterpolated($"AddEmp  {emp.TypeOfEmployement}, {emp.YearlySalary},{emp.ExistingEmi},{emp.CustomerId}");
                }

                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return Created("Record Successfully Added!", emp); //doubt????
        }

        [HttpPut]
        [Route("EditEmp/{id}")]
        public IActionResult PutVehicle(int id, EmploymentDetail emp)
        {
            if (ModelState.IsValid)
            {
                EmploymentDetail e = vc.EmploymentDetails.Find(id); // find record
                e.TypeOfEmployement = emp.TypeOfEmployement;
                e.YearlySalary= emp.YearlySalary;
                e.ExistingEmi = emp.ExistingEmi;
                
                vc.SaveChanges();
                return Ok(); //removed the message here to navigate back to list
            }
            return BadRequest("Unable to edit the record");
        }

        [HttpDelete]
        [Route("DeleteEmp/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var data = vc.EmploymentDetails.Find(id);
            vc.EmploymentDetails.Remove(data);
            return Ok("Record Deleted");
        }
    }
}
