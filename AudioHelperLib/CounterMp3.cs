using System;
using System.IO;
using System.Linq;

namespace AudioHelperLib
{
    public class CounterMp3
    {
        public event Action<string> CurrentDirectoryChanged;
        public event Action<int> CounterChanded;
        public event Action<int> Finished;

        private string _currentDirectory;
        public string CurrentDirectory
        {
            get
            {
                return _currentDirectory;
            }
            set
            {
                _currentDirectory = value;
                if (CurrentDirectoryChanged != null)
                    CurrentDirectoryChanged.BeginInvoke(CurrentDirectory, null, null);
            }
        }

        private int _counter;
        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
                if (CounterChanded != null)
                    CounterChanded.BeginInvoke(_counter, null, null);
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

            Counter = 0;

            var sum = (recursive) ? RecursiveSum(directory) : Sum(directory);

            if (Finished != null) Finished.BeginInvoke(Counter, null, null);
            return sum;
        }

        /// <summary>
        /// Count mp3 files in directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static int Sum(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentException();
            }

            return Directory.GetFiles(directory)
                .Where(f => f.IsMp3())
                .ToList()
                .Count;
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
                if (CurrentDirectoryChanged != null)
                    CurrentDirectoryChanged.BeginInvoke(currentDirectory, null, null);

                var files = Directory.GetFiles(currentDirectory);
                Counter = files.Where((f) => f.IsMp3()).ToList().Count;
                var directoryes = Directory.GetDirectories(currentDirectory);
                foreach (var directory in directoryes)
                    Counter += RecursiveSum(directory);
                return _counter;
            }
            catch (UnauthorizedAccessException) { return 0; }
            catch (PathTooLongException) { return 0; }
        }
    }
}
