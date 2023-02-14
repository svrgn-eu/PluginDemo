using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Helpers
{
    public static class PluginIdentifierHelper
    {
        #region ExtractIdentifier
        public static IPluginIdentifier ExtractIdentifier(string assemblyFullname)
        {
            IPluginIdentifier result = default;

            // e.g. assemblyFullname = "PluginDemo.Implementations.DemoPlugin1, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null"
            string[] parts = assemblyFullname.Split(',');
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
                string versionText = parts[1].Replace("Version=", string.Empty);  //TODO: should we be less static here?

                Version version;
                if (Version.TryParse(versionText, out version))
                {
                    result = new PluginIdentifier(name, version);  //TODO: outsource creation
                }
                else
                { 
                    //todo: error, string could not be parsed as version
                }
            }
            else
            { 
                //TODO: error, string is not in the right format
            }
            return result;
        }
        #endregion ExtractIdentifier
    }
}
