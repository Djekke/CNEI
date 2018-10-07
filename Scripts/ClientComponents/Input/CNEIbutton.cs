namespace CryoFall.CNEI.ClientComponents.Input
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.GameApi;
    using AtomicTorch.CBND.GameApi.ServicesClient;

    [NotPersistent]
    public enum CNEIbutton
    {
        [ButtonInfo(
            "Open CNEI",
            "Open CryoFall NEI",
            InputKey.N,
            Category = "CNEI")]
        MenuOpen,

        [ButtonInfo(
            "Show item info",
            "Open CryoFall NEI Details window with information about item under mouse coursor",
            InputKey.I,
            Category = "CNEI")]
        MenuDetails,

        [ButtonInfo(
            "Previous info",
            "Show previous information in Details window or close it",
            InputKey.Back,
            Category = "CNEI")]
        MenuBack,
    }
}