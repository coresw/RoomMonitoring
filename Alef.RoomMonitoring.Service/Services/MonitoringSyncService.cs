using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services
{
    public class MonitoringSyncService : IMonitoringSyncService
    {
        private readonly IMSGraphProvider _graphProvider;
        private readonly IPersonRepository _personRepo;
        private readonly IReservationRepository _reservRepo;
        private readonly IAttendeeRepository _attendRepo;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        #region Ctor

        public MonitoringSyncService(IMSGraphProvider graphProvider, IPersonRepository personRepo, IReservationRepository reservRepo, IAttendeeRepository attendRepo)
        {
            _graphProvider = graphProvider;
            _personRepo = personRepo;
            _reservRepo = reservRepo;
            _attendRepo = attendRepo;
        }

        #endregion

        #region Public methods

        public async Task GetNewReservation()
        {
            try
            {
                _logger.Info("GetNewReservation started...");

                var reservations = _graphProvider.GetReservations();

                //Pro vsechny mistnosti (dane v DB ciselniku)
                //nacti z O365 pres MS Graph vsechny aktualni schuzky pro dany den. Zajimaji nas pouze ty, ktere jsou od ted do konce dne
                //je treba uvazovat, ze schuzky se prubezne rusi, posunuji a zakladaji
                //schuzky maji sva unikatni ID v O365
                //na zaklade unikatniho ID najit schuzku v DB a provest jeji update ci zalozeni
                //naopak schuzky, ktere jsou v DB a v kolekci  s O365 jiz nejsou, je treba zrusit
                //vysledkem je, ze pri kazdem behu teto metody bude v DB obraz toho co je v O365

                _logger.Info("GetNewReservation finished...");
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error");
                throw;
            }
        }

        #endregion

    }
}
