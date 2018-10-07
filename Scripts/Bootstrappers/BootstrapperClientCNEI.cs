namespace CryoFall.CNEI.Bootstrappers
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Controls;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.ClientComponents.Input;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Controls;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;


    public class BootstrapperClientCNEI: BaseBootstrapper
    {
        public override void ClientInitialize()
        {
            ClientInputManager.RegisterButtonsEnum<CNEIbutton>();

            EntityViewModelsManager.Init();

            ClientInputContext.Start("CNEI menu overlay")
                              .HandleButtonDown(
                                  CNEIbutton.MenuOpen,
                                  Menu.Toggle<WindowCNEImenu>);

            ClientInputContext.Start("CNEI context info")
                .HandleButtonDown(
                    CNEIbutton.MenuDetails,
                    () =>
                    {
                        var hitTestResult = Api.Client.UI.GetVisualInPointedPosition();
                        if (hitTestResult == null)
                        {
                            return;
                        }

                        if (VisualTreeHelperExtension.FindParentOfType(
                            hitTestResult, typeof(ItemSlotControl)) is ItemSlotControl itemSlotControl)
                        {
                            if (itemSlotControl.Item != null)
                            {
                                WindowCNEIdetails.Open(
                                    EntityViewModelsManager.GetEntityViewModel(itemSlotControl.Item.ProtoItem));
                            }
                            return;
                        }

                        if (VisualTreeHelperExtension.FindParentOfType(
                            hitTestResult, typeof(RequiredItemControl)) is RequiredItemControl requiredItemControl)
                        {
                            if (requiredItemControl.ProtoItemWithCount != null)
                            {
                                WindowCNEIdetails.Open(
                                    EntityViewModelsManager.GetEntityViewModel(requiredItemControl.ProtoItemWithCount.ProtoItem));
                            }
                            return;
                        }

                        if (VisualTreeHelperExtension.FindParentOfType(
                            hitTestResult, typeof(EntitySlotControl)) is EntitySlotControl entitySlotControl)
                        {
                            if (entitySlotControl.DataContext is ProtoEntityViewModel entityViewModel)
                            {
                                WindowCNEIdetails.Open(entityViewModel);
                            }
                            else
                            {
                                Api.Logger.Error("CNEI: Wrong view model for details window " + entitySlotControl.DataContext);
                            }
                        }
                    });
        }
    }
}