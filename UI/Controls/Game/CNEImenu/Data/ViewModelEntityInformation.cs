namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    public class ViewModelEntityInformation : BaseViewModel
    {
        public ViewModelEntityInformation(string header, string text)
        {
            Header = header;
            Text = text;
            TextVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, int i) : this(header, i.ToString())
        {
        }

        public ViewModelEntityInformation(string header, double d)
            : this(header, d.ToString("0.###", CultureInfo.CurrentCulture))
        {
        }

        public ViewModelEntityInformation(string header, TimeSpan timeSpan)
            : this(header, ClientTimeFormatHelper.FormatTimeDuration(timeSpan))
        {
        }

        public ViewModelEntityInformation(string header, List<ProtoEntityViewModel> collection)
        {
            HeaderVisibility = Visibility.Collapsed;
            InformationArray = new ArrayList() { header };
            InformationArray.AddRange(collection);
            InformationArrayVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, IEnumerable<ProtoEntityViewModel> collection)
            : this(header, collection.ToList())
        {
        }

        public ViewModelEntityInformation(string header, ProtoEntityViewModel protoEntityViewModel)
        {
            Header = header;
            Collection = new ObservableCollection<ProtoEntityViewModel>() { protoEntityViewModel };
            CollectionVisibility = Visibility.Visible;
        }

        public ViewModelEntityInformation(string header, ProtoEntityViewModel protoEntityViewModel, double intensity)
        {
            if (protoEntityViewModel is ProtoStatusEffectViewModel protoStatusEffectViewModel)
            {
                Header = header;
                StatusEffect = protoStatusEffectViewModel;
                StatusEffectVisibility = Visibility.Visible;
                ToolTipIntensityPercent = intensity * 100;
                StatusEffectBackground = protoStatusEffectViewModel.GetBackgroundBrush(intensity);
            }
            else
            {
                Api.Logger.Error("CNEI: Wrong status effect in entity information " + protoEntityViewModel.ProtoEntity);
            }
        }

        public ArrayList InformationArray { get; }

        public Visibility InformationArrayVisibility { get; } = Visibility.Collapsed;

        public string Header { get; }

        public Visibility HeaderVisibility { get; } = Visibility.Visible;

        public string Text { get; }

        public Visibility TextVisibility { get; } = Visibility.Collapsed;

        public ObservableCollection<ProtoEntityViewModel> Collection { get; }

        public Visibility CollectionVisibility { get; } = Visibility.Collapsed;

        public ProtoStatusEffectViewModel StatusEffect { get; }

        public Visibility StatusEffectVisibility { get; } = Visibility.Collapsed;

        public double ToolTipIntensityPercent { get; }

        public Brush StatusEffectBackground { get; }
    }
}