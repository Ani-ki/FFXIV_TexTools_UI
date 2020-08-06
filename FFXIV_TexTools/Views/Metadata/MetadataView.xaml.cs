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
using xivModdingFramework.Items.Interfaces;

namespace FFXIV_TexTools.Views.Metadata
{
    /// <summary>
    /// Interaction logic for MetdataView.xaml
    /// </summary>
    public partial class MetadataView : UserControl
    {
        private MetadataViewModel _vm;

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
            return await _vm.SetRoot(item.GetRoot());
        }
    }
}
