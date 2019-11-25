namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;

    public class ViewModelEntityInformation : BaseViewModel
    {
        /// <summary>
        /// Simple text information line "Header: text"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="text">text</param>
        public ViewModelEntityInformation(string header, string text)
        {
            Header = header;
            Text = text;
            TextVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Simple numeric information line "Header: number"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="i">number</param>
        public ViewModelEntityInformation(string header, int i) : this(header, i.ToString())
        {
        }

        /// <summary>
        /// Simple numeric information line "Header: number"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="d">number</param>
        public ViewModelEntityInformation(string header, double d)
            : this(header, d.ToString("0.###", CultureInfo.CurrentCulture))
        {
        }

        /// <summary>
        /// Simple formatted time information "Header: time"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="timeSpan">time</param>
        public ViewModelEntityInformation(string header, TimeSpan timeSpan)
            : this(header, ClientTimeFormatHelper.FormatTimeDuration(timeSpan))
        {
        }

        /// <summary>
        /// Information with list on entity "Header: entity list"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="collection">entity collection</param>
        public ViewModelEntityInformation(string header, List<ProtoEntityViewModel> collection)
        {
            HeaderVisibility = Visibility.Collapsed;
            InformationArray = new ArrayList() { header };
            InformationArray.AddRange(collection);
            InformationArrayVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Information with list on entity "Header: entity list"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="collection">entity collection</param>
        public ViewModelEntityInformation(string header, IEnumerable<ProtoEntityViewModel> collection)
            : this(header, collection.ToList())
        {
        }

        /// <summary>
        /// Information with single entity "Header: entity"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="protoEntityViewModel">entity</param>
        public ViewModelEntityInformation(string header, ProtoEntityViewModel protoEntityViewModel)
        {
            Header = header;
            Collection = new ObservableCollection<ProtoEntityViewModel>() { protoEntityViewModel };
            CollectionVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Information with status effect "Header: effect icon with intensity"
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="protoEntityViewModel">effect entity</param>
        /// <param name="intensity">intensity</param>
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