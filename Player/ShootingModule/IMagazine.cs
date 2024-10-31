using System;

namespace Player.ShootingModule
{
    public interface IMagazine
    {
        void Spend();
        bool Check();

        void Replenishment(Action fulled);

        void Shortage();
        void UpdateLevel();
    }
}