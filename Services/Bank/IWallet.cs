using System;

namespace Services.Bank
{
    public interface IWallet
    {
        event Action<int> MoneyChanged;
        event Action<int> GemChanged;
        void ApplyMoney(int money);
        void SpendMoney(int money);
        void ApplyGem(int gem);
        void SpendGem(int gem);
        bool Check(int price);
        int ReadCurrentMoney();
        int ReadCurrentGem();
    }
}