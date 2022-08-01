using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleLoan.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleLoan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDetailController : ControllerBase
    {
        VehicleLoanContext vc = new VehicleLoanContext();

        [HttpGet]
        [Route("ListVehicles")]
        public IActionResult GetVehicle()
        {
            var data = from VehicleDetail in vc.VehicleDetails select new { VehicleId = VehicleDetail.VehicleId, Carmake = VehicleDetail.CarMake,CarModel=VehicleDetail.CarModel, ExshowroomPrice=VehicleDetail.ExshowroomPrice, OnroadPrice=VehicleDetail.OnroadPrice, CustomerId=VehicleDetail.CustomerId };
            return Ok(data);
        }

        [HttpGet]
        [Route("ListVehicles/{id}")]
        public IActionResult GetVehicle(int? id)
        {
            if (id == null)
            {
                return BadRequest("Vehicle ID cannot be null");
            }
            var data = (from VehicleDetail in vc.VehicleDetails where VehicleDetail.VehicleId==id select new { VehicleId = VehicleDetail.VehicleId, Carmake = VehicleDetail.CarMake, CarModel = VehicleDetail.CarModel, ExshowroomPrice = VehicleDetail.ExshowroomPrice, OnroadPrice = VehicleDetail.OnroadPrice, CustomerId = VehicleDetail.CustomerId }).FirstOrDefault();
            
            if (data == null)
            {
                return NotFound($"Vehicle {id} not found/available");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("AddVehicle")]
        public IActionResult PostVehicle(VehicleDetail vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*db.Depts.Add(dept);
                      db.SaveChanges();*/
                    vc.Database.ExecuteSqlInterpolated($"AddVehicle  {vehicle.CarMake}, {vehicle.CarModel},{vehicle.ExshowroomPrice},{vehicle.OnroadPrice},{vehicle.CustomerId}");
                }

                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return Created("Record Successfully Added!", vehicle); //doubt????
        }

        [HttpPut]
        [Route("EditVehicle/{id}")]
        public IActionResult PutVehicle(int id, VehicleDetail vehicle)
        {
            if (ModelState.IsValid)
            {
                VehicleDetail veh = vc.VehicleDetails.Find(id); // find record
                veh.CarMake = vehicle.CarMake;
                veh.CarModel = vehicle.CarModel;
                veh.ExshowroomPrice = vehicle.ExshowroomPrice;
                veh.OnroadPrice = vehicle.OnroadPrice;
                vc.SaveChanges();
                return Ok(); //removed the message here to navigate back to list
            }
            return BadRequest("Unable to edit the record");
        }

        [HttpDelete]
        [Route("DeleteVehicles/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var data = vc.VehicleDetails.Find(id);
            vc.VehicleDetails.Remove(data);
            return Ok("Record Deleted");
        }

    }
}
