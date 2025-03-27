using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IClubService
    {
        Task<ClubDetailsViewDto> GetClubDetailsAsync(int clubId);
        Task AddClubAsync(Club club);
    }
}
