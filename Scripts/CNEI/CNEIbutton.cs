namespace CryoFall.CNEI
{
    using System.ComponentModel;
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.GameApi;
    using AtomicTorch.CBND.GameApi.ServicesClient;

    [NotPersistent]
    public enum CNEIButton
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