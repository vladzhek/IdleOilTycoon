using Gameplay.Region;
using Zenject;

namespace Gameplay.BattlePass
{
    public class BuyBattlePassButton : ButtonBaseSFX
    {
        private BattlePassModel _battlePassModel;

        [Inject]
        private void Construct(BattlePassModel battlePassModel)
        {
            _battlePassModel = battlePassModel;
        }

        public override void OnClick()
        {
            base.OnClick();

            _battlePassModel.BuyBattlePass();
        }
    }
}