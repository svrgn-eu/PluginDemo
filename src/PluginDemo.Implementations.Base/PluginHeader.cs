using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public class PluginHeader : IPluginHeader
    {
        #region Properties

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public string Author { get; private set; }

        public List<IPluginSetting> Settings { get; private set; }


        #endregion Properties

        #region Construction

        public PluginHeader(string Name, Version Version, string Author)
        { 
            this.Name = Name;
            this.Version = Version;
            this.Author = Author;
            this.Settings = new List<IPluginSetting>(); 
        }

        #endregion Construction

        #region Methods

        #region AddSetting
        public void AddSetting(IPluginSetting Setting)
        {
            this.Settings.Add(Setting);
        }
        #endregion AddSetting

        #endregion Methods

        #region Events

        #endregion Events
    }
}
