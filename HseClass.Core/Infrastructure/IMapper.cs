using System.Diagnostics;

namespace HseClass.Core.Infrastructure
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource source);
    }
}