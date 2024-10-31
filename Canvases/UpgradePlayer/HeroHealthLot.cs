using GameAnalyticsSDK;

namespace Canvases.UpgradePlayer
{
    public class HeroHealthLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroHealth;

        protected override void UpdateValue()
        {
            CurrentValue = HeroData.MaxHealth;
            
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Hero upgrade:MaxHealth {HeroData.MaxHealth}");
#endif
        }

        protected override void Upgrade() => 
            HeroData.MaxHealth += PriceList.StepIncreaseHealthHero;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroHealth += PriceList.MultiplierPriceHeroHealth;

        protected override bool CheckUpperLimit() => 
            HeroData.MaxHealth == HeroData.UpperLimitMaxHealth;
    }
}