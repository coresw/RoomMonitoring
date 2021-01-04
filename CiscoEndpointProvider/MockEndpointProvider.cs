using System;
using System.Collections.Generic;
using System.Text;

namespace CiscoEndpointProvider
{
    public class MockEndpointProvider : IEndpointProvider
    {
        public int GetPeopleCount(string endpointIP)
        {
            try
            {
                int peopleCount = 1;

                //TODO - hodnoptu lze nacitat z DB, kde lze lepe nastavovat pro ucely testovani

                return (peopleCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
