namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Equipment;
    using AtomicTorch.CBND.CoreMod.Stats;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

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
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(),
                            (pair.Value * 100).ToString() + "%"));
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
                        EntityInformation.Add(new ViewModelEntityInformation("Light size", "(" +
                            headWithLight.ItemLightConfig.Size.X + ", " +
                            headWithLight.ItemLightConfig.Size.Y + ")"));
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