using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IAccountService _accountService;
        private readonly IImageHelperService _imageHelperService;

        public ClubService(IClubRepository clubRepository, IAccountService accountService, IImageHelperService imageHelperService)
        {
            _clubRepository = clubRepository;
            _accountService = accountService;
            _imageHelperService = imageHelperService;
        }

        public async Task<ClubDetailsViewDto> GetClubDetailsAsync(int clubId)
        {
            var club = await _clubRepository.GetClubByIdWithMembersAsync(clubId);

            if (club == null) return null;

            var clubMemberDtos = club.ClubMembers
                .Select(member => new ClubMemberDto
                {
                    UserId = member.UserId,
                    Username = member.User.Username,
                    ProfilePictureBase64 = _imageHelperService.ConvertToBase64(member.User.ProfilePicture, "png")
                })
                .ToList();

            var viewModel = new ClubDetailsViewDto
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                Logo = club.Logo,
                Cover = club.Cover,
                Description = club.Description,
                ClubMembers = clubMemberDtos
            };

            return viewModel;
        }
        public async Task AddClubAsync(Club club)
        {
            await _clubRepository.AddClubAsync(club);
        }
    }

}
