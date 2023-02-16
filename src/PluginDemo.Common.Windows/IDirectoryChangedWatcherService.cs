using PluginDemo.Common.Windows.Eventing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Common.Windows
{
    public interface IDirectoryChangedWatcherService
    {
        //TODO: move to different interface project
        //TODO: move to common libs
        #region Properties

        #endregion Properties

        #region Methods

        //void SetPath(string Path, bool DoIncludeSubdirectories = true);  //TODO: see if this stays private, more effort needed (unhang events etc

        #endregion Methods

        #region Events

        event EventHandler<DirectoryChangedEventArgs> ContentChanged;

        #endregion Events
    }
}
