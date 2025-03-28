﻿using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IClubRepository
    {
        Task<Club> GetClubByIdWithMembersAsync(int clubId);
        Task AddClubAsync(Club club);
        Task<IEnumerable<Club>> GetAllClubAsync();
    }
}
