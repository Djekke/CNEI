namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;

    public class ViewModelWindowCNEIDetails : BaseViewModel
    {
        public List<ProtoEntityViewModel> EntityList { get; set; }

        private ProtoEntityViewModel selectedEntity;

        public ProtoEntityViewModel SelectedEntityViewModel
        {
            get => this.selectedEntity;
            set
            {
                if (this.selectedEntity == value)
                {
                    return;
                }
                this.selectedEntity = value;
                this.NotifyThisPropertyChanged();
            }
        }

        public int TotalWindowsCount => EntityList?.Count ?? 0;

        public int CurrentWindowNumber => (EntityList?.FindIndex(s => s == SelectedEntityViewModel) ?? -1) + 1;

        public ViewModelWindowCNEIDetails()
        {

        }

        public ViewModelWindowCNEIDetails(ProtoEntityViewModel entityViewModel)
        {
            this.SelectedEntityViewModel = entityViewModel;
        }
    }
}