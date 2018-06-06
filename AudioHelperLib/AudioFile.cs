using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace AudioHelperLib
{
    public class AudioFile : TagLib.File
    {
        public AudioFile(string path) : base(path)
        {
        }

        public AudioFile(IFileAbstraction abstraction) : base(abstraction)
        {
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void RemoveTags(TagTypes types)
        {
            throw new NotImplementedException();
        }

        public override Tag GetTag(TagTypes type, bool create)
        {
            throw new NotImplementedException();
        }

        public override Tag Tag { get; }
        public override Properties Properties { get; }
    }
}
