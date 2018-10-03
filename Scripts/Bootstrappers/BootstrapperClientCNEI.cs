namespace CryoFall.CNEI.Bootstrappers
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;


    public class BootstrapperClientCNEI: BaseBootstrapper
    {
        public override void ClientInitialize()
        {
            EntityViewModelsManager.Init();

            ClientInputContext.Start("CNEI menu overlay")
                              .HandleButtonDown(
                                  GameButton.CNEIMenu,
                                  Menu.Toggle<WindowCNEIMenu>);
        }
    }
}