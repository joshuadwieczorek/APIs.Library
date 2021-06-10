using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace APIs.Library.Common
{
    public abstract class BaseLogger<T>
    {
        private readonly ILogger<T> _logger;
        private readonly Bugsnag.IClient _bugSnag;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="bugSnag"></param>
        protected BaseLogger(
              ILogger<T> logger
            , Bugsnag.IClient bugSnag)
        {
            _logger = logger;
            _bugSnag = bugSnag;
        }


        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="e"></param>
        protected void LogError(Exception e)
        {
            var exceptionData = new Dictionary<string, object>();
            foreach (DictionaryEntry item in e.Data)
                exceptionData.Add(item.Key?.ToString(), item.Value);

            _bugSnag?.Notify(e, (report) =>
            {
                report.Event.Metadata.Add("Data", exceptionData);
            });

            _logger?.LogError("{e}", e);
        }
    }
}