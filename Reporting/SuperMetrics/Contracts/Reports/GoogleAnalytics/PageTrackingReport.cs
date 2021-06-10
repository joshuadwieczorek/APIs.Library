using CsvHelper.Configuration.Attributes;

namespace APIs.Library.Reporting.SuperMetrics.Contracts.Reports.GoogleAnalytics
{
    public class PageTrackingReport
    {
        [Name("View ID")]
        public long ViewId { get; set; }

        [Name("Source & medium")]
        public string SourceMedium { get; set; }

        [Name("Campaign")]
        public string Campaign { get; set; }

        [Name("Device category")]
        public string DeviceCategory { get; set; }

        [Name("Page path")]
        public string PagePath { get; set; }

        [Name("Channel group")]
        public string ChannelGrouping { get; set; }

        [Name("Month")]
        public int Month { get; set; }

        [Name("Day of month"), Optional]
        public int Day { get; set; }

        [Name("Year")]
        public int Year { get; set; }

        [Name("Pageviews")]
        public string PageViews { get; set; }

        [Name("Unique pageviews")]
        public string UniquePageViews { get; set; }

        [Name("Avg. time on page")]
        public string AvgTimeOnPage { get; set; }

        [Name("Entrances")]
        public string Entrances { get; set; }

        [Name("Total time on page")]
        public string TotalTimeOnPage { get; set; }

        [Name("Exit rate")]
        public string ExitRate { get; set; }

        [Name("Entrance bounce rate")]
        public string EntranceBounceRate { get; set; }
    }
}