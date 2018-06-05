using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AudioFile = TagLib.Audible.File;

namespace Sorter
{
    internal static class Sorter
    {
        private static SynchronizationContext FormContext;
        private static bool Cancelled;

        internal static event Action<string> Log;
        private static void OnLog(object o) => Log?.Invoke((string)o);


        internal static void Stop() => Cancelled = true;

        internal static void Start(object startParameters)
        {
            var param = (StartParameters)startParameters;
            FormContext = FormContext == null ? param.Context : FormContext;
            MoveFiles(param.SourseFolder, param.DestinationFolder);
        }

        private static List<string> AddFiles(List<string> list, string currentfolder)
        {
            var files = Directory.GetFiles(currentfolder);
            list.AddRange(files);

            FormContext.Send(OnLog, $"Files added from {currentfolder}");
            FormContext.Send(OnLog, $"In {currentfolder} founded {files.Length} files.");

            var directories = Directory.GetDirectories(currentfolder);
            foreach (var directory in directories) AddFiles(list, directory);

            return list;
        }

        private static bool MoveFiles(string currentfolder, string destinationFolder)
        {
            if (Cancelled) return true;
            FormContext.Send(OnLog, $"Directory chanded: {currentfolder}");

            var files = Directory.GetFiles(currentfolder);
            foreach (var file in files)
            {
                if (Cancelled) return true;

                var audio = TagLib.File.Create(file);
                var performers = string.Join(", ", audio.Tag.Performers);

                if (performers.Length > 100) performers = performers.Substring(0, 20);

                var newpath = $"{destinationFolder}\\{performers}";

                try
                {
                    if (!Directory.Exists(newpath)) Directory.CreateDirectory(newpath);
                }
                catch (Exception e)
                {
                    
                }


                var rgx = new Regex("[\\\\" + "\\/" + "\\:" + "\\*" + "\\?" + "\\<" + "\\>" + "\\|" + "\\+]");
                var filename = rgx.Replace(file.Split('\\').Last(), " ");
                File.Copy(file, $"{newpath}\\{filename}");

                FormContext.Send(OnLog, $"{performers} {file}");
            }

            var directories = Directory.GetDirectories(currentfolder);
            foreach (var directory in directories)
                if (!MoveFiles(directory, destinationFolder))
                    FormContext.Send(OnLog, $"MoveFiles error."); ;

            return true;
        }

        private static bool MoveFile(string filename)
        {

            return true;
        }




    }
}
