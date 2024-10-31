using System.Collections.Generic;
using Assistant;
using Canvases;
using ContactZones;
using Services.Factory;
using SO;
using Turrets.Children;

namespace Infrastructure.Factory.Pools
{
    public class PoolCargoAssistant
    {
        private readonly List<CargoAssistant> _assistants = new();

        public IReadOnlyList<CargoAssistant> Assistants =>
            _assistants.AsReadOnly();

        public PoolCargoAssistant(IGameFactory factory, PoolData poolData, AssistantData assistantData,
            CartridgeGun[] cartridgeGuns, StorageAmmoPlate storageAmmoPlate, SectionPlate[] sectionPlates)
        {
            int maxCountAssistant = poolData.MaxCountCargoAssistant;
            
            for (int i = 0; i < maxCountAssistant; i++)
            {
                CargoAssistant cargoAssistant = factory.CreateCargoAssistant();
                cargoAssistant.Construct(assistantData, cartridgeGuns, storageAmmoPlate, sectionPlates);
                cargoAssistant.InActive();
                _assistants.Add(cargoAssistant);
            }
        }

        public void AdaptingLevel()
        {
            foreach (CargoAssistant assistant in _assistants) 
                assistant.InActive();
        }
    }
}