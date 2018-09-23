namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        public ProtoCharacterMobViewModel(IProtoCharacterMob creature) : base(creature)
        {
        }

        // Prewiew from skeleton?
        //public override TextureBrush Icon => Api.Client.UI.GetTextureBrush(this.Creature);
    }
}