namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.Systems.Droplists;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource defaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        public ProtoCharacterMobViewModel(IProtoCharacterMob creature) : base(creature, defaultIcon)
        {
            //creature.LootDroplist
            //droplist.Outputs
            //droplist.OutputsRandom
        }

        // TODO: Prewiew from skeleton
        //public override TextureBrush Icon
    }
}