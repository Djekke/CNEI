namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using System.Windows;

    public class ViewModelOutputItem : ViewModelItemWithCount
    {
        public ViewModelOutputItem(IProtoItem item, ushort count, ushort countRandom = 0, double probability = 1) : base(item, count)
        {
            if (countRandom > 0)
            {
                this.CountString = "(" + count + "-" + (ushort)(countRandom + count) + ") * " + (probability * 100) + "%"; 
            }
            else
            {
                this.CountString = count + " * " + (probability * 100) + "%";
            }
        }

        public string CountString { get; private set; }

        public override Visibility CountVisibility => Visibility.Collapsed;
    }
}