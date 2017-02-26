using Liquid.Library.Importer.Events;
using System;
using System.IO;
using System.Linq;

namespace Liquid.Library.Importer.Services
{
    class CSVReadService : ReadService
    {
        public virtual string FilePath { get; set; }

        public override void Read()
        { 
            using (var fileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var reader = new StreamReader(fileStream);
                var contents = reader.ReadToEnd();

                var seperator = new string[] { "\r\n" };
                var rows = contents.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                var columns = rows.Select(x => x.Split(',')).ToArray();

                var args = new ReadEventArgs();
                args.Data = columns;

                FireDataReadEvent(args);
                FireReadCompleteEvent();
            }
        }
    }
}
