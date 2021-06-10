using System.Collections.Generic;
using System.Xml.Serialization;
using APIs.Library.Enums;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    [XmlRoot("Report")]
    public class ReportConfiguration : BaseReportConfiguration
    {
        [XmlElement("Report")]
        public Report Report { get; set; }

        [XmlElement("ReportFileName")]
        public string ReportFileName { get; set; }

        [XmlElement("DataSet")]
        public ReportDataSet DataSet { get; set; }

        [XmlArray("FieldsDaily"), XmlArrayItem("Field")]
        public List<string> FieldsDaily { get; set; }

        [XmlArray("FieldsMonthly"), XmlArrayItem("Field")]
        public List<string> FieldsMonthly { get; set; }
    }
}