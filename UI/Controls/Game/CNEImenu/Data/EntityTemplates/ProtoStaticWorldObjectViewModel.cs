namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
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