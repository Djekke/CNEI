namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using JetBrains.Annotations;

    public class ProtoItemFertilizerViewModel : ProtoItemViewModel
    {
        public ProtoItemFertilizerViewModel([NotNull] IProtoItemFertilizer fertilizer) : base(fertilizer)
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

            if (ProtoEntity is IProtoItemFertilizer itemFertilizer)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Plant grow speed increase",
                    "+" + ((itemFertilizer.PlantGrowthSpeedMultiplier - 1.0) * 100) + "%"));
            }
        }
    }
}