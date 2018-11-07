namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Equipment;
    using AtomicTorch.CBND.CoreMod.Stats;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Linq;

    public class ProtoItemEquipmentViewModel : ProtoItemViewModel
    {
        public ProtoItemEquipmentViewModel([NotNull] IProtoItemEquipment equipment) : base(equipment)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemEquipment equipment)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Equipment type",
                    equipment.EquipmentType.ToString()));

                if (equipment?.ProtoEffects?.Values.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in equipment.ProtoEffects.Values)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }

                if (equipment?.ProtoEffects?.Multipliers.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in equipment.ProtoEffects.Multipliers)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }

                if (equipment is IProtoItemEquipmentHeadWithLight headWithLight)
                {
                    if (headWithLight?.ItemLightConfig?.IsLightEnabled == true)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Light radius",
                            headWithLight.ItemLightConfig.LightSize));
                    }

                    if (headWithLight?.ItemFuelConfig != null)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Fuel initial",
                            headWithLight.ItemFuelConfig.FuelAmountInitial));
                        EntityInformation.Add(new ViewModelEntityInformation("Fuel max",
                            headWithLight.ItemFuelConfig.FuelCapacity));
                        EntityInformation.Add(new ViewModelEntityInformation("Fuel use",
                            headWithLight.ItemFuelConfig.FuelUsePerSecond));
                        if (headWithLight.ItemFuelConfig.FuelProtoItemsList?.Count > 0)
                        {
                            EntityInformation.Add(new ViewModelEntityInformation("Compatible fuel",
                                headWithLight.ItemFuelConfig.FuelProtoItemsList.Select(EntityViewModelsManager
                                    .GetEntityViewModel)));
                        }
                    }
                }
            }
        }
    }
}