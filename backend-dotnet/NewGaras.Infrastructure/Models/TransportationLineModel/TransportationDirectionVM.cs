


namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class TransportationDirectionVM
    {
       // public long Id { get; set; }

       //  public long TransportationVehicleRouteId { get; set; }

  
        public string RouteDirection { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }

        //  public DateTime CreationDate { get; set; }

        // public long CreationBy { get; set; }
        public decimal? Latitude { get; set; }

        public decimal? Longtitud { get; set; }

    }

    public class TransportationDirectionDto
    {

        public List<TransportationDirectionVM> Data { get; set; }


    }

}
