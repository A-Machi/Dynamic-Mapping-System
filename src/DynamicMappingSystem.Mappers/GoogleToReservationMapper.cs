using DynamicMappingSystem.Models;

namespace DynamicMappingSystem.Mappers;

public class GoogleToReservationMapper : IMapper
{
    public string SourceType => "Google.Reservation";
    public string TargetType => "Model.Reservation";

    public object Map(object data)
    {
        var res = data as GoogleReservation ?? throw new ArgumentException("Invalid data type");
        return new Reservation
        (
            res.ReservationCode,
            res.Customer,
            DateTime.Parse(res.Start),
            DateTime.Parse(res.End)
        );
    }
}
