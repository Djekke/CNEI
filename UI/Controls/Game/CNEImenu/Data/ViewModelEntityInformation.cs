namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    public class ViewModelEntityInformation : BaseViewModel
    {
        public ViewModelEntityInformation(string header, string text)
        {
            Text = text;
            Header = header;
            TextVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, int i) : this (header, i.ToString()) {}
        public ViewModelEntityInformation(string header, double d) : this (header, d.ToString()) {}

        public ViewModelEntityInformation(string header, IEnumerable<ProtoEntityViewModel> collection)
        {
            Header = header;
            Collection = new ObservableCollection<ProtoEntityViewModel>(collection);
            CollectionVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, List<ProtoEntityViewModel> collection)
        {
            Header = header;
            Collection = new ObservableCollection<ProtoEntityViewModel>(collection);
            CollectionVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, ProtoEntityViewModel protoEntityViewModel)
        {
            Header = header;
            Collection = new ObservableCollection<ProtoEntityViewModel>() { protoEntityViewModel };
            CollectionVisibility = Visibility.Visible;
        }

        public string Header { get; }

        public string Text { get; }

        public Visibility TextVisibility { get; } = Visibility.Collapsed;

        public ObservableCollection<ProtoEntityViewModel> Collection { get; }

        public Visibility CollectionVisibility { get; } = Visibility.Collapsed;
    }
}