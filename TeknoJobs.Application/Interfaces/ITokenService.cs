using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Application.Interfaces
{
    /// <summary>
    /// Defines a service for generating authentication tokens.
    /// </summary>
    /// <remarks>This interface provides a method for creating tokens based on user-specific information.
    /// Implementations of this interface are responsible for encoding the provided data into a token that can be used
    /// for authentication or authorization purposes.</remarks>
    public interface ITokenService
    {
        string GenerateToken(string userId, string username, string role);
    }
}
