namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;

    public class ViewModelEntityWithCount : BaseViewModel
    {
        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel)
        {
            EntityViewModel = entityViewModel;
            CountString = "";
        }

        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel, string text) : this(entityViewModel)
        {
            CountString = text;
        }

        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel, int count) : this(entityViewModel)
        {
            CountString += count + " x";
        }

        public ViewModelEntityWithCount(ProtoEntityViewModel entityViewModel, int count, int countRandom,
            double probability) : this(entityViewModel, count)
        {
            if (countRandom > 0)
            {
                CountString = "(" + count + "-" + (count + countRandom) + ") x";
            }

            if (0 < probability && probability <= 1)
            {
                CountString += " " + (probability * 100) + "%";
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