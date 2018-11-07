namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using AtomicTorch.CBND.GameApi.Resources;
    using JetBrains.Annotations;
    using System;

    public class ObjectCorpseViewModel : ProtoStaticWorldObjectViewModel
    {
        public ObjectCorpseViewModel([NotNull] ObjectCorpse corpse)
            : base(corpse)
        {
            IconResource = new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            EntityInformation.Add(new ViewModelEntityInformation("Disapear after",
                TimeSpan.FromSeconds(ObjectCorpse.CorpseTimeoutSeconds)));
        }
    }
}