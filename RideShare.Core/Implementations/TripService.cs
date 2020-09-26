using System;
using System.Collections.Generic;
using System.Linq;
using RideShare.Data.Repositories;
using RideShare.Domain;

namespace RideShare.Core.Implementations
{
    public class TripService : ITripService
    {
        private TripRepository TripRepository { get; set; }

        public TripService(TripRepository tripRepository)
        {
            TripRepository = tripRepository;
        }

        /// <summary>
        /// Trip create service with trip model
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public bool Create(Trip trip)
        {
            try
            {
                TripRepository.Create(trip);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// ChangeStatus changes status of trip 
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public bool ChangeStatus(Trip trip)
        {
            var currentTrip = TripRepository.Find(trip.Id.ToString());
            if (currentTrip != null)
            {
                currentTrip.IsActive = trip.IsActive;
                return TripRepository.Update(currentTrip.Id.ToString(), currentTrip);
            }

            return false;
        }

        /// <summary>
        /// Join is adding riders to specified trip with models
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Join(Trip trip, User user)
        {
            if (trip != null)
            {
                if (trip.Riders == null)
                {
                    trip.Riders = new List<User>();
                }

                trip.Riders.Add(user);
                return TripRepository.Update(trip.Id.ToString(), trip);
            }

            return false;
        }

        /// <summary>
        /// FindById is basically finding trip with specified trip id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Trip FindById(string id)
        {
            var foundTrip = TripRepository.Find(id);
            return foundTrip;
        }

        /// <summary>
        /// FindByRoute is finding trips by routes or location data(s)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="locationX"></param>
        /// <param name="locationY"></param>
        /// <returns></returns>
        public List<Trip> FindByRoute(string origin, string destination, decimal? locationX, decimal? locationY)
        {
            var allTrips = TripRepository.FindAll();
            if (allTrips.Any())
            {
                allTrips = allTrips.Where(i => (i.Origin == origin && i.Destination == destination)
                                               && (i.OriginLocationX >= locationX &&
                                                   i.DestinationLocationX >= locationX) &&
                                               i.OriginLocationY <= locationY && i.DestinationLocationY >= locationY)
                    .ToList();
                allTrips.ForEach(i => i.Driver.Password = "");
                return allTrips;
            }

            return new List<Trip>();
        }
    }
}