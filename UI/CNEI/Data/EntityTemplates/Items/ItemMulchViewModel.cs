namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using JetBrains.Annotations;

    public class ItemMulchViewModel : ProtoItemViewModel
    {
        public ItemMulchViewModel([NotNull] IProtoItem mulch) : base(mulch)
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

            EntityInformation.Add(new ViewModelEntityInformation("Organic value for craft", 10));
        }
    }
}