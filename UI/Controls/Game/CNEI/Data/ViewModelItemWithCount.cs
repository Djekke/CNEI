namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using System.Windows;

    public class ViewModelItemWithCount : ProtoEntityViewModel
    {
        public ViewModelItemWithCount(IProtoItem item, ushort count, ushort countRandom = 0, double probability = 0) : base(item, item.Icon)
        {
            this.Count = count;
            this.CountRandom = countRandom;
            this.Probability = probability * 100;
        }

        public ushort Count { get; private set; }

        public ushort CountRandom { get; private set; }

        public double Probability { get; private set; }

        public override Visibility CountVisibility => Visibility.Visible;
    }
}