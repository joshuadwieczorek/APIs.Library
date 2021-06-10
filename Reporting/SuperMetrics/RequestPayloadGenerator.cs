using System.Collections.Generic;
using AAG.Global.Contracts;
using APIs.Library.Reporting.SuperMetrics.Contracts;

namespace APIs.Library.Reporting.SuperMetrics
{
    public static class RequestPayloadGenerator
    {
        public static RequestPayload GenerateRequest(
              string apiKey
            , DateRange dateRange
            , IEnumerable<string> fields
            , string dataSetId
            , string dataSetUser
            , object accountId
            , int maxRows = 100000
            , bool accountIdIsList = true
            , RequestPayloadSettings settings = null)
        {
            var request = new RequestPayload
            {
                DataSetId = dataSetId,
                DataSetUser = dataSetUser,
                DataSetAccounts = accountIdIsList ? new List<string> { $"{accountId}" } : null,
                StartDate = $"{dateRange.StartDate:yyyy-MM-dd}",
                EndDate = $"{dateRange.EndDate:yyyy-MM-dd}",
                MaxRows = maxRows,
                ApiKey = apiKey,
                Fields = new List<RequestPayloadField>()
            };

            if (!accountIdIsList)
                request.DataSetAccounts = new { filter_id = accountId };

            if (settings is not null)
                request.Settings = settings;

            foreach (var field in fields)
                request.Fields.Add(new RequestPayloadField(field));

            return request;
        }
    }
}