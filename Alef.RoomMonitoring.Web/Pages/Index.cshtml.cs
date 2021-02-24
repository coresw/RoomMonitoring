using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Alef.RoomMonitoring.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public readonly IReservationRepository ReservRepo;
        public readonly IReservationStatusRepository ReservStatusRepo;
        public readonly IAttendeeRepository AttendRepo;
        public readonly IAttendeeTypeRepository AttendTypeRepo;
        public readonly IPersonRepository PersRepo;
        public readonly IRoomRepository RoomRepo;

        public IndexModel(ILogger<IndexModel> logger, IReservationRepository reservRepo, IReservationStatusRepository reservStatusRepo, IAttendeeRepository attendRepo, IAttendeeTypeRepository attendTypeRepo, IPersonRepository persRepo, IRoomRepository roomRepo)
        {
            _logger = logger;
            ReservRepo = reservRepo;
            ReservStatusRepo = reservStatusRepo;
            AttendRepo = attendRepo;
            AttendTypeRepo = attendTypeRepo;
            PersRepo = persRepo;
            RoomRepo = roomRepo;
        }

        public void OnGet()
        {
            
        }
    }
}
