using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using TaskManagement.DAL.Models;
using Telerik.Windows.Controls;

namespace TaskManagement.MVVM
{
    [ContentProperty("ContextMenu")]
    public class RadContextMenuXamlHolder : INotifyPropertyChanged
    {
        private RadContextMenu _contextMenu;
        public event PropertyChangedEventHandler PropertyChanged;

        public RadContextMenu ContextMenu
        {
            get
            {
                return this._contextMenu;
            }
            set
            {
                if (this._contextMenu != value)
                {
                    this._contextMenu = value;
                    this.RaisePropertyChanged("ContextMenu");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
