using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHelperLib
{
    public class CounterMp3
    {
        public event Action<int> Folder;
        public event Action<int> File;
        public event Action Finish;

        private int numberOfFolders;
        public int NumberOfFolders
        {
            get
            {
                return numberOfFolders;
            }
            set
            {
                numberOfFolders = value;
                if (Folder != null)
                    Folder.BeginInvoke(numberOfFolders, null, null);
            }
        }

        private int numberOfFiles;
        public int NumberOfFiles
        {
            get
            {
                return numberOfFiles;
            }
            set
            {

                numberOfFiles = value;
                if (File != null)
                    File.BeginInvoke(numberOfFolders, null, null);

            }
        }


        /// <summary>
        /// Count mp3 files in directory and inner directories.
        /// </summary>
        /// <param name="directory">Path.</param>
        /// <param name="recursive">Flag.</param>
        /// <returns>Number of files.</returns>
        public int Sum(string directory, bool recursive)
        {
            if (directory == null) throw new ArgumentException();

            numberOfFiles = 0;
            numberOfFolders = 0;

            int sum = (recursive) ? RecursiveSum(directory) : Sum(directory);
            if (Finish != null) Finish.BeginInvoke(null, null);
            return sum;
        }

        /// <summary>
        /// Count mp3 files in directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private int Sum(string directory)
        {
            if (directory == null) throw new ArgumentException();
            numberOfFiles =
                Directory.GetFiles(directory)
                    .Where((f) => f.IsMp3()).ToList().Count;
            return numberOfFiles;
        }

        /// <summary>
        /// Count mp3 files in dir ad inner dirs.
        /// </summary>
        /// <param name="currentDirectory">Directory.</param>
        /// <returns>Number of mp3 files.</returns>
        private int RecursiveSum(string currentDirectory)
        {
            if (currentDirectory == null) throw new ArgumentException();
            try
            {

                var files = Directory.GetFiles(currentDirectory);
                numberOfFiles = files.Where((f) => f.IsMp3()).ToList().Count;
                var directoryes = Directory.GetDirectories(currentDirectory);
                foreach (var directory in directoryes)
                    numberOfFiles += RecursiveSum(directory);
                return numberOfFiles;
            }
            catch (UnauthorizedAccessException) { return 0; }
        }
    }
}
