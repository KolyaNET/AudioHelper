using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagHelper
{
public static class Methods
    {
        public static int CheckEmptyPerformers(string currentfolder, List<TagLib.File> list)
        {
            var files = Directory.GetFiles(currentfolder);
            var audios = files.Select(TagLib.File.Create);
            var emtyPerformers = audios.Count(audio => audio.Tag.Performers.Length == 0);

            var listEmtyPerformers = audios.Select(o => o).Where(audio => audio.Tag.Performers.Length == 0);
            list.AddRange(listEmtyPerformers);

            var directories = Directory.GetDirectories(currentfolder);
            emtyPerformers += directories.Sum(directory => CheckEmptyPerformers(directory, list));

            return emtyPerformers;
        }

        public static int SetPerformersFromFilename(string currentfolder, List<TagLib.File> list)
        {
            var files = Directory.GetFiles(currentfolder);
            var audios = files.Select(TagLib.File.Create);
            var emtyPerformers = audios.Count(audio => audio.Tag.Performers.Length == 0);

            var listEmtyPerformers = audios.Select(o => o).Where(audio => audio.Tag.Performers.Length == 0);

            foreach (var emtyPerformer in listEmtyPerformers)
            {
                emtyPerformer.Tag.Performers = new[] { emtyPerformer.Name.Split('\\').Last().Split('-')[0].Trim(' ') };
                emtyPerformer.Save();
            }
            list.AddRange(listEmtyPerformers);

            var directories = Directory.GetDirectories(currentfolder);
            emtyPerformers += directories.Sum(directory => CheckEmptyPerformers(directory, list));

            return emtyPerformers;
        }

    }
}
