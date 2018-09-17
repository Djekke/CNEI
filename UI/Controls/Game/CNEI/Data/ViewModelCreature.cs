namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;

    public class ViewModelCreature : ViewModelEntity
    {
        public readonly IProtoCharacterMob Creature;

        public ViewModelCreature(IProtoCharacterMob creature)
        {
            this.Creature = creature;
        }

        // Prewiew from skeleton?
        //public override TextureBrush Icon => Api.Client.UI.GetTextureBrush(this.Creature);

        public override string Title => this.Creature.Name;

        public override string ToString()
        {
            return this.Creature?.ToString() ?? string.Empty;
        }
    }
}