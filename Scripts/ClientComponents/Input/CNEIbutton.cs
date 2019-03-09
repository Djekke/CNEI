namespace CryoFall.CNEI.ClientComponents.Input
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.GameApi;
    using AtomicTorch.CBND.GameApi.ServicesClient;
    using System.ComponentModel;

    [NotPersistent]
    public enum CNEIbutton
    {
        [Description("Open CNEI")]
        [ButtonInfo(InputKey.N, Category = "CNEI")]
        MenuOpen,

        [Description("Show item info")]
        [ButtonInfo(InputKey.I, Category = "CNEI")]
        MenuDetails,

        [Description("Previous info")]
        [ButtonInfo(InputKey.Back, Category = "CNEI")]
        MenuBack,
    }
}