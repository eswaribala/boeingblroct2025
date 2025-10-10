using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public class TelemetryDemo
    {
        private readonly TelemetryClient _telemetry;

        public TelemetryDemo()
        {
            var cfg = TelemetryConfiguration.CreateDefault();
            cfg.ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            _telemetry = new TelemetryClient(cfg);
        }

        public void TrackBusinessEvent(string policyId)
        {
            _telemetry.TrackEvent("PolicyCreated");
            _telemetry.TrackMetric("PolicyCreateLatencyMs", 123.4);
        }
    }
}
