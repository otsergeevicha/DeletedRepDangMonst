using System.Collections.Generic;
using System.Linq;
using Ammo;
using Services.Factory;
using UnityEngine;

namespace Infrastructure.Factory.Pools
{
    public class PoolAmmoBoxPlayer
    {
        private readonly List<AmmoBox> _ammoBoxes = new();
        private AmmoBox _box;

        public PoolAmmoBoxPlayer(IGameFactory factory, int maxCountAmmoBox)
        {
            for (int i = 0; i < maxCountAmmoBox; i++)
            {
                _box = factory.CreateAmmoBox();
                _box.InActive();
                _ammoBoxes.Add(_box);
            }

            _box = null;
        }

        public void AcceptBox()
        {
            _box = _ammoBoxes.FirstOrDefault(box =>
                box.isActiveAndEnabled == false);

            if (_box != null)
                _box.OnActive();
            
            _box = null;
        }

        public void SpendBox()
        {
            _box = _ammoBoxes.LastOrDefault(box =>
                box.isActiveAndEnabled);

            if (_box != null)
                _box.InActive();
            
            _box = null;
        }

        public void FirstPointPosition(Transform basketTransform)
        {
            float ratePositionY = 0;

            for (int i = 0; i < _ammoBoxes.Count; i++)
            {
                _ammoBoxes[i].transform.parent = basketTransform;
                _ammoBoxes[i].SetPosition(new Vector3(basketTransform.position.x, basketTransform.position.y + ratePositionY, basketTransform.position.z));
                ratePositionY += .4f;
            }
        }

        public void AdaptingLevel()
        {
            foreach (AmmoBox box in _ammoBoxes) 
                box.InActive();
        }
        
        public void AllInActive()
        {
            foreach (AmmoBox box in _ammoBoxes) 
                box.InActive();
        }
    }
}