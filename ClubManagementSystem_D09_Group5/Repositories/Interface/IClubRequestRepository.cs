using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;

namespace Repositories.Interface
{
    public interface IClubRequestRepository
    {
        Task<ClubRequest?> AddClubRequest(ClubRequest clubRequest);
    }
}
