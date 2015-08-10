
using Maui.Data.Recognition.Spec;
namespace Maui.Data.Recognition
{
    /// <summary>
    /// Represents a single fetched result.
    /// </summary>
    public class SingleResultValue<T>
    {
        public SingleResultValue( Site site, T value )
        {
            Site = site;
            Value = value;
        }

        public T Value { get; private set; }
        public Site Site { get; private set; }
    }
}
