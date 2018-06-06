using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AudioHelperLib
{
    public class SorterMp3
    {
        public void Sort(string from, string to)
        {
            if (from == null || to == null) throw new ArgumentException();
            RecursiveSort(from, to);
        }

        void RecursiveSort(string currentFolder, string destinationFolder)
        {
            if (currentFolder == null || destinationFolder == null) throw new ArgumentException();
            if (!Directory.Exists(currentFolder)) throw new ArgumentException("Directory is not exist.");

            var files = Directory.GetFiles(currentFolder);
            foreach (var file in files) TagLib.File.Create(file);
        }

        // пойти по папкам. и с каждым вновь найденным mp3 файлом. сделать следующее.
        // если заполнены теги исполнителя и названия
        // и если длина каждой строки меньше 50
        // и если нет недопустимых символов
        // привести к виду {исполнитель - название.формат}

        private static List<string> AddFiles(List<string> list, string currentfolder)
        {
            var files = Directory.GetFiles(currentfolder);
            list.AddRange(files);

            var directories = Directory.GetDirectories(currentfolder);
            foreach (var directory in directories) AddFiles(list, directory);

            return list;
        }

        private static bool MoveFiles(string currentfolder, string destinationFolder)
        {

            var files = Directory.GetFiles(currentfolder);
            foreach (var file in files)
            {
                var audio = TagLib.File.Create(file);
                var performers = string.Join(", ", audio.Tag.Performers);

                if (performers.Length > 100) performers = performers.Substring(0, 20);
                Directory.CreateDirectory("");



                var rgx = new Regex("[\\\\" + "\\/" + "\\:" + "\\*" + "\\?" + "\\<" + "\\>" + "\\|" + "\\+]");
                var filename = rgx.Replace(file.Split('\\').Last(), " ");
                //CounterChanded.Copy(file, $"{newpath}\\{filename}");

            }

            var directories = Directory.GetDirectories(currentfolder);
            foreach (var directory in directories)
                if (!MoveFiles(directory, destinationFolder)) ;

            return true;
        }

        private static bool MoveFile(string filename)
        {

            return true;
        }




    }
}
