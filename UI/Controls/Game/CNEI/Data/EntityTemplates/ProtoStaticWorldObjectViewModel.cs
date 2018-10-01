namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.World;

    public abstract class ProtoStaticWorldObjectViewModel : ProtoEntityViewModel
    {
        protected ProtoStaticWorldObjectViewModel(IProtoStaticWorldObject staticObject)
            : base(staticObject, staticObject.Icon ?? staticObject.DefaultTexture)
        {

        }
    }
}