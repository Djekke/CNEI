namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.GameApi.Resources;

    public class ViewModelStructure : ViewModelEntity
    {
        public readonly IProtoObjectStructure Structure;

        public ViewModelStructure(IProtoObjectStructure building)
        {
            this.Structure = building;
        }

        public override ITextureResource IconResource => this.Structure.Icon;

        public override string Title => this.Structure.Name;

        public override string ToString()
        {
            return this.Structure?.ToString() ?? string.Empty;
        }
    }
}