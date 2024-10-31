using GameAnalyticsSDK;

namespace Canvases.UpgradePlayer
{
    public class HeroSpeedLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroSpeed;

        protected override void UpdateValue()
        {
            CurrentValue = HeroData.Speed;
            
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Hero upgrade:Speed {HeroData.Speed}");
#endif
        }

        protected override void Upgrade() =>
            HeroData.Speed++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroSpeed += PriceList.MultiplierPriceHeroSpeed;
        
        protected override bool CheckUpperLimit() => 
            HeroData.Speed == HeroData.UpperLimitSpeed;
    }
}