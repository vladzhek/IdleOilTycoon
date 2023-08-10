using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Gameplay.Investing.UI
{
    public class InvestingView : MonoBehaviour
    {
        public event Action InvestingClick;

        [SerializeField] private TextWithIconView _upperView;
        [SerializeField] private TextWithIconView _footerView;
        [SerializeField] private InvestingButton _button;
        [SerializeField] private Sprite _adsIcon;

        public void SetDefaultState(Sprite coinSprite, int amountHard, int amountSoft)
        {
            _upperView.SetTextView("Вложите");
            _upperView.SetIcon(coinSprite);
            _upperView.SetActiveIcon(true);
            _upperView.SetAmountView(IntExtensions.ToFormattedBigNumber(amountHard));
            
            _footerView.SetTextView("и получите доход");
            _footerView.SetActiveIcon(true);
            _footerView.SetAmountView(IntExtensions.ToFormattedBigNumber(amountSoft));
            
            _button.SetText($"{amountHard} Вложить");

            _button.SetIcon(coinSprite);
            _button.IconActive(true);
            _button.IsActiveAdsBG(false);
            _button.IsActiveDisableIcon(false);
        }
        
        public void SetProcessState(Sprite icon, int amount)
        {
            _upperView.SetTextView("Доход");
            _upperView.SetAmountView(IntExtensions.ToFormattedBigNumber(amount));
            _upperView.SetIcon(icon);
            
            _footerView.SetTextView("поступит через: ");
            _footerView.SetAmountView("4ч 00м");
            _footerView.SetActiveIcon(false);
            
            _button.SetText("Ускорить на 10 мин");
            _button.SetIcon(_adsIcon);
            _button.IsActiveAdsBG(true);
            _button.IconActive(true);
            _button.IsActiveDisableIcon(false);
        }

        public void SetReadyToTakeState(int amount)
        {
            _upperView.SetTextView("Поздравляем!");
            _upperView.SetActiveIcon(false);
            _upperView.SetAmountView("");

            _footerView.SetTextView("Ваш доход:");
            _footerView.SetActiveIcon(true);
            _footerView.SetAmountView(amount.ToString());
            
            _button.SetText("Получить");

            _button.IconActive(false);
            _button.IsActiveAdsBG(false);
            _button.IsActiveDisableIcon(false);
        }
        
        public void SetDisableButtonADS()
        {
            _button.IsActiveDisableIcon(true);
        }
        
        public void UpdateProcessState(int time)
        {
            var timeFormat = FormatTime.HoursStringFormat(time);
            _footerView.SetAmountView(timeFormat);
        }

        public void Click()
        {
            InvestingClick?.Invoke();
        }
    }
}