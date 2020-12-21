using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services
{
    /// <summary>
    /// Trida implementujici funkcnost kontroly 
    /// </summary>
    public class CheckReservationService : ICheckReservationService
    {
        private readonly IReservationRepository _reservRepo;

        #region Ctor

        public CheckReservationService(IReservationRepository reservRepo)
        {
            _reservRepo = reservRepo;
        }

        #endregion

        #region Public methods

        public async Task CheckReservation()
        {
            try
            {
                //Nacist rezervace z DB. Nacitaji se pouze rezervace, ktere nebyly jiz zpracovany tj. notifikovany atd.
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
