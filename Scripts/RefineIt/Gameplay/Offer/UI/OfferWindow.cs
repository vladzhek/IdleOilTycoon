using Gameplay.Investing.UI;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Offer.UI
{
    public class OfferWindow : Window
    {
        [SerializeField] private OfferViewInitializer _initializer;
        private IOfferModel _model;

        [Inject]
        private void Construct(IOfferModel model)
        {
            _model = model;
        }

        private void OnEnable()
        {
            _initializer.Initialize(_model);
        }
    }
}