using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxViewModelFactory : IViewModelFactory<MoneyBoxViewModel, MoneyBoxView, MoneyBoxModel>
    {
        private readonly IStaticDataService _staticDataService;

        public MoneyBoxViewModelFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MoneyBoxViewModel Create(MoneyBoxModel model, MoneyBoxView view)
        {
            return new MoneyBoxViewModel(model, view, _staticDataService);
        }
    }
}