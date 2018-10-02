namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using JetBrains.Annotations;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public ProtoItemViewModel([NotNull] IProtoItem item) : base(item, item.Icon)
        {
            this.Description = item.Description;
            this.IsStackable = item.IsStackable;
            this.MaxItemsPerStack = item.MaxItemsPerStack;
        }

        public string Description { get; }

        public bool IsStackable { get; }

        public ushort MaxItemsPerStack { get; }
    }
}