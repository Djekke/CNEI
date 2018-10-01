namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using System.Reflection;
    using System.Windows;

    public class ProtoEntityViewModel : BaseViewModel
    {
        private ITextureResource iconResource;

        private TextureBrush icon;

        public virtual IProtoEntity ProtoEntity { get; }

        public ProtoEntityViewModel(IProtoEntity entity)
        {
            this.ProtoEntity = entity;
            this.Title = entity.Name;
            this.Type = entity.Id;
        }

        public ProtoEntityViewModel(IProtoEntity entity, ITextureResource icon) : this(entity)
        {
            this.iconResource = icon;
        }

        public virtual ITextureResource IconResource
        {
            get
            {
                if (iconResource == null)
                {
                    this.iconResource = GetPropertyByName(this.ProtoEntity, "Icon") as ITextureResource;
                }
                return this.iconResource;
            }
        }

        public virtual TextureBrush Icon
        {
            get
            {
                if (this.icon == null)
                {
                    if (this.IconResource == null)
                    {
                        // Default icon.
                        this.iconResource = new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");
                    }
                    this.icon = Api.Client.UI.GetTextureBrush(this.IconResource);
                }
                return this.icon;
            }
        }

        public virtual string Title { get; }

        public virtual string Type { get; }

        public virtual Visibility Visibility => Visibility.Visible;

        public virtual Visibility CountVisibility => Visibility.Collapsed;

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" /> and
        /// <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public virtual void InitEntityRelationships()
        {
        }

        /// <summary>
        /// Add recipe View Model reference to current View Model.
        /// Recipe View Model describe how this entity can be acquired.
        /// </summary>
        /// <param name="recipeViewModel">View Model for recipe.</param>
        public virtual void AddRecipeLink(RecipeViewModel recipeViewModel)
        {
            Api.Logger.Warning(
                "CNEI: Trying to add recipe [" + recipeViewModel + "] link to mere entity " + ProtoEntity);
        }

        /// <summary>
        /// Add recipe View Model reference to current View Model.
        /// Recipe View Model describe there current entity can be used.
        /// </summary>
        /// <param name="recipeViewModel">View Model for recipe.</param>
        public virtual void AddUsesLink(RecipeViewModel recipeViewModel)
        {
            Api.Logger.Warning(
                "CNEI: Trying to add uses [" + recipeViewModel + "] link to mere entity " + ProtoEntity);
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