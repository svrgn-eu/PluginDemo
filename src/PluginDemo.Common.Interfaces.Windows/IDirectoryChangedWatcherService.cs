using PluginDemo.Common.Interfaces.Windows.Eventing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Common.Interfaces.Windows
{
    public interface IDirectoryChangedWatcherService
    {
        //TODO: move to different interface project
        //TODO: move to common libs
        #region Properties

        #endregion Properties

        #region Methods

        void SetPath(string Path, bool DoIncludeSubdirectories = true);

        #endregion Methods

        #region Events

        event EventHandler<DirectoryChangedEventArgs> ContentChanged;

        #endregion Events
    }
}
