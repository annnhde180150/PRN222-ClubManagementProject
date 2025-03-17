using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation
{
    public class ClubRequestService : IClubRequestService
    {
        private readonly IClubRequestRepository _clubRequestRepository;
        public ClubRequestService(IClubRequestRepository clubRequestRepository)
        {
            _clubRequestRepository = clubRequestRepository;
        }
        public async Task<ClubRequest?> AddClubRequest(ClubRequest clubRequest)
        {
            return await _clubRequestRepository.AddClubRequest(clubRequest);
        }
    }
}
