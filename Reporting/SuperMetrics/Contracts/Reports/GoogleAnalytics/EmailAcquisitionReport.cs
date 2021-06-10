using CsvHelper.Configuration.Attributes;

namespace APIs.Library.Reporting.SuperMetrics.Contracts.Reports.GoogleAnalytics
{
    public class EmailAcquisitionReport
    {
        [Name("View ID")]
        public long ViewId { get; set; }

        [Name("Source")]
        public string Source { get; set; }

        [Name("Medium")]
        public string Medium { get; set; }

        [Name("Campaign")]
        public string Campaign { get; set; }

        [Name("Device category")]
        public string DeviceCategory { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("Month")]
        public int Month { get; set; }

        [Name("Day of month"), Optional]
        public int Day { get; set; }

        [Name("Year")]
        public int Year { get; set; }

        [Name("Users")]
        public string Users { get; set; }

        [Name("New users")]
        public string NewUsers { get; set; }

        [Name("Sessions")]
        public string Sessions { get; set; }

        [Name("Entrance bounce rate")]
        public string EntranceBounceRate { get; set; }

        [Name("Goal completions all goals")]
        public string GoalCompletionsAllGoals { get; set; }

        [Name("Avg. pageviews per session")]
        public string AvgPageviewsPerSession { get; set; }

        [Name("Avg. session length (sec)")]
        public string AvgSessionDuration { get; set; }
    }
}