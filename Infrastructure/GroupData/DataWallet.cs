using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataWallet
    {
        public int RemainingMoney = 0;
        public int RemainingGem = 0;

        public void RecordMoney(int remainingMoney) => 
            RemainingMoney = remainingMoney;
        
        public void RecordGem(int remainingGem) => 
            RemainingGem = remainingGem;
    }
}