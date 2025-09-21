public interface IMapper
{
    string SourceType { get; }
    string TargetType { get; }
    object Map(object data);
}
