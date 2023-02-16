using PluginDemo.Common.Windows.Eventing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginDemo.Common.Windows
{
    public class DirectoryChangedWatcherService : IDirectoryChangedWatcherService
    {
        //TODO: move to common libs
        #region Properties

        private FileSystemWatcher fileSystemWatcher;

        #endregion Properties

        #region Construction

        public DirectoryChangedWatcherService(string Path)
        { 
            this.fileSystemWatcher = new FileSystemWatcher();
            this.SetPath(Path);  //path needs to be set before the rest can happen
            this.InitializeWatcher();
            this.AddEvents();
        }

        #endregion Construction

        #region Methods

        #region InitializeWatcher
        private void InitializeWatcher()
        {
            this.fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes |
                                                    NotifyFilters.CreationTime |
                                                    NotifyFilters.DirectoryName |
                                                    NotifyFilters.FileName |
                                                    NotifyFilters.LastAccess |
                                                    NotifyFilters.LastWrite |
                                                    NotifyFilters.Security |
                                                    NotifyFilters.Size;  
            this.fileSystemWatcher.Filter = "*.*";  // filter set to any
        }
        #endregion InitializeWatcher

        #region AddEvents
        private void AddEvents()
        {
            this.fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnChanged);
            this.fileSystemWatcher.Created += new FileSystemEventHandler(this.OnChanged);
            this.fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnChanged);
            this.fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnRenamed);
            this.fileSystemWatcher.EnableRaisingEvents = true;
        }
        #endregion AddEvents

        #region SetPath
        public void SetPath(string Path, bool DoIncludeSubdirectories = true)
        {
            this.fileSystemWatcher.Path = Path;
            this.fileSystemWatcher.IncludeSubdirectories = DoIncludeSubdirectories;
        }
        #endregion SetPath

        #region OnRenamed
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string filenameBefore = e.OldFullPath;
            string filenameAfter = e.FullPath;
            bool isChanged = false;
            bool isCreated = false;
            bool isDeleted = false;
            bool isRenamed = false;

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.All:
                    isChanged = true;
                    break;
                case WatcherChangeTypes.Changed:
                    isChanged = true;
                    break;
                case WatcherChangeTypes.Created:
                    filenameBefore = string.Empty;
                    isCreated = true;
                    break;
                case WatcherChangeTypes.Deleted:
                    filenameAfter = string.Empty;
                    isDeleted = true;
                    break;
                case WatcherChangeTypes.Renamed:
                    isRenamed = true;
                    break;
                default:
                    break;
            }

            this.ContentChanged?.Invoke(this, new DirectoryChangedEventArgs(filenameBefore, filenameAfter, isChanged, isCreated, isDeleted, isRenamed));
        }
        #endregion OnRenamed

        #region OnChanged
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filenameBefore = e.FullPath;
            string filenameAfter = e.FullPath;
            bool isChanged = false;
            bool isCreated = false;
            bool isDeleted = false;
            bool isRenamed = false; 
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.All:
                    isChanged = true;
                    break;
                case WatcherChangeTypes.Changed:
                    isChanged = true;
                    break;
                case WatcherChangeTypes.Created:
                    filenameBefore = string.Empty;
                    isCreated = true;
                    break;
                case WatcherChangeTypes.Deleted:
                    filenameAfter = string.Empty;
                    isDeleted = true;
                    break;
                case WatcherChangeTypes.Renamed:
                    isRenamed = true;
                    break;
                default:
                    break;
            }

            this.ContentChanged?.Invoke(this, new DirectoryChangedEventArgs(filenameBefore, filenameAfter, isChanged, isCreated, isDeleted, isRenamed));
        }
        #endregion OnChanged

        #endregion Methods

        #region Events

        public event EventHandler<DirectoryChangedEventArgs> ContentChanged;

        #endregion Events
    }
}
