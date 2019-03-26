namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Extensions;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    public class ProtoEntityViewModel : BaseViewModel
    {
        protected Brush icon;

        private HashSet<RecipeViewModel> recipeVMList = new HashSet<RecipeViewModel>();

        private HashSet<RecipeViewModel> usageVMList = new HashSet<RecipeViewModel>();

        public virtual IProtoEntity ProtoEntity { get; }

        public virtual string ResourceDictonaryName => "ProtoEntityDataTemplate.xaml";

        public ProtoEntityViewModel([NotNull] IProtoEntity entity)
        {
            ProtoEntity = entity;
            Title = entity.Name == "" ? entity.GetType().Name : entity.Name;
            TitleLower = entity.Name.ToLower();
            Type = entity.Id;
            TypeLower = entity.Id.ToLower();
            RecipeVMList = new FilteredObservableWithPaging<RecipeViewModel>();
            UsageVMList = new FilteredObservableWithPaging<RecipeViewModel>();
            EntityInformation = new ObservableCollection<ViewModelEntityInformation>();
        }

        /// <summary>
        /// Uses in texture procedural generation.
        /// </summary>
        /// <param name="request">Request from ProceduralTexture generator</param>
        /// <param name="textureWidth">Texture width</param>
        /// <param name="textureHeight">Texture height</param>
        /// <param name="spriteQualityOffset">Sprite quality modifier (0 = full size, 1 = x0.5, 2 = x0.25)</param>
        /// <returns></returns>
        public virtual async Task<ITextureResource> GenerateIcon(
            ProceduralTextureRequest request,
            ushort textureWidth = 512,
            ushort textureHeight = 512,
            sbyte spriteQualityOffset = 0)
        {
            if (!(GetPropertyByName(ProtoEntity, "Icon") is ITextureResource iconResource))
            {
                // Default icon.
                iconResource = new TextureResource(
                    localFilePath: "Content/Textures/StaticObjects/ObjectUnknown.png",
                    qualityOffset: spriteQualityOffset);
            }
            return iconResource;
        }

        /// <summary>
        /// Entity icon.
        /// </summary>
        public virtual Brush Icon
        {
            get
            {
                if (icon == null)
                {
                    icon = Api.Client.UI.GetTextureBrush(
                        new ProceduralTexture("CNEI icon for " + Title,
                            proceduralTextureRequest => GenerateIcon(proceduralTextureRequest),
                            isTransparent: true,
                            isUseCache: false));
                }
                return icon;
            }
        }

        /// <summary>
        /// Entity name.
        /// </summary>
        public virtual string Title { get; }

        public string TitleLower;

        /// <summary>
        /// C# type full name (with namespace).
        /// </summary>
        public virtual string Type { get; }

        public string TypeLower;

        public string Description { get; set; }

        public virtual Visibility Visibility => Visibility.Visible;

        public Visibility TypeVisibility => EntityViewModelsManager.TypeVisibility;

        public virtual Visibility CountVisibility => Visibility.Collapsed;

        public ObservableCollection<ViewModelEntityInformation> EntityInformation { get; set; }

        public FilteredObservableWithPaging<RecipeViewModel> RecipeVMList { get; private set; }

        public FilteredObservableWithPaging<RecipeViewModel> UsageVMList { get; private set; }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public virtual void InitAdditionalRecipes()
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public virtual void InitInformation()
        {
        }

        // TODO: Add Linktype enum.
        /// <summary>
        /// Add recipe View Model reference to current View Model.
        /// Recipe View Model describe how this entity can be acquired.
        /// </summary>
        /// <param name="recipeViewModel">View Model for recipe.</param>
        /// <param name="linkType">Link type (1 - inputItem, 2 - outputItem, 3 - station, 4 - techNode)</param>
        public virtual void AddRecipeLink([NotNull] RecipeViewModel recipeViewModel, byte linkType)
        {
            //Api.Logger.Warning(
            //    "CNEI: Trying to add recipe [" + recipeViewModel + "] link to mere entity " + ProtoEntity);
            switch (linkType)
            {
                case 1:
                    usageVMList.Add(recipeViewModel);
                    break;
                case 2:
                    recipeVMList.Add(recipeViewModel);
                    break;
                case 3:
                    usageVMList.Add(recipeViewModel);
                    break;
                case 4:
                    usageVMList.Add(recipeViewModel);
                    break;
                default:
                    Api.Logger.Error("CNEI: Wrong linkType " + linkType + " for " + recipeViewModel + " " + this);
                    break;
            }
        }

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public virtual void FinalizeRecipeLinking()
        {
            RecipeVMList = new FilteredObservableWithPaging<RecipeViewModel>(recipeVMList);
            UsageVMList = new FilteredObservableWithPaging<RecipeViewModel>(usageVMList);
        }

        private static object GetPropertyByName(object obj, string name)
        {
            return obj?.GetType().ScriptingGetProperty(name)?.GetValue(obj, null);
        }
    }
}