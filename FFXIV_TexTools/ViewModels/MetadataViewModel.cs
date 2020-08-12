using FFXIV_TexTools.Views.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xivModdingFramework.Cache;
using xivModdingFramework.Mods.FileTypes;

namespace FFXIV_TexTools.ViewModels
{

    public class MetadataViewModel
    {
        private MetadataView _view;
        private ItemMetadata _metadata;
        public MetadataViewModel(MetadataView view)
        {
            _view = view;
        }


        /// <summary>
        /// Sets the given dependency root for display.
        /// Returns false if the root or metadata is invalid.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public async Task<bool> SetRoot(XivDependencyRoot root)
        {
            _metadata = await ItemMetadata.GetMetadata(root);
            if (_metadata == null) return false;

            if (_metadata.ImcEntries.Count > 0)
            {
                _view.ImcView.Visibility = System.Windows.Visibility.Visible;
                await _view.ImcView.SetMetdata(_metadata);
            } else
            {
                _view.ImcView.Visibility = System.Windows.Visibility.Collapsed;
            }

            if(_metadata.EqpEntry != null)
            {
                _view.EqpView.Visibility = System.Windows.Visibility.Visible;
                await _view.EqpView.SetMetdata(_metadata);
            }
            else
            {
                _view.EqpView.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (_metadata.EqdpEntries.Count > 0)
            {
                _view.EqdpView.Visibility = System.Windows.Visibility.Visible;
                await _view.EqdpView.SetMetdata(_metadata);
            }
            else
            {
                _view.EqdpView.Visibility = System.Windows.Visibility.Collapsed;
            }

            return (_metadata != null);
        }

        public async Task<bool> Save()
        {
            if (_metadata == null) return false;

            await ItemMetadata.ApplyMetadata(_metadata);
            return true;
        }
    }
}
