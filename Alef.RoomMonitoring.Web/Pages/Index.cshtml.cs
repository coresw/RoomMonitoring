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
        public readonly IAttendeeRepository AttendRepo;
        public readonly IPersonRepository PersRepo;
        public readonly IRoomRepository RoomRepo;

        public IndexModel(ILogger<IndexModel> logger, IReservationRepository reservRepo, IAttendeeRepository attendRepo, IPersonRepository persRepo, IRoomRepository roomRepo)
        {
            _logger = logger;
            ReservRepo = reservRepo;
            AttendRepo = attendRepo;
            PersRepo = persRepo;
            RoomRepo = roomRepo;
        }

        public void OnGet()
        {
            
        }
    }
}
