using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorAttribute : Attribute
    {
        public string Name { get; set; }

        // This is a positional argument
        public AuthorAttribute(string Name)
        {
            this.Name = Name;

            // TODO: Implement code here

            throw new NotImplementedException();
        }
    }
}
