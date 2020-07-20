namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ViewModelToolsPanel : BaseViewModel
    {
        public BaseCommand OpenFishingCalculator { get; }
        public ViewModelToolsPanel()
        {
            OpenFishingCalculator = new ActionCommand(FishingCalculatorWindow.Open);
        }
    }
}