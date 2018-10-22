namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools.Lights;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoItemToolLightViewModel : ProtoItemViewModel
    {
        private readonly IProtoItemToolLight light;

        public ProtoItemToolLightViewModel([NotNull] IProtoItemToolLight light) : base(light)
        {
            this.light = light;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (light?.ItemLightConfig != null)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Light radius",
                    light.ItemLightConfig.LightSize));
            }

            if (light?.ItemFuelConfig != null)
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
                        light.ItemFuelConfig.FuelProtoItemsList.Select(EntityViewModelsManager.GetEntityViewModel)));
                }
            }
        }
    }
}