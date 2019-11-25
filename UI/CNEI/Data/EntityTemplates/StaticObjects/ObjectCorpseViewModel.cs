namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Threading.Tasks;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using AtomicTorch.CBND.GameApi.Resources;
    using JetBrains.Annotations;

    public class ObjectCorpseViewModel : ProtoStaticWorldObjectViewModel
    {
        public ObjectCorpseViewModel([NotNull] ObjectCorpse corpse)
            : base(corpse)
        {
        }

        /// <summary>
        /// Uses in texture procedural generation.
        /// </summary>
        /// <param name="request">Request from ProceduralTexture generator</param>
        /// <param name="textureWidth">Texture width</param>
        /// <param name="textureHeight">Texture height</param>
        /// <param name="spriteQualityOffset">Sprite quality modifier (0 = full size, 1 = x0.5, 2 = x0.25)</param>
        /// <returns></returns>
        public override async Task<ITextureResource> GenerateIcon(
            ProceduralTextureRequest request,
            ushort textureWidth = 512,
            ushort textureHeight = 512,
            sbyte spriteQualityOffset = 0)
        {
            // Override default texture generations, as for CNEI corpse icon is not set and not needed.
            return new TextureResource(
                localFilePath: "Content/Textures/StaticObjects/ObjectUnknown.png",
                qualityOffset: spriteQualityOffset);
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            EntityInformation.Add(new ViewModelEntityInformation("Disappear after",
                TimeSpan.FromSeconds(ObjectCorpse.CorpseTimeoutSeconds)));
        }
    }
}