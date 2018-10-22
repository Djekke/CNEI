namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools.WateringCans;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolWateringCanViewModel : ProtoItemViewModel
    {
        private readonly IProtoItemToolWateringCan wateringCan;

        public ProtoItemToolWateringCanViewModel([NotNull] IProtoItemToolWateringCan wateringCan) : base(wateringCan)
        {
            this.wateringCan = wateringCan;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            EntityInformation.Add(new ViewModelEntityInformation("Water capacity", wateringCan.WaterCapacity));
        }
    }
}