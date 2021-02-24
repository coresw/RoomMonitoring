using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration.Model
{
    public class ReservationSettings
    {

        /// <summary>
        /// Time between reservation start and room status check
        /// </summary>
        public int CheckTimeout;

        /// <summary>
        /// Subject of the notification email sent when a reserved room is not occupied
        /// </summary>
        public string NotificationTitle;

        /// <summary>
        /// Body of the notification email sent when a reserved room is not occupied
        /// 
        /// Can be HTML
        /// 
        /// Following sequences will be replaced:
        /// %ReservName% - reservation name
        /// %ReservBody% - reservation body
        /// %ReservFrom% - reservation start time
        /// %ReservTo% - reservation end time
        /// %Organizer% - organizer of the meeting
        /// %Room% - name of the room at which the meeting takes place
        /// </summary>
        public string NotificationBody;

    }
}
