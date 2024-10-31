using System.Collections.Generic;
using Services.Bank;
using Services.Factory;
using SO;
using Triggers;
using Turrets;

namespace Infrastructure.Factory.Pools
{
    public class PoolTurrets
    {
       private readonly List<Turret> _turrets = new();

       public IReadOnlyList<Turret> Turrets =>
           _turrets.AsReadOnly();

        public PoolTurrets(IGameFactory factory, TurretPlate[] turretPlates, TurretData turretData,
            PoolMissiles poolMissiles, BaseGate baseGate)
        {
            int maxCount = turretPlates.Length;

            for (int i = 0; i < maxCount; i++)
            {
                Turret turret = factory.CreateTurret();
                turret.Construct(turretPlates[i].GetCartridgeGun, turretData, poolMissiles, baseGate);
                turret.InActive();
                _turrets.Add(turret);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Turret turret in _turrets)
            {
                turret.InActive();
            }
        }
    }
}