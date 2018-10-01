namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;

    public class ViewModelEntityWithCount : BaseViewModel
    {
        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel)
        {
            this.EntityViewModel = entityViewModel;
            this.CountString = "";
        }

        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel, int count) : this(entityViewModel)
        {
            this.CountString += count + " x";
        }

        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel, int count, int countRandom,
            double probability) : this(entityViewModel, count)
        {
            if (countRandom > 0)
            {
                this.CountString = "(" + count + "-" + (count + countRandom) + ") x";
            }

            if (0 < probability && probability <= 1)
            {
                this.CountString += " " + (probability * 100) + "%";
            }
            else
            {
                Api.Logger.Warning("CNEI: Wrong probability (" + probability + ") for " + entityViewModel);
            }
        }

        public string CountString { get; }

        public ProtoEntityViewModel EntityViewModel { get; }

    }
}