using DynamicMappingSystem.Models;

namespace DynamicMappingSystem.Mappers;

public class ReservationToGoogleMapper : IMapper
{
    public string SourceType => "Model.Reservation";
    public string TargetType => "Google.Reservation";

    public object Map(object data)
    {
        var res = data as Reservation ?? throw new ArgumentException("Invalid data type");
        return new GoogleReservation
        (
            res.Id,
            res.GuestName,
            res.CheckIn.ToString(),
            res.CheckOut.ToString()
        );
    }
}

