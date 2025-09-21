# Dynamic Mapping System

## Overview

The Dynamic Mapping System is designed to map data between the internal DIRS21 C# data models (e.g. Reservation, Room) and external partner-specific models (e.g. Google.Reservation).

It supports bidirectional transformations:

- Internal -> Partner
- Partner -> Internal

The system is extensible, allowing new mapping rules and partner models to be added.

## Using Mappers

When using or [adding](#adding-new-model) new mappers, please don't forget to register the mapper you want to use.  
This ensures that the system knows which mapper to use for each source and target type.

## System Architecture
High-Level Components

- **DynamicMappingSystem.Core**

    Contains the IMapHandler interface and its implementation of MapHandler. This is the system entry point for all mappings.

- **DynamicMappingSystem.Models**

    Defines the internal data models (e.g. Reservation). These represent the DIRS21 domain objects.

- **DynamicMappingSystem.Mappers**
    
    Contains mapper implementations for specific transformations (e.g. ReservationToGoogleMapper, GoogleToReservationMapper). Each mapper implements the IMapper interface.

- **DynamicMappingSystem.Tests**

    Unit tests using xUnit and FluentAssertions, ensuring mapping correctness and system stability.

## Adding New Model

When you need to introduce a new internal model (e.g. `Room`), you do not need to rebuild the mapping system.
Follow these steps to integrate the new model:

1. **Define the Model**
   - Create a new record or class in the `DynamicMappingSystem.Models` project.
   - Keep it simple and aligned with existing models (e.g. `Reservation`).

   Example:
   ```csharp
   namespace DynamicMappingSystem.Models;

   public record Room(Guid id, int RoomNr, int Capacity);

1. **Define the Partner Model(s)**
    - Add the equivalent representation in the appropriate partner namespace/project.
    - Match the structure required by the partner system (e.g. Google).

    Example:
    ```csharp
    namespace DynamicMappingSystem.Partners.Google;

    public record Room(string Id, string Name);

1. **Implement Mapping**
    - Reuse the existing IMapper pattern.
    - Do not change or duplicate core logic in MapHandler!
    - Implement mapping between model (e.g Model.Room) and the partner model (e.g. Google.Room). 

1. **Register the Mapping**
    - Register your new mappers

    Example:
    ```csharp
    var mapHandler = new MapHandler();

    mapHandler.Register(new RoomToGoogleRoomMapper());
    mapHandler.Register(new GoogleRoomToRoomMapper());

1. **Add Unit Tests**
    - Create tests in DynamicMappingSystem.Tests to ensure your mappings work correctly.


## Error Handling & Validation
- No registered mappers -> throw a descriptive exception (InvalidOperationException).
- Null or incompatible input -> throw ArgumentException.