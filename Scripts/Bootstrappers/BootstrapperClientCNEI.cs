namespace AtomicTorch.CBND.CNEI.Bootstrappers
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI;
    using AtomicTorch.CBND.GameApi.Scripting;

    public class BootstrapperClientCNEI: BaseBootstrapper
    {
        public override void ClientInitialize()
        {
            ClientInputContext.Start("CNEI menu overlay")
                              .HandleButtonDown(
                                  GameButton.CNEIMenu,
                                  Menu.Toggle<WindowCNEIMenu>);
        }
    }
}