using PluginDemo.Common.Interfaces.Windows;
using PluginDemo.Common.Interfaces.Windows.Eventing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginDemo.Common.Implementations.Windows
{
    /// <summary>
    /// File system watcher encapsulation. After Creation, you need to set the watched path with SetPath. It is recommended to have one instance of this class per watched directory.
    /// </summary>
    public class DirectoryChangedWatcherService : IDirectoryChangedWatcherService
    {
        //TODO: move to common libs
        #region Properties

        private FileSystemWatcher fileSystemWatcher;  // the main protagonist of our class

        private FileSystemEventHandler fileSystemEventHandlerChanged;  //event var to be able to unhang it
        private RenamedEventHandler fileSystemEventHandlerRenamed;  //event var to be able to unhang it

        #endregion Properties

        #region Construction

        public DirectoryChangedWatcherService()
        {
            fileSystemWatcher = new FileSystemWatcher();
            InitializeWatcher();
            CreateEventHandlers();
        }

        #endregion Construction

        #region Methods

        #region InitializeWatcher
        private void InitializeWatcher()
        {
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes |
                                                    NotifyFilters.CreationTime |
                                                    NotifyFilters.DirectoryName |
                                                    NotifyFilters.FileName |
                                                    NotifyFilters.LastAccess |
                                                    NotifyFilters.LastWrite |
                                                    NotifyFilters.Security |
                                                    NotifyFilters.Size;
            fileSystemWatcher.Filter = "*.*";  // filter set to any
        }
        #endregion InitializeWatcher

        #region CreateEventHandlers
        private void CreateEventHandlers()
        {
            fileSystemEventHandlerChanged = new FileSystemEventHandler(OnChanged);
            fileSystemEventHandlerRenamed = new RenamedEventHandler(OnRenamed);
        }
        #endregion CreateEventHandlers

        #region AddEvents
        private void AddEvents()
        {
            fileSystemWatcher.Changed += fileSystemEventHandlerChanged;
            fileSystemWatcher.Created += fileSystemEventHandlerChanged;
            fileSystemWatcher.Deleted += fileSystemEventHandlerChanged;
            fileSystemWatcher.Renamed += fileSystemEventHandlerRenamed;
            fileSystemWatcher.EnableRaisingEvents = true;
        }
        #endregion AddEvents

        #region RemoveEvents
        private void RemoveEvents()
        {
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Changed -= fileSystemEventHandlerChanged;
            fileSystemWatcher.Created -= fileSystemEventHandlerChanged;
            fileSystemWatcher.Deleted -= fileSystemEventHandlerChanged;
            fileSystemWatcher.Renamed -= fileSystemEventHandlerRenamed;
        }
        #endregion RemoveEvents

        #region SetPath
        public void SetPath(string Path, bool DoIncludeSubdirectories = true)
        {
            RemoveEvents();
            fileSystemWatcher.Path = Path;
            fileSystemWatcher.IncludeSubdirectories = DoIncludeSubdirectories;
            AddEvents();
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

            ContentChanged?.Invoke(this, new DirectoryChangedEventArgs(filenameBefore, filenameAfter, isChanged, isCreated, isDeleted, isRenamed));
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

            ContentChanged?.Invoke(this, new DirectoryChangedEventArgs(filenameBefore, filenameAfter, isChanged, isCreated, isDeleted, isRenamed));
        }
        #endregion OnChanged

        #endregion Methods

        #region Events

        public event EventHandler<DirectoryChangedEventArgs> ContentChanged;

        #endregion Events
    }
}
