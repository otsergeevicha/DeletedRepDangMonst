using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataStateGame
    {
        public bool FirstLaunch = true;
        
        public int CurrentLevel = 1;
        
        public int HeroHealth = 1;
        public int HeroSpeed = 3;
        public int HeroSizeBasket = 5;
        public float HeroRadiusDetection = 5f;
    }
}