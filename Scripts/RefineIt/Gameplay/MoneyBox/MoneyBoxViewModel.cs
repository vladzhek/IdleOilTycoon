using System.Threading.Tasks;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxViewModel : ViewModelBase<MoneyBoxModel, MoneyBoxView>
    {
        private readonly IStaticDataService _staticDataService;
        
        public MoneyBoxViewModel(MoneyBoxModel model, MoneyBoxView view, IStaticDataService staticDataService) : base(model, view)
        {
            _staticDataService = staticDataService;
        }

        public override Task Show()
        {
            UpdateView();
           
           return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.OnChange += UpdateView;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            
            Model.OnChange -= UpdateView;
        }

        private void UpdateView()
        {
            View.SetLevel(Model.ProgressData.Level + 1);
            View.SetQuantity(Model.ProgressData.Amount);
            View.SetData(Model.ProgressData.IsBuy, Model.IsMaxLevel);
            View.SetCapacity(Model.Config.Capacities, Model.NextLevelConfig.Capacities);
            View.SetSlider(Model.ProgressData.Amount, Model.Config.Capacities);
            View.SetButtonsStatus(Model.IsFillMoneyBox, Model.ProgressData.IsBuy);
            View.SetPriceValue("", Model.ProgressData.IsBuy);
        }
    }
}