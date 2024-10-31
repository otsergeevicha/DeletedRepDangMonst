using System;
using Infrastructure.GroupData;

namespace SaveLoadModule
{
    [Serializable]
    public class Progress
    {
        public DataWallet DataWallet;
        public DataStateGame DataStateGame;

        public Progress()
        {
            DataWallet = new DataWallet();
            DataStateGame = new DataStateGame();
        }
    }
}