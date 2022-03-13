using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Helpers
{
    public static class Timestamp
    {
        public static string Get(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }
    }
}
