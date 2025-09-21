namespace DynamicMappingSystem.Core;

public class MapHandler
{
    private readonly List<IMapper> _mappers = new();

    public void Register(IMapper mapper)
    {
        _mappers.Add(mapper);
    }

    public object Map(object data, string sourceType, string targetType)
    {
        var mapper = _mappers.FirstOrDefault(m =>
            m.SourceType == sourceType && m.TargetType == targetType);

        if (mapper == null)
        {
            throw new InvalidOperationException(
                $"No mapper registered for {sourceType} -> {targetType}");
        }

        return mapper.Map(data);
    }
}

