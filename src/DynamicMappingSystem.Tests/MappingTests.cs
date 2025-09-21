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

    [Fact]
    public void Should_Throw_InvalidOperationException()
    {
        // Arrange
        var mapHandler = new MapHandler();
        mapHandler.Register(new GoogleToReservationMapper());

        var googleRes = new GoogleReservation(
            Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), 
            new DateTime(2025, 9, 21).ToString(), 
            new DateTime(2025, 9, 30).ToString()
        );

        var unknownReservationType = Guid.NewGuid().ToString();

        // Act
        Action act = () => mapHandler.Map(googleRes, unknownReservationType, "Model.Reservation");

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"No mapper registered for {unknownReservationType} -> Model.Reservation");
    }

    [Fact]
    public void Should_Throw_ArgumentException_In_GoogleToReservationMapper()
    {
        // Arrange
        var mapHandler = new MapHandler();
        mapHandler.Register(new GoogleToReservationMapper());

        var wrongObject = new Reservation(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            new DateTime(2025, 9, 21),
            new DateTime(2025, 9, 30)
        );

        // Act
        Action act = () => mapHandler.Map(wrongObject, "Google.Reservation", "Model.Reservation");

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Invalid data type");
    }

    [Fact]
    public void Should_Throw_ArgumentException_In_ReservationToGoogleMapper()
    {
        // Arrange
        var mapHandler = new MapHandler();
        mapHandler.Register(new ReservationToGoogleMapper());

        var wrongObject = new GoogleReservation(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            new DateTime(2025, 9, 21).ToString(),
            new DateTime(2025, 9, 30).ToString()
        );

        // Act
        Action act = () => mapHandler.Map(wrongObject, "Model.Reservation", "Google.Reservation");

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Invalid data type");
    }

}