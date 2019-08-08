namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.CharacterSkeletons;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.Technologies;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource DefaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        public override string ResourceDictonaryName => "ProtoCharacterMobDataTemplate.xaml";

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature)
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
            var creature = ProtoEntity as IProtoCharacterMob;
            creature.SharedGetSkeletonProto(null, out var creatureSkeleton, out var scale);
            var worldScale = 1.0;
            if (creatureSkeleton is ProtoCharacterSkeletonAnimal animalSkeleton)
            {
                worldScale = animalSkeleton.WorldScale * 2;
            }
            string RenderingTag = request.TextureName;

            var renderTarget = Api.Client.Rendering.CreateRenderTexture(RenderingTag, textureWidth, textureHeight);
            var cameraObject = Api.Client.Scene.CreateSceneObject(RenderingTag);
            var camera = Api.Client.Rendering.CreateCamera(cameraObject,
                                                                 renderingTag: RenderingTag,
                                                                 drawOrder: -10);

            camera.RenderTarget = renderTarget;
            camera.ClearColor = Color.FromArgb(0, 0, 0, 0);
            camera.SetOrthographicProjection(textureWidth, textureHeight);

            var currentSkeleton = ClientCharacterEquipmentHelper.CreateCharacterSkeleton(
                                    cameraObject,
                                    creatureSkeleton,
                                    worldScale: worldScale,
                                    spriteQualityOffset: spriteQualityOffset);
            currentSkeleton.PositionOffset = (textureWidth / 2d, -textureHeight * 0.70);
            currentSkeleton.RenderingTag = RenderingTag;

            await camera.DrawAsync();
            cameraObject.Destroy();

            request.ThrowIfCancelled();

            var generatedTexture = await renderTarget.SaveToTexture(
                    isTransparent: true,
                    qualityScaleCoef: Api.Client.Rendering.CalculateCurrentQualityScaleCoefWithOffset(
                        spriteQualityOffset));

            currentSkeleton.Destroy();
            renderTarget.Dispose();
            request.ThrowIfCancelled();
            return generatedTexture;
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
                EntityInformation.Add(new ViewModelEntityInformation("Experience for kill", ""));
                EntityInformation.Add(new ViewModelEntityInformation("- Multiplier",
                    characterMob.MobKillExperienceMultiplier + "x"));
                // \Scripts\Systems\Weapons\WeaponSystem.cs:475
                var experienceForKill = characterMob.MobKillExperienceMultiplier * SkillHunting.ExperienceForKill;
                EntityInformation.Add(new ViewModelEntityInformation("- Total experience",
                    experienceForKill));
                // \Scripts\Skills\Base\ProtoSkill.cs:163
                var skillHunting = Api.FindProtoEntities<SkillHunting>().FirstOrDefault();
                if (skillHunting == null)
                {
                    Api.Logger.Error("CNEI: Error geting skillHunting for LP calculation for mob " + Title + " kill.");
                    return;
                }
                var multiplier = skillHunting.ExperienceToLearningPointsConversionMultiplier
                                 * TechConstants.SkillExperienceToLearningPointsConversionMultiplier;
                if (multiplier <= 0)
                {
                    Api.Logger.Error("CNEI: Error LP multiplier less then zero for mob " + Title + " kill.");
                    return;
                }
                var lpAtZeroLevel = experienceForKill * multiplier;
                EntityInformation.Add(new ViewModelEntityInformation("- Total LP (at skill level 0)",
                    lpAtZeroLevel.ToString("F3")));
                var lpAtMaxLevel = lpAtZeroLevel * TechConstants.SkillLearningPointMultiplierAtMaximumLevel;
                EntityInformation.Add(new ViewModelEntityInformation("- Total LP (at max skill level)",
                    lpAtMaxLevel.ToString("F3")));
            }
        }

        public RecipeViewModel Droplist { get; private set; }

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;
    }
}