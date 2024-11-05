using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LanCloud.Domain.Share;
using LanCloud.Shared.Log;

namespace LanCloud.Domain.Collections
{
    public class LocalSharePartCollection : IEnumerable<LocalSharePart>
    {
        public LocalSharePartCollection(LocalShare share, ILogger logger)
        {
            Share = share;
            Logger = logger;

            Parts = share.Config.Parts
                .Select(part => new LocalSharePart(this, part))
                .ToArray();
        }

        public LocalShare Share { get; }
        public ILogger Logger { get; }

        public LocalSharePart[] Parts { get; }

        public IEnumerator<LocalSharePart> GetEnumerator()
        {
            return ((IEnumerable<LocalSharePart>)Parts).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}