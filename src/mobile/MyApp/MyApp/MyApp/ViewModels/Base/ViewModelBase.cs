using System.Threading.Tasks;

namespace MyApp.ViewModels.Base
{
    public class ViewModelBase
    {
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
