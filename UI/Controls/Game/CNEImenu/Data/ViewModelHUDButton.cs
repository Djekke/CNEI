namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using CryoFall.CNEI.ClientComponents.Input;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Controls;

    public class ViewModelHUDButton: BaseViewModel
    {
        public Menu MenuCNEI { get; }

        public IButtonReference ButtonReference { get; set; }

        public ViewModelHUDButton()
        {
            MenuCNEI = Menu.Register<WindowCNEImenu>();
            ButtonReference = new CNEIbuttonReference() { Button = CNEIbutton.MenuOpen };
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            MenuCNEI?.Dispose();
            ButtonReference = null;
        }
    }
}