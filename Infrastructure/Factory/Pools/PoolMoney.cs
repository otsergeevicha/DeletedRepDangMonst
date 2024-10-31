using System.Collections.Generic;
using Loots;
using Services.Factory;

namespace Infrastructure.Factory.Pools
{
    public class PoolMoney
    {
        public readonly List<Money> Moneys = new();
        
        public PoolMoney(IGameFactory factory, int maxCountMoney)
        {
            for (int i = 0; i < maxCountMoney; i++)
            {
                Money money = factory.CreateMoney();
                money.gameObject.SetActive(false);
                Moneys.Add(money);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Money money in Moneys) 
                money.gameObject.SetActive(false);
        }
    }
}