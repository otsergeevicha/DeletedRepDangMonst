using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPriceList", menuName = "Price List", order = 1)]
    public class PriceListData : ScriptableObject
    {
        [Range(1, 1000)] 
        public int SectionPriceMultiplier = 100;

        [Range(3, 1000)] 
        public int PriceTransitionPlate = 3;

        [Range(3, 1000)] 
        public int MultiplierIncreasePrice = 3;
        
        [Range(100, 5000)]
        public int StartPriceTurret = 100;        
        [Range(100, 5000)]
        public int StepIncreasePriceTurret = 100;

        [Header("Price upgrade hero")] 
        [Range(50, 1000)]
        public int PriceHeroHealth = 50;
        [Range(3, 1000)] 
        public int MultiplierPriceHeroHealth = 3;
        [Range(10, 100)] 
        public int StepIncreaseHealthHero = 10;
        
        [Range(50, 1000)]
        public int PriceHeroSpeed = 50;
        public readonly int MultiplierPriceHeroSpeed = 1;
        
        [Range(50, 1000)]
        public int PriceHeroBasket = 50;
        public readonly int MultiplierPriceHeroBasket = 1;
        
        [Range(50, 1000)]
        public int PriceHeroFiringRange = 50;
        public readonly int MultiplierPriceHeroFiringRange = 1;
        
        [Range(50, 1000)]
        public int LoseBonusMoney = 500;
        [Range(50, 1000)]
        public int LoseBonusGem = 50;
    }
}