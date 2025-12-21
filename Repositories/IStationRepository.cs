using System.Collections.Generic;

namespace DatabazeProjekt.Repositories
{
    public interface IStationRepository
    {
        void AddStation(StationRecord station);
        void DeleteStationByName(string name);
        StationRecord GetStationByName(string name);
        IEnumerable<StationRecord> GetAllStations();
    }
}

