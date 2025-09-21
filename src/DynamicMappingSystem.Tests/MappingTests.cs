namespace DynamicMappingSystem.Tests;
using Xunit;
using FluentAssertions;
using DynamicMappingSystem.Core;
using DynamicMappingSystem.Models;
using DynamicMappingSystem.Mappers;

public class MappingTests
{
    [Fact]
    public void Should_Map_Reservation_To_Google()
    {
        // Arrange
        var mapHandler = new MapHandler();
        mapHandler.Register(new ReservationToGoogleMapper());

        var reservationCode = Guid.NewGuid().ToString();
        var customer = Guid.NewGuid().ToString();
        var checkIn = new DateTime(2025, 9, 21);
        var checkOut = checkIn.AddDays(5);

        var res = new Reservation(reservationCode, customer, checkIn, checkOut);

        // Act
        var googleRes = mapHandler.Map(res, "Model.Reservation", "Google.Reservation") as GoogleReservation;

        // Assert
        googleRes.Should().NotBeNull();
        googleRes.ReservationCode.Should().Be(reservationCode);
        googleRes.Customer.Should().Be(customer);
        googleRes.Start.Should().Be(checkIn.ToString());
        googleRes.End.Should().Be(checkOut.ToString());
    }

    [Fact]
    public void Should_Map_Google_To_Reservation()
    {
        // Arrange
        var mapHandler = new MapHandler();
        mapHandler.Register(new GoogleToReservationMapper());

        var id = Guid.NewGuid().ToString();
        var guestName = Guid.NewGuid().ToString();
        var start = new DateTime(2025, 9, 21).ToString();
        var end = new DateTime(2025, 9, 30).ToString();

        var googleRes = new GoogleReservation(id, guestName, start, end);

        // Act
        var res = mapHandler.Map(googleRes, "Google.Reservation", "Model.Reservation") as Reservation;

        // Assert
        res.Should().NotBeNull();
        res.Id.Should().Be(id);
        res.GuestName.Should().Be(guestName);
        res.CheckIn.Should().Be(DateTime.Parse(start));
        res.CheckOut.Should().Be(DateTime.Parse(end));
    }
}