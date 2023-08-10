using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.BattlePass;
using Gameplay.Currencies;
using Gameplay.DailyEntry;
using Gameplay.Investing;
using Gameplay.MoneyBox;
using Gameplay.Offer;
using Gameplay.PromoCode;
using Gameplay.Quests;
using Gameplay.Region.Data;
using Gameplay.Shop;
using Gameplay.TutorialStateMachine;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public List<RegionProgress> RegionProgresses;
        public List<CurrencyData> Currencies;
        public List<QuestsProgress> DailyQuests;
        public List<QuestsProgress> WeeklyQuests;
        public List<PromoCodeProgress> PromoCodeProgresses;
        public List<ShopCurrencyProductProgress> ShopCurrencyProductProgress;
        public List<ShopPurchaseProductProgress> ShopPurchaseProductProgresses;
        public List<DailyEntryProgress> DailyEntryProgresses;
        public InvestingProgress InvestingProgresses;
        public MoneyBoxProgressData MoneyBoxProgressData;
        public BattlePassProgressData BattlePassProgressData;
        public TutorialProgress TutorialProgress = new();
        public OfferProgress OfferProgresses;
        public string LastSession;
        public string BattlePassFirstDay;
        public string DateCreateWeekQuests;
        public int CountRefreshDaily;
        public int CountRefreshWeekly;
        public float MusicVolume;
        public float SFXVolume;
        public int CurrentDailyEntry;

        public PlayerProgress()
        {
            RegionProgresses = new List<RegionProgress>
            {
                new(RegionType.Desert, false),
                new(RegionType.City, true)
            };
            
            Currencies = new List<CurrencyData>
            {
                new(CurrencyType.SoftCurrency, 250000),
                new(CurrencyType.HardCurrency, 500),
                new(CurrencyType.WorkerCurrency, 300)
            };

            PromoCodeProgresses = new List<PromoCodeProgress>();
            DailyQuests = new List<QuestsProgress>();
            WeeklyQuests = new List<QuestsProgress>();
            InvestingProgresses = new InvestingProgress();
            ShopCurrencyProductProgress = new List<ShopCurrencyProductProgress>();
            ShopPurchaseProductProgresses = new List<ShopPurchaseProductProgress>();
            OfferProgresses = new OfferProgress();
            DailyEntryProgresses = new List<DailyEntryProgress>
            {
                new (DailyEntryType.Day1, 0,false , false),
                new (DailyEntryType.Day2, 0,false , false),
                new (DailyEntryType.Day3, 0,false , false),
                new (DailyEntryType.Day4, 0,false , false),
                new (DailyEntryType.Day5, 0,false , false),
                new (DailyEntryType.Day6, 0,false , false),
                new (DailyEntryType.Day7, 0,false , false),
            };

            MoneyBoxProgressData = new MoneyBoxProgressData();
            BattlePassProgressData = new BattlePassProgressData();
            DateCreateWeekQuests = DateTime.Now.ToString();
            BattlePassFirstDay = DateTime.Now.ToString();
        }
        
        public RegionProgress GetRegionProgress()
        {
            var regionProgress = RegionProgresses.Find(x => x.IsSelected);

            if (regionProgress == null)
            {
                return RegionProgresses.First();
            }

            return regionProgress;
        }

        public QuestsProgress GetOrCreateQuestProgress(List<QuestsProgress> questsProgresses, QuestsGuid id, int progress, bool isTakeReward)
        {
            foreach (var quest in questsProgresses)
            {
                if (quest.Guid == id)
                {
                    return quest;
                }
            }

            QuestsProgress questsProgress = new(progress, id, isTakeReward);
            questsProgresses.Add(questsProgress);

            return questsProgress;
        }

        public void RemoveQuest(QuestsGuid id, List<QuestsProgress> quests)
        {
            foreach (var quest in quests)
            {
                if (quest.Guid == id)
                {
                    quests.Remove(quest);
                    return;
                }
            }
        }
        
        public DailyEntryProgress GetOrCreateDailyEntry(DailyEntryType day)
        {
           var progress = DailyEntryProgresses.Find(x => x.Day == day);
           
           if (progress != null)
           {
               return progress;
           }
           
           progress = new DailyEntryProgress(day,0,false,false);
           DailyEntryProgresses.Add(progress);
           
           return progress;
        }

        public void ReCreateProgress()
        {
            DailyEntryProgresses.Clear();
            DailyEntryProgresses = new List<DailyEntryProgress>
            {
                new (DailyEntryType.Day1, 0,false , false),
                new (DailyEntryType.Day2, 0,false , false),
                new (DailyEntryType.Day3, 0,false , false),
                new (DailyEntryType.Day4, 0,false , false),
                new (DailyEntryType.Day5, 0,false , false),
                new (DailyEntryType.Day6, 0,false , false),
                new (DailyEntryType.Day7, 0,false , false),
            };
        }

        public void CreatePromoCodeProgress(string id, string fromTime, string toTime)
        {
            var promoCodeProgress = PromoCodeProgresses.Find(x => x.Key == id);

            if (promoCodeProgress != null)
            {
                return;
            }

            promoCodeProgress = new PromoCodeProgress()
            {
                Key = id,
                FromDateTime = fromTime,
                ToTheTime = toTime,
                IsEntered = false,
                isExpired = false,
            };

            PromoCodeProgresses.Add(promoCodeProgress);
        }

        public ShopCurrencyProductProgress GetOrCreateCurrencyProductProgress(string id)
        {
            ShopCurrencyProductProgress currencyProductProgress = ShopCurrencyProductProgress
                .Find(x => x.Id == id);

            if (currencyProductProgress != null)
            {
                return currencyProductProgress;
            }

            currencyProductProgress = new ShopCurrencyProductProgress
            {
                Id = id,
            };

            ShopCurrencyProductProgress.Add(currencyProductProgress);

            return currencyProductProgress;
        }

        public ShopPurchaseProductProgress GetOrCreatePurchaseProductProgress(string id)
        {
            ShopPurchaseProductProgress purchaseProductProgress = ShopPurchaseProductProgresses
                .Find(x => x.Id == id);

            if (purchaseProductProgress != null)
            {
                return purchaseProductProgress;
            }

            purchaseProductProgress = new ShopPurchaseProductProgress
            {
                Id = id,
            };

            ShopPurchaseProductProgresses.Add(purchaseProductProgress);

            return purchaseProductProgress;
        }
    }
}