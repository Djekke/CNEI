namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using System.Reflection;

    public class ViewModelEntity : BaseViewModel
    {
        public virtual IProtoEntity ProtoEntity { get; private set; }

        public ViewModelEntity()
        {
        }

        public ViewModelEntity(IProtoEntity entity)
        {
            this.ProtoEntity = entity;
            this.Title = entity.Name;
            this.IconResource = GetPropertyByName(entity, "Icon") as ITextureResource;
            this.Type = entity.Id;
        }

        public virtual ITextureResource IconResource { get; }

        public virtual TextureBrush Icon
        {
            get
            {
                ITextureResource icon = new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");
                if (this.IconResource != null)
                {
                    icon = this.IconResource;
                }
                return Api.Client.UI.GetTextureBrush(icon);
            }
        }

        public virtual string Title { get; }

        public virtual string Type { get; }

        private object GetPropertyByName(object obj, string name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.Instance |
                                                   BindingFlags.Public |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.GetProperty)?.GetValue(obj, null);
        }
    }
}