namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ViewModelEntity : BaseViewModel
    {
        public ViewModelEntity()
        {
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
    }
}