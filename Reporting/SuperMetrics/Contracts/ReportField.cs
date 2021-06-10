using System.Xml.Serialization;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public class ReportField
    {
        [XmlElement("Field")]
        public string Field { get; set; }
    }
}