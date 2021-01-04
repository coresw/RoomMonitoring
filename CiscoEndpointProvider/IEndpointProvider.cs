using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CiscoEndpointProvider
{
    public interface IEndpointProvider
    {
        int GetPeopleCount(string endpointIP);
    }
}
