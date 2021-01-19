namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.Stats;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD.Data;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoStatusEffectViewModel : ProtoEntityWithRecipeBondsViewModel
    {
        public override string ResourceDictionaryName => "ProtoStatusEffectDataTemplate.xaml";

        public ProtoStatusEffectViewModel([NotNull] IProtoStatusEffect statusEffect)
            : base(statusEffect)
        {
            Description = statusEffect.Description;
            Kind = statusEffect.Kind;
        }

        /// <summary>
        /// Uses in texture procedural generation.
        /// </summary>
        /// <param name="request">Request from ProceduralTexture generator</param>
        /// <param name="textureWidth">Texture width</param>
        /// <param name="textureHeight">Texture height</param>
        /// <param name="spriteQualityOffset">Sprite quality modifier (0 = full size, 1 = x0.5, 2 = x0.25)</param>
        /// <returns></returns>
        //public override async Task<ITextureResource> GenerateIcon(
        //    ProceduralTextureRequest request,
        //    ushort textureWidth = 512,
        //    ushort textureHeight = 512,
        //    sbyte spriteQualityOffset = 0)
        //{
        //    var statusEffect = ProtoEntity as IProtoStatusEffect;
        //    var icon = statusEffect.Icon;
        //    if (icon == null || icon == TextureResource.NoTexture)
        //    {
        //        icon = new TextureResource(
        //              localFilePath: "Content/Textures/StaticObjects/ObjectUnknown.png",
        //              qualityOffset: spriteQualityOffset);
        //    }
        //
        //    var textureSize = await Api.Client.Rendering.GetTextureSize(icon);
        //    request.ThrowIfCancelled();
        //
        //    var renderingTag = request.TextureName;
        //    var cameraObject = Api.Client.Scene.CreateSceneObject(renderingTag);
        //
        //    var camera = Api.Client.Rendering.CreateCamera(cameraObject,
        //                                                   renderingTag,
        //                                                   drawOrder: -10);
        //
        //    var renderTexture = Api.Client.Rendering.CreateRenderTexture(renderingTag,
        //                                                                 textureSize.X,
        //                                                                 textureSize.Y);
        //
        //    Api.Client.Rendering.CreateSpriteRenderer(cameraObject,
        //                                              icon,
        //                                              spritePivotPoint: (0, 1),
        //                                              renderingTag: renderingTag);
        //
        //    camera.RenderTarget = renderTexture;
        //    camera.ClearColor = ((SolidColorBrush)ClientStatusEffectIconColorizer.GetBrush(Kind, 1.0)).Color;
        //    camera.SetOrthographicProjection(textureSize.X, textureSize.Y);
        //
        //    await camera.DrawAsync();
        //    cameraObject.Destroy();
        //
        //    request.ThrowIfCancelled();
        //
        //    var generatedTexture = await renderTexture.SaveToTexture(isTransparent: true,
        //        qualityScaleCoef: Api.Client.Rendering.CalculateCurrentQualityScaleCoefWithOffset(spriteQualityOffset));
        //
        //    renderTexture.Dispose();
        //    request.ThrowIfCancelled();
        //
        //    return generatedTexture;
        //}

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoStatusEffect statusEffect)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Status effect kind",
                    statusEffect.Kind.ToString()));
                EntityInformation.Add(new ViewModelEntityInformation("Is removed on respawn",
                    statusEffect.IsRemovedOnRespawn ? "Yes" : "No"));
                EntityInformation.Add(new ViewModelEntityInformation("Visibility threshold",
                    statusEffect.VisibilityIntensityThreshold == double.MaxValue
                    ? "Invisible"
                    : (statusEffect.VisibilityIntensityThreshold * 100) + "%"));

                if (statusEffect.ProtoEffects?.Values.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in statusEffect.ProtoEffects.Values)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }

                if (statusEffect.ProtoEffects?.Multipliers.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in statusEffect.ProtoEffects.Multipliers)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }

                if (statusEffect.IntensityAutoDecreasePerSecondValue > 0)
                {
                    double seconds = 1.0 / statusEffect.IntensityAutoDecreasePerSecondValue;
                    EntityInformation.Add(new ViewModelEntityInformation("Auto-disappear in",
                        TimeSpan.FromSeconds(seconds)));
                }
            }
        }

        public void AddConsumable([NotNull] ProtoEntityViewModel consumableViewModel, double intensity)
        {
            if (Consumables.Count == 0)
            {
                ConsumableEffect = new ConsumableEffectViewModel(this, consumableViewModel, intensity);
                EntityViewModelsManager.AddRecipe(ConsumableEffect);
            }
            else
            {
                ConsumableEffect.AddConsumable(consumableViewModel, intensity);
            }

            Consumables.Add(consumableViewModel);
        }

        public ConsumableEffectViewModel ConsumableEffect { get; private set; }

        public List<ProtoEntityViewModel> Consumables { get; private set; }
            = new List<ProtoEntityViewModel>();

        public StatusEffectKind Kind { get; }

        //public Brush GetBackgroundBrush(double intensity)
        //{
        //    return ClientStatusEffectIconColorizer.GetBrush(Kind, intensity);
        //}
    }
}