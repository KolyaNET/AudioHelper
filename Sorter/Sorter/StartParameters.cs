using System.Threading;

namespace Sorter
{
    internal class StartParameters
    {

        internal StartParameters(SynchronizationContext context, string sourseFolder, string destinationFolder)
        {
            Context = context;
            SourseFolder = sourseFolder;
            DestinationFolder = destinationFolder;
        }

        internal SynchronizationContext Context { get; set; }

        internal string SourseFolder { get; set; }
        internal string DestinationFolder { get; set; }
    }
}