using TalentoPlus.Domain.Entities;
using TalentoPlus.Application.DTOs;
using Microsoft.AspNetCore.Identity; 

using System.Collections.Generic;
using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces
{
    public interface IJwtService
    {
        LoginResponseDto GenerateToken(string userId, string userName, IList<string> roles);
    }
}
