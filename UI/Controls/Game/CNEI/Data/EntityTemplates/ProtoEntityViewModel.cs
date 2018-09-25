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
        public virtual IProtoEntity ProtoEntity { get; private set; }

        public ProtoEntityViewModel(IProtoEntity entity)
        {
            this.ProtoEntity = entity;
            this.Title = entity.Name;
            this.IconResource = GetPropertyByName(entity, "Icon") as ITextureResource;
            this.Type = entity.Id;
        }

        public virtual ITextureResource IconResource { get; private set; }

        public virtual TextureBrush Icon
        {
            get
            {
                if (this.IconResource == null)
                {
                    this.IconResource = new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");
                }
                return Api.Client.UI.GetTextureBrush(this.IconResource);
            }
        }

        public virtual string Title { get; }

        public virtual string Type { get; }

        public virtual Visibility Visibility => Visibility.Visible;

        public virtual Visibility CountVisibility => Visibility.Collapsed;

        public virtual int Count => 0;

        private object GetPropertyByName(object obj, string name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.Instance |
                                                   BindingFlags.Public |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.GetProperty)?.GetValue(obj, null);
        }
    }
}