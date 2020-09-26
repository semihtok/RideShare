using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShare.API.Models;
using RideShare.Core;
using RideShare.Domain;

namespace RideShare.API.Controllers
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IUserService _userService;

        public TripController(ITripService tripService, IUserService userService)
        {
            _tripService = tripService;
            _userService = userService;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult Search(string origin, string destination,decimal? locationX, decimal? locationY)
        {
            var foundTrips = _tripService.FindByRoute(origin, destination,locationX,locationY);
            if (foundTrips.Any())
            {
                return Ok(new ResponseModel
                {
                    Message = $"{foundTrips.Count} trips found",
                    Data = foundTrips
                });
            }

            return NotFound(new ResponseModel {Message = "Available trips not found"});
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult Create([FromBody] Trip trip)
        {
            var currentUserPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var user = _userService.Find(currentUserPhone);
            trip.Driver = user;
            
            var result = _tripService.Create(trip);

            if (result)
            {
                return Ok(new ResponseModel
                {
                    Message = "Trip created successfully",
                });
            }

            return BadRequest(new ResponseModel
            {
                Message = "Bad request"
            });
        }

        [HttpPut]
        [Route("[action]")]
        public ActionResult ChangeStatus(bool status, string tripId)
        {
            var currentUserPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var foundTrip = _tripService.FindById(tripId);

            if (foundTrip != null && foundTrip.Driver.Phone == currentUserPhone)
            {
                foundTrip.IsActive = status;
                var result = _tripService.ChangeStatus(foundTrip);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        Message = "Trip status changed successfully",
                    });
                }
            }

            return BadRequest(new ResponseModel
            {
                Message = "Bad request"
            });
        }

        [HttpPut]
        [Route("[action]")]
        public ActionResult Join(string tripId)
        {
            var currentUserPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var foundTrip = _tripService.FindById(tripId);

            // Drivers cannot join to their car
            if (foundTrip != null && foundTrip.Driver.Phone != currentUserPhone)
            {
                var user = _userService.Find(currentUserPhone);
                _tripService.Join(foundTrip, user);
                return Ok(new ResponseModel
                {
                    Message = "You joined to trip successfully! Have a nice trip!"
                });
                
            }
            return BadRequest(new ResponseModel
            {
                Message = "Bad request"
            });
        }
    }
}