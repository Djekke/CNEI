namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Resources;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public ProtoItemViewModel(IProtoItem item) : base(item, item.Icon)
        {
            this.Description = item.Description;
        }

        public string Description { get; }
    }
}