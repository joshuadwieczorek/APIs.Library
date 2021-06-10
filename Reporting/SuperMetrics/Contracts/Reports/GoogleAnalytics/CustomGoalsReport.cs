using CsvHelper.Configuration.Attributes;

namespace APIs.Library.Reporting.SuperMetrics.Contracts.Reports.GoogleAnalytics
{
    public class CustomGoalsReport
    {
        [Name("View ID")]
        public long ViewId { get; set; }

        [Name("Campaign")]
        public string Campaign { get; set; }

        [Name("Source & medium")]
        public string Source { get; set; }

        [Name("Channel group")]
        public string Medium { get; set; }

        [Name("Month")]
        public int Month { get; set; }

        [Name("Day of month"), Optional]
        public int Day { get; set; }

        [Name("Year")]
        public int Year { get; set; }

        [Name("Goal 1 completions")]
        public string PhoneCalls { get; set; }

        [Name("Goal 2 completions")]
        public string Chat { get; set; }

        [Name("Goal 3 completions")]
        public string NewInventory { get; set; }

        [Name("Goal 4 completions")]
        public string UsedInventory { get; set; }

        [Name("Goal 5 completions")]
        public string ScheduleService { get; set; }

        [Name("Goal 6 completions")]
        public string Preapproval { get; set; }

        [Name("Goal 7 completions")]
        public string TradeValuation { get; set; }

        [Name("Goal 10 completions")]
        public string GeneralForms { get; set; }

        [Name("Goal 11 completions")]
        public string DigitalRetailing { get; set; }
    }
}