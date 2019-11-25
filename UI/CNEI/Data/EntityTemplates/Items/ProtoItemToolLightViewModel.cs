namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Tools.Lights;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolLightViewModel : ProtoItemViewModel
    {
        public ProtoItemToolLightViewModel([NotNull] IProtoItemToolLight light) : base(light)
        {
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemToolLight light)
            {
                if (light.ItemLightConfig?.IsLightEnabled == true)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Light size", "(" +
                        light.ItemLightConfig.Size.X + ", " +
                        light.ItemLightConfig.Size.Y + ")"));
                }

                if (light.ItemFuelConfig != null)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Fuel initial",
                        light.ItemFuelConfig.FuelAmountInitial));
                    EntityInformation.Add(new ViewModelEntityInformation("Fuel max",
                        light.ItemFuelConfig.FuelCapacity));
                    EntityInformation.Add(new ViewModelEntityInformation("Fuel use",
                        light.ItemFuelConfig.FuelUsePerSecond));
                    if (light.ItemFuelConfig.FuelProtoItemsList?.Count > 0)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Compatible fuel",
                            light.ItemFuelConfig.FuelProtoItemsList.Select(EntityViewModelsManager
                                .GetEntityViewModel)));
                    }
                }
            }
        }
    }
}