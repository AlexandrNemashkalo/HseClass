namespace HseClass.Core.Infrastructure
{
    public interface IMapper<in TSource, out TResult>
    {
        TResult Map(TSource source);
    }
}