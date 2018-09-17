namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Resources;

    public class ViewModelItem : ViewModelEntity
    {
        public readonly IProtoItem Item;

        public ViewModelItem(IProtoItem item)
        {
            this.Item = item;
        }

        public override ITextureResource IconResource => this.Item.Icon;
        
        public override string Title => this.Item.Name;

        public override string ToString()
        {
            return this.Item?.ToString() ?? string.Empty;
        }
    }
}