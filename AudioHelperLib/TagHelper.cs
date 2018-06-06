using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHelperLib
{
    public class TagHelper
    {
        /// <summary>
        /// Find audio files.
        /// </summary>
        /// <param name="list">Order files.</param>
        /// <param name="currentDirectory">Directory.</param>
        /// <returns>Number of mp3 files.</returns>
        public static List<TagLib.File> RecursiveSearch(List<TagLib.File> list, string currentDirectory)
        {
            if (currentDirectory == null) throw new ArgumentException();
            try
            {
                var files = Directory.GetFiles(currentDirectory).Where((f) => f.IsMp3()).ToList();
                foreach (var file in files)
                    list.Add(TagLib.File.Create(file));

                var directoryes = Directory.GetDirectories(currentDirectory);
                foreach (var directory in directoryes)
                    RecursiveSearch(list, directory);
                return list;
            }
            catch (UnauthorizedAccessException)
            {
                return list;
            }
        }

    }
}
