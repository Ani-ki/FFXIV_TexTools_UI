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
using xivModdingFramework.Models.DataContainers;
using xivModdingFramework.Mods.FileTypes;

namespace FFXIV_TexTools.Views.Metadata
{
    /// <summary>
    /// Interaction logic for EqpView.xaml
    /// </summary>
    public partial class EqpControl : UserControl
    {
        private ItemMetadata _metadata;
        private EquipmentParameter entry;
        public EqpControl()
        {
            InitializeComponent();
        }

        public async Task SetMetdata(ItemMetadata m)
        {
            _metadata = m;

            entry = _metadata.EqpEntry;

            RawGrid.Children.Clear();
            if (entry == null) return;

            var flags = entry.GetFlags();

            var idx = 0;
            foreach(var flag in flags)
            {
                var cb = new CheckBox();
                cb.Content = flag.Key.ToString();
                cb.DataContext = flag.Key;
                cb.IsChecked = flag.Value;

                cb.SetValue(Grid.RowProperty, idx / 4);
                cb.SetValue(Grid.ColumnProperty, idx % 4);

                RawGrid.Children.Add(cb);
                idx++;
            }




        }
    }
}
