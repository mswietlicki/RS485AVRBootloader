using System;
using System.Collections.Generic;
using System.Linq;

namespace SerialAVRBootloader.Loader.Common
{
    public class MultiLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public MultiLogger(params ILogger[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void WriteLine(string rawInfo)
        {
            _loggers.ForEach(_ => _.WriteLine(rawInfo));
        }

        public void WriteError(Exception exception)
        {
            _loggers.ForEach(_ => _.WriteError(exception));
        }
    }
}