using Maui;

namespace Maui.Data.Recognition
{
    public interface IDatumProvider
    {
        string Datum { get; }
        SingleResultValue<T> FetchSingle<T>();
        IResultPolicy Fetch();
    }
}
