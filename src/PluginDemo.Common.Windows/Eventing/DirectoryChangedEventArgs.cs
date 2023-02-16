using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Common.Windows.Eventing
{
    public class DirectoryChangedEventArgs : EventArgs
    {
        #region Properties

        public string FilenameBefore;
        public string FilenameAfter;
        public bool IsChanged;
        public bool IsCreated;
        public bool IsDeleted;
        public bool IsRenamed;

        #endregion Properties

        #region Construction

        public DirectoryChangedEventArgs(string FilenameBefore, string FilenameAfter, bool IsChanged, bool IsCreated, bool IsDeleted, bool IsRenamed)
        { 
            this.FilenameBefore = FilenameBefore;
            this.FilenameAfter = FilenameAfter;
            this.IsChanged = IsChanged;
            this.IsCreated = IsCreated;
            this.IsDeleted = IsDeleted;
            this.IsRenamed = IsRenamed;
        }

        #endregion Construction

        #region Methods

        #endregion Methods
    }
}
