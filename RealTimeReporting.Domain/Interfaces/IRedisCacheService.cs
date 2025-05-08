using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeReporting.Domain.Interfaces
{
    public interface IRedisCacheService
    {
        Task<string?> GetValueAsync(string key);
        Task SetValueAsync(string key, string value, TimeSpan? expiry = null);
    }
}
