namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using System.Windows;

    public class ViewModelItemWithCount : ProtoEntityViewModel
    {
        public ViewModelItemWithCount(IProtoItem item, ushort count) : base(item, item.Icon)
        {
            this.Count = count;
        }

        public ushort Count { get; private set; }

        public override Visibility CountVisibility => Visibility.Visible;
    }
}