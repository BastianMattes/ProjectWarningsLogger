using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectWarningsLogger
{
    public class ProjectWarningsLogger : Logger
    {
        private static readonly Dictionary<string, int> _projectWarningCountDictionary = new Dictionary<string, int>();

        public override void Initialize(IEventSource eventSource)
        {
            eventSource.WarningRaised += OnWarningRaised;
            eventSource.BuildFinished += OnBuildFinished;
        }

        private void OnWarningRaised(object sender, BuildWarningEventArgs e)
        {
            var projectName = Path.GetFileNameWithoutExtension(e.ProjectFile);
            if (!_projectWarningCountDictionary.ContainsKey(projectName))
            {
                _projectWarningCountDictionary[projectName] = 1;
            }
            else
            {
                _projectWarningCountDictionary[projectName] += 1;
            }
        }

        private void OnBuildFinished(object sender, BuildFinishedEventArgs e)
        {
            foreach (var kvp in _projectWarningCountDictionary)
            {
                Console.WriteLine($"::warning title=[{kvp.Key}]::{kvp.Value} warnings found in project.");
            }
        }
    }
}
