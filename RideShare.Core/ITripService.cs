using System.Collections.Generic;
using RideShare.Domain;

namespace RideShare.Core
{
    public interface ITripService
    {
        bool Create(Trip trip);
        bool ChangeStatus(Trip trip);
        bool Join(Trip trip,User user);
        Trip FindById(string id);
        List<Trip> FindByRoute(string origin, string destination, decimal? locationX, decimal? locationY);
    }
}