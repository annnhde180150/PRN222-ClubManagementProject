using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IClubMemberRepository
    {
        Task<bool> IsUserInClubAsync(int userId, int clubId);
    }
}
