

using NewGaras.Infrastructure.DTO.HrUser;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class CreateHrUserWithAllRoutesDto
    {
      

        public HrUserDto HrUserDto { get; set; }
        public List<AddRoutesForHrUserVM> Data { get; set; }

    }
}
