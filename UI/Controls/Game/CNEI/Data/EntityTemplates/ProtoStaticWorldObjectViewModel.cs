namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.World;
    using JetBrains.Annotations;

    public abstract class ProtoStaticWorldObjectViewModel : ProtoEntityViewModel
    {
        protected ProtoStaticWorldObjectViewModel([NotNull] IProtoStaticWorldObject staticObject)
            : base(staticObject, staticObject.Icon ?? staticObject.DefaultTexture)
        {

        }
    }
}