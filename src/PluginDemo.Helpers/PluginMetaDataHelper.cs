using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Helpers
{
    public static class PluginMetaDataHelper
    {
        #region ExtractMetadata
        public static IPluginMetaData ExtractMetadata(string AuthorName, string AssemblyFullname)
        {
            IPluginMetaData result = default;

            IPluginIdentifier identifier = PluginIdentifierHelper.ExtractIdentifier(AssemblyFullname);

            if (identifier != null)
            {
                result = new PluginMetaData(identifier, AuthorName);  //TODO: create somewhere different with a service
            }
 
            return result;
        }
        #endregion ExtractMetadata
    }
}
