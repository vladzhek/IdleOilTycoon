using Gameplay.PromoCode;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Investing.UI
{
    public class InvestingWindow : Window
    {
        [SerializeField] private InvestingViewInitializer _investingViewInitializer;
        private IInvestingModel _investingModel;

        [Inject]
        private void Construct(IInvestingModel investingModel)
        {
            _investingModel = investingModel;
        }

        private void OnEnable()
        {
            _investingViewInitializer.Initialize(_investingModel);
        }
    }
}