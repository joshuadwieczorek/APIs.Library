using System.Xml.Serialization;
using APIs.Library.Enums;

namespace APIs.Library.Reporting
{
    public class BaseReportConfiguration
    {
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("ReportSchedule")]
        public ReportSchedule ReportSchedule { get; set; }

        [XmlElement("DataSourceDirectory")]
        public string DataSourceDirectory { get; set; }

        [XmlElement("DataInboundDirectory")]
        public string DataInboundDirectory { get; set; }

        [XmlElement("DataArchiveDirectory")]
        public string DataArchiveDirectory { get; set; }

        [XmlElement("DataFailureDirectory")]
        public string DataFailureDirectory { get; set; }

        [XmlElement("ConnectionStringName")]
        public string ConnectionStringName { get; set; }

        [XmlElement("DataLoadStoredProcedure")]
        public string DataLoadStoredProcedure { get; set; }

        [XmlElement("DataLoadStoredProcedureDaily")]
        public string DataLoadStoredProcedureDaily { get; set; }

        [XmlElement("DataLoadStoredProcedureMonthly")]
        public string DataLoadStoredProcedureMonthly { get; set; }

        [XmlElement("NormalizationStoredProcedure")]
        public string NormalizationStoredProcedure { get; set; }

        [XmlElement("NormalizationStoredProcedureDaily")]
        public string NormalizationStoredProcedureDaily { get; set; }

        [XmlElement("NormalizationStoredProcedureMonthly")]
        public string NormalizationStoredProcedureMonthly { get; set; }
    }
}