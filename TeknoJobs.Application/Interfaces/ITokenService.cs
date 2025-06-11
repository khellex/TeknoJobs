using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string username, string role);
    }
}
