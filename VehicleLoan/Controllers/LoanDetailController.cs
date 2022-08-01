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
    public class LoanDetailController : ControllerBase
    {
        VehicleLoanContext vc = new VehicleLoanContext();

        [HttpGet]
        [Route("ListLD")]
        public IActionResult GetLD()
        {
            var data = from LoanDetail in vc.LoanDetails select new { LoanId=LoanDetail.LoanId, LoanAmount=LoanDetail.LoanAmount,LoanTenure=LoanDetail.LoanTenure, LoanInterestRate=LoanDetail.LoanInterestRate, StatusId=LoanDetail.StatusId, CustomerId=LoanDetail.CustomerId  };
            return Ok(data);
        }

        [HttpGet]
        [Route("ListLD/{id}")]
        public IActionResult GetLD(int? id)
        {
            if (id == null)
            {
                return BadRequest("Loan ID cannot be null");
            }
            var data = (from LoanDetail in vc.LoanDetails where LoanDetail.LoanId==id select new { LoanId = LoanDetail.LoanId, LoanAmount = LoanDetail.LoanAmount, LoanTenure = LoanDetail.LoanTenure, LoanInterestRate = LoanDetail.LoanInterestRate, StatusId = LoanDetail.StatusId, CustomerId = LoanDetail.CustomerId }).FirstOrDefault();

            if (data == null)
            {
                return NotFound($"Loan {id} not found/available");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("AddLD")]
        public IActionResult PostLD(LoanDetail loan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*db.Depts.Add(dept);
                      db.SaveChanges();*/
                    vc.Database.ExecuteSqlInterpolated($"AddLD {loan.LoanAmount}, {loan.LoanTenure},{loan.LoanInterestRate},{loan.CustomerId}");
                }

                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return Created("Record Successfully Added!", loan); //doubt????
        }

 
    }
}
