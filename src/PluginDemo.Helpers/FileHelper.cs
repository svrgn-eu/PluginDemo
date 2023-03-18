using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginDemo.Helpers
{
    public static class FileHelper
    {
        #region CopyFilesRecursively
        // see https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp
        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(targetPath))
            { 
                // create directory if needed
                Directory.CreateDirectory(targetPath);
            }

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        #endregion CopyFilesRecursively
    }
}