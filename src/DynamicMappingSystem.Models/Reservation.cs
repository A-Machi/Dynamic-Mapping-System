namespace DynamicMappingSystem.Models;

public record Reservation
(
    string Id,
    string GuestName,
    DateTime CheckIn,
    DateTime CheckOut
);

