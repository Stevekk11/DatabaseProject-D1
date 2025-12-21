using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly SqlConnection _connection;

        public StationRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void AddStation(StationRecord station)
        {
            string insertQuery = @"INSERT INTO dbo.stanice (nazev, typ_stanice, ma_pristresek, ma_lavicku, ma_kos, ma_infopanel, na_znameni, bezbarierova)
                                    VALUES (@stationname, @stationtype, @hasshelter, @hasbench, @hastrashbin, @hasinfopanel, @requeststop, @barrierfree);";
            using (SqlCommand cmd = new SqlCommand(insertQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@stationname", station.StationName);
                cmd.Parameters.AddWithValue("@stationtype", station.StationType);
                cmd.Parameters.AddWithValue("@hasshelter", station.HasShelter);
                cmd.Parameters.AddWithValue("@hasbench", station.HasBench);
                cmd.Parameters.AddWithValue("@hastrashbin", station.HasTrashBin);
                cmd.Parameters.AddWithValue("@hasinfopanel", station.HasInfoPanel);
                cmd.Parameters.AddWithValue("@requeststop", station.RequestStop);
                cmd.Parameters.AddWithValue("@barrierfree", station.BarrierFree);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStationByName(string name)
        {
            string deleteQuery = "DELETE FROM dbo.stanice WHERE nazev = @name";
            using (SqlCommand cmd = new SqlCommand(deleteQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }

        public StationRecord GetStationByName(string name)
        {
            string selectQuery = "SELECT * FROM dbo.stanice WHERE nazev = @name";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StationRecord
                        {
                            StationName = reader["nazev"]?.ToString() ?? string.Empty,
                            StationType = reader["typ_stanice"]?.ToString() ?? string.Empty,
                            HasShelter = (bool)reader["ma_pristresek"],
                            HasBench = (bool)reader["ma_lavicku"],
                            HasTrashBin = (bool)reader["ma_kos"],
                            HasInfoPanel = (bool)reader["ma_infopanel"],
                            RequestStop = (bool)reader["na_znameni"],
                            BarrierFree = (bool)reader["bezbarierova"],
                        };
                    }
                }
            }
            return null;
        }

        public IEnumerable<StationRecord> GetAllStations()
        {
            var stations = new List<StationRecord>();
            string selectQuery = "SELECT * FROM dbo.stanice";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    stations.Add(new StationRecord
                    {
                        StationName = reader["nazev"]?.ToString() ?? string.Empty,
                        StationType = reader["typ_stanice"]?.ToString() ?? string.Empty,
                        HasShelter = (bool)reader["ma_pristresek"],
                        HasBench = (bool)reader["ma_lavicku"],
                        HasTrashBin = (bool)reader["ma_kos"],
                        HasInfoPanel = (bool)reader["ma_infopanel"],
                        RequestStop = (bool)reader["na_znameni"],
                        BarrierFree = (bool)reader["bezbarierova"],
                    });
                }
            }
            return stations;
        }
    }
}
