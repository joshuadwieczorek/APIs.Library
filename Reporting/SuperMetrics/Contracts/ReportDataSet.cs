using System.Xml.Serialization;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    [XmlRoot("DataSet")]
    public class ReportDataSet
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("User")]
        public string User { get; set; }
    }
}