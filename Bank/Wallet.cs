using System;
using Services.Bank;
using Services.SaveLoad;
using UnityEngine;

namespace Bank
{
    public class Wallet : IWallet
    {
        private readonly ISave _save;
        private int _currentMoney;
        private int _currentGem;

        public Wallet(ISave save)
        {
            _save = save;
            _currentMoney = save.AccessProgress().DataWallet.RemainingMoney;
            _currentGem = save.AccessProgress().DataWallet.RemainingGem;
        }

        public event Action<int> MoneyChanged;
        public event Action<int> GemChanged;

        public void ApplyMoney(int money)
        {
            _currentMoney += money;
            MoneyChanged?.Invoke(_currentMoney);
            WritingSave();
        }

        public void SpendMoney(int money)
        {
            _currentMoney -= Mathf.Clamp(money, 0, int.MaxValue);
            MoneyChanged?.Invoke(_currentMoney);
            WritingSave();
        }

        public void ApplyGem(int gem)
        {
            _currentGem += gem;
            GemChanged?.Invoke(_currentGem);
            WritingSave();
        }

        public void SpendGem(int gem)
        {
            _currentGem -= Mathf.Clamp(gem, 0, int.MaxValue);
            GemChanged?.Invoke(_currentGem);
            WritingSave();
        }

        public bool Check(int price) => 
            _currentMoney >= price;

        public int ReadCurrentMoney() => 
            _save.AccessProgress().DataWallet.RemainingMoney;

        public int ReadCurrentGem() => 
            _save.AccessProgress().DataWallet.RemainingGem;

        private void WritingSave()
        {
            _save.AccessProgress().DataWallet.RecordMoney(_currentMoney);
            _save.AccessProgress().DataWallet.RecordGem(_currentGem);
            _save.Save();
        }
    }
}