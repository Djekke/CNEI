namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Windows;

    public class ProtoEntityViewModel : BaseViewModel
    {
        private ITextureResource iconResource;

        private TextureBrush icon;

        private HashSet<RecipeViewModel> recipeVMList = new HashSet<RecipeViewModel>();

        private HashSet<RecipeViewModel> usageVMList = new HashSet<RecipeViewModel>();

        public virtual IProtoEntity ProtoEntity { get; }

        public virtual string ResourceDictonaryName => "ProtoEntityDataTemplate.xaml";

        public ProtoEntityViewModel([NotNull] IProtoEntity entity)
        {
            ProtoEntity = entity;
            Title = entity.Name;
            TitleLower = entity.Name.ToLower();
            Type = entity.Id;
            TypeLower = entity.Id.ToLower();
            RecipeVMList = new FilteredObservableWithPaging<RecipeViewModel>();
            UsageVMList = new FilteredObservableWithPaging<RecipeViewModel>();
            EntityInformation = new ObservableCollection<ViewModelEntityInformation>();
        }

        public ProtoEntityViewModel([NotNull] IProtoEntity entity, [NotNull] ITextureResource icon) : this(entity)
        {
            iconResource = icon;
        }

        public virtual ITextureResource IconResource
        {
            get
            {
                if (iconResource == null)
                {
                    iconResource = GetPropertyByName(ProtoEntity, "Icon") as ITextureResource;
                }
                return iconResource;
            }
        }

        public virtual TextureBrush Icon
        {
            get
            {
                if (icon == null)
                {
                    if (IconResource == null)
                    {
                        // Default icon.
                        iconResource = new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");
                    }
                    icon = Api.Client.UI.GetTextureBrush(IconResource);
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
            return obj?.GetType().GetProperty(name, BindingFlags.Instance |
                                                    BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.GetProperty)?.GetValue(obj, null);
        }
    }
}