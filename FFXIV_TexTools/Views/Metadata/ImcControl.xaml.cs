using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using xivModdingFramework.Helpers;
using xivModdingFramework.Mods.FileTypes;

namespace FFXIV_TexTools.Views.Metadata
{
    /// <summary>
    /// Interaction logic for ImcControl.xaml
    /// </summary>
    public partial class ImcControl : UserControl
    {
        private ItemMetadata _metadata;
        public ImcControl()
        {
            InitializeComponent();

            ImcVariantBox.SelectionChanged += ImcVariantBox_SelectionChanged;

            foreach (var cb in PartsGrid.Children)
            {
                var box = (CheckBox)cb;
                box.Checked += Box_Checked;
                box.Unchecked += Box_Checked;
            }
        }
        public async Task SetMetdata(ItemMetadata m)
        {
            _metadata = m;
            ImcVariantBox.Items.Clear();
            MaterialSetBox.Items.Clear();

            int maxMaterialSetId = 0;
            for (int i = 0; i < m.ImcEntries.Count; i++)
            {
                ImcVariantBox.Items.Add(i);
                if (m.ImcEntries[i].Variant > maxMaterialSetId)
                {
                    maxMaterialSetId = m.ImcEntries[i].Variant;
                }
            }

            for(int i = 1; i <= maxMaterialSetId; i++)
            {
                MaterialSetBox.Items.Add(i);
            }

            ImcVariantBox.SelectedItem = 0;
        }


        private void Box_Checked(object sender, RoutedEventArgs e)
        {
            var box = (CheckBox)sender;
            var part = box.Content.ToString().Trim().ToLower()[0];
            var partIndex = Array.IndexOf(Constants.Alphabet, part);

            ushort bit = (ushort)(1 << partIndex);
            var variant = (int)ImcVariantBox.SelectedItem;
            var on = box.IsChecked;

            // Update the mask as needed.
            if (on == true)
            {
                _metadata.ImcEntries[variant].Mask = (ushort)(_metadata.ImcEntries[variant].Mask | bit);
            } else
            {
                _metadata.ImcEntries[variant].Mask = (ushort)(_metadata.ImcEntries[variant].Mask & ~bit);
            }
        }

        private void ImcVariantBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_metadata == null) return;
            if (ImcVariantBox.SelectedItem == null) return;

            SetImcVariant((int)ImcVariantBox.SelectedItem);
        }

        public async Task SetImcVariant(int variant)
        {
            var entry = _metadata.ImcEntries[variant];
            foreach (var cb in PartsGrid.Children)
            {
                var box = (CheckBox)cb;
                var part = box.Content.ToString().Trim().ToLower()[0];
                var partIndex = Array.IndexOf(Constants.Alphabet, part);

                ushort bit = (ushort)(1 << partIndex);
                var active = (bit & entry.Mask) > 0;
                box.IsChecked = active;
            }

            SfxBox.Items.Clear();
            ushort sfx = (ushort)(entry.Mask >> 10);
            SfxBox.Items.Add(sfx);
            SfxBox.SelectedIndex = 0;

            VfxBox.Items.Clear();
            ushort vfx = entry.Vfx;
            VfxBox.Items.Add(vfx);
            VfxBox.SelectedIndex = 0;

            MaterialSetBox.SelectedIndex = entry.Variant -1;

        }
    }
}
