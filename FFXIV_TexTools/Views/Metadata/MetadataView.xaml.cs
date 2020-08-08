using FFXIV_TexTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xivModdingFramework.Cache;
using xivModdingFramework.Items.Enums;
using xivModdingFramework.Items.Interfaces;
using xivModdingFramework.Models.FileTypes;

namespace FFXIV_TexTools.Views.Metadata
{
    /// <summary>
    /// Interaction logic for MetdataView.xaml
    /// </summary>
    public partial class MetadataView : UserControl
    {
        private MetadataViewModel _vm;
        private XivDependencyRoot _root;

        public MetadataView()
        {
            _vm = new MetadataViewModel(this);
            DataContext = _vm;
            InitializeComponent();
        }

        /// <summary>
        /// Sets the view to the given item. 
        /// Returns False if the view should be hidden.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> SetItem(IItem item)
        {
            return await SetRoot(item.GetRoot());
        }
        public async Task<bool> SetRoot(XivDependencyRoot root)
        {
            _root = root;
            if (_root == null) return false;
            
            SetLabel.Content = XivItemTypes.GetSystemPrefix(root.Info.PrimaryType) + root.Info.PrimaryId.ToString().PadLeft(4, '0');
            SlotLabel.Content = Mdl.SlotAbbreviationDictionary.FirstOrDefault(x => x.Value == _root.Info.Slot).Key + "(" + _root.Info.Slot + ")";

            var items = await _root.GetAllItems();
            ItemNameBox.Text  = "[" + items.Count + "] " + items[0].Name;

            return await _vm.SetRoot(_root);
        }

        private void PreviousSlotButton_Click(object sender, RoutedEventArgs e)
        {
            var type = _root.Info.PrimaryType;
            if (type == XivItemType.demihuman)
            {
                type = (XivItemType)_root.Info.SecondaryType;
            }

            var slots = XivItemTypes.GetAvailableSlots(type);

            var currentSlotIdx = Array.IndexOf(slots.ToArray(), _root.Info.Slot);
            var nextSlotIdx = currentSlotIdx - 1;
            if (nextSlotIdx < 0)
            {
                nextSlotIdx = slots.Count - 1;
            }

            var nextSlot = slots[nextSlotIdx];

            var newRootInfo = (XivDependencyRootInfo)_root.Info.Clone();
            newRootInfo.Slot = nextSlot;

            var newRoot = newRootInfo.ToFullRoot();

            if (newRoot == null)
            {
                // Shouldn't ever actually hit this, but if we do, cancel the process.
                return;
            }

            SetRoot(newRoot);
        }

        private void NexSlotButton_Click(object sender, RoutedEventArgs e)
        {
            var type = _root.Info.PrimaryType;
            if (type == XivItemType.demihuman)
            {
                type = (XivItemType)_root.Info.SecondaryType;
            }

            var slots = XivItemTypes.GetAvailableSlots(type);

            var currentSlotIdx = Array.IndexOf(slots.ToArray(), _root.Info.Slot);
            var nextSlotIdx = currentSlotIdx + 1;
            if (nextSlotIdx == slots.Count)
            {
                nextSlotIdx = 0;
            }

            var nextSlot = slots[nextSlotIdx];

            var newRootInfo = (XivDependencyRootInfo) _root.Info.Clone();
            newRootInfo.Slot = nextSlot;

            var newRoot = newRootInfo.ToFullRoot();

            if(newRoot == null)
            {
                // Shouldn't ever actually hit this, but if we do, cancel the process.
                return;
            }

            SetRoot(newRoot);
        }

        private void AffectedItemsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
