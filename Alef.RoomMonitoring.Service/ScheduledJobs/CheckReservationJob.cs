﻿using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.ScheduledJobs
{
    /// <summary>
    /// Implementuje job, ktera zajistuje kontrolu realne obsazenosti proti planovane
    /// </summary>
    public class CheckReservationJob : ICheckReservationJob
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICheckReservationService _reservService;


        public CheckReservationJob(ICheckReservationService reservService)
        {
            _reservService = reservService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info($"Do CheckReservationJob at time ={DateTimeOffset.Now}");

                await _reservService.CheckReservation();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
