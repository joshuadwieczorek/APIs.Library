using CsvHelper.Configuration.Attributes;

namespace APIs.Library.Reporting.SuperMetrics.Contracts.Reports.GoogleAnalytics
{
    public class WebTrafficReport
    {
        [Name("View ID")]
        public long ViewId { get; set; }

        [Name("Campaign")]
        public string Campaign { get; set; }

        [Name("Source")]
        public string Source { get; set; }

        [Name("Medium")]
        public string Medium { get; set; }

        [Name("Channel group")]
        public string ChannelGrouping { get; set; }

        [Name("Month")]
        public int Month { get; set; }

        [Name("Day of month"), Optional]
        public int Day { get; set; }

        [Name("Year")]
        public int Year { get; set; }

        [Name("Bounces")]
        public string Bounces { get; set; }

        [Name("Users")]
        public string Users { get; set; }

        [Name("New users")]
        public string NewUsers { get; set; }

        [Name("Total time on site")]
        public string SessionDuration { get; set; }

        [Name("Sessions")]
        public string Sessions { get; set; }

        [Name("Pageviews")]
        public string PageViews { get; set; }

        [Name("Goal completions all goals")]
        public string GoalCompletionsAll { get; set; }
    }
}