using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Investing;
using Gameplay.Quests;
using Infrastructure.PersistenceProgress;
using Infrastructure.Purchasing;
using Infrastructure.StaticData;

namespace Gameplay.Offer
{
    public class OfferModel : IOfferModel
    {
        public event Action OnBuyOffer;
        
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IGenericPurchasingService _purchasingService;
        private readonly CurrenciesModel _currenciesModel;
        
        public OfferModel(IProgressService progressService,
            IGenericPurchasingService purchasingService, 
            IStaticDataService staticDataService,
            CurrenciesModel currenciesModel)
        {
            _progressService = progressService;
            _purchasingService = purchasingService;
            _staticDataService = staticDataService;
            _currenciesModel = currenciesModel;
        }

        public void Initialize()
        {
            //GetPurchases();
        }

        private async void GetPurchases()
        {
            var purchases = await _purchasingService.GetPurchases();
            foreach (var purchase in purchases)
            {
                foreach (var offer in _staticDataService.OfferData.Offers)
                {
                    if (offer.purchaseData.ID == purchase.Data.ID)
                    {
                        offer.SetProduct(purchase.Product);
                    }
                }
            }
        }

        public async void BuyOffer(string purchaseID)
        {
            /*if (!await _purchasingService.Purchase(purchaseID))
            {
                return;
            }*/

            switch (GetProgressData().StatusType)
            {
                case OfferType.StartOffer:
                    _progressService.PlayerProgress.OfferProgresses.StatusType = OfferType.ProfessionalOffer;
                    TakeResources(GetConfig(OfferType.StartOffer));
                    break;
                case OfferType.ProfessionalOffer:
                    _progressService.PlayerProgress.OfferProgresses.StatusType = OfferType.RichOffer;
                    TakeResources(GetConfig(OfferType.ProfessionalOffer));
                    break;
                case OfferType.RichOffer:
                    TakeResources(GetConfig(OfferType.RichOffer));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnBuyOffer?.Invoke();
        }

        public OfferProgress GetProgressData()
        {
            return _progressService.PlayerProgress.OfferProgresses;
        }

        private void TakeResources(OfferData data)
        {
            foreach (var currency in data.purchaseData.Currencies)
            {
                _currenciesModel.Add(currency.Type, currency.Amount);
            }
        }

        private OfferData GetConfig(OfferType type)
        {
            return _staticDataService.OfferData.Offers.Find(x => x.Type == type);
        }
    }
}