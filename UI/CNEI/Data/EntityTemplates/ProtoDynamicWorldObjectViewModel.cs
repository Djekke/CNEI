namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.GameApi.Data.World;
    using JetBrains.Annotations;

    public abstract class ProtoDynamicWorldObjectViewModel : ProtoEntityWithRecipeBondsViewModel
    {
        protected ProtoDynamicWorldObjectViewModel([NotNull] IProtoDynamicWorldObject dynamicWorldObject)
            : base(dynamicWorldObject)
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

            if (ProtoEntity is IProtoDynamicWorldObject dynamicWorldObject)
            {
                EntityInformation.Add(new ViewModelEntityInformation("HP",
                    dynamicWorldObject.StructurePointsMax));
                EntityInformation.Add(new ViewModelEntityInformation("Object sound radius",
                    dynamicWorldObject.ObjectSoundRadius));
            }
        }
    }
}