using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public class PluginMetaData : IPluginMetaData
    {
        #region Properties

        public IPluginIdentifier Identifier { get; private set; }

        public string Author { get; private set; }

        #endregion Properties

        #region Construction

        public PluginMetaData(IPluginIdentifier Identifier, string Author)
        {
            this.Identifier = Identifier;
            this.Author = Author;
        }

        #endregion Construction
    }
}
