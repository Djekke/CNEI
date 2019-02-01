namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.CBND.GameApi.ServicesClient.Components.Camera;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;
    using System.Windows.Media;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource DefaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        public override string ResourceDictonaryName => "ProtoCharacterMobDataTemplate.xaml";

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature, DefaultIcon)
        {
            creature.SharedGetSkeletonProto(null, out var creatureSkeleton, out var scale);

            ushort textureWidth = 256;
            ushort textureHeight = 256;
            string RenderingTag = Title + " skeleton camera";
            var sceneObjectCamera = Api.Client.Scene.CreateSceneObject(RenderingTag);
            var camera = Api.Client.Rendering.CreateCamera(sceneObjectCamera,
                                                           renderingTag: RenderingTag,
                                                           drawOrder: -10,
                                                           drawMode: CameraDrawMode.Auto);

            var renderTarget = Api.Client.Rendering.CreateRenderTexture(RenderingTag, textureWidth, textureHeight);
            camera.RenderTarget = renderTarget;
            camera.ClearColor = Color.FromArgb(0, 0, 0, 0);
            camera.SetOrthographicProjection(textureWidth, textureHeight);

            var sceneObjectSkeleton = Api.Client.Scene.CreateSceneObject(Title + " skeleton renderer");

            var currentSkeleton = ClientCharacterEquipmentHelper.CreateCharacterSkeleton(
                                    sceneObjectSkeleton,
                                    creatureSkeleton,
                                    worldScale: 115.0 / textureWidth,
                                    spriteQualityOffset: -1);

            if (currentSkeleton != null)
            {
                currentSkeleton.PositionOffset = (textureWidth / 2d, -textureHeight * 0.70);
                currentSkeleton.RenderingTag = RenderingTag;

                var ImageBrush = Api.Client.UI.CreateImageBrushForRenderTarget(renderTarget);
                ImageBrush.Stretch = Stretch.Uniform;

                icon = ImageBrush;
            }
            else
            {
                Api.Logger.Error("CNEI: Failed to create skeleton for " + Title);
            }
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoCharacterMob creature &&
                creature.LootDroplist != null &&
                creature.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new RecipeViewModel(this, creature.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is ProtoCharacterMob characterMob)
            {
                EntityInformation.Add(new ViewModelEntityInformation("HP",
                    characterMob.StatDefaultHealthMax));
                EntityInformation.Add(new ViewModelEntityInformation("Move speed",
                    characterMob.StatMoveSpeed));
                EntityInformation.Add(new ViewModelEntityInformation("Kill exp multiplier",
                    characterMob.MobKillExperienceMultiplier));
            }
        }

        public RecipeViewModel Droplist { get; private set; }

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;
    }
}