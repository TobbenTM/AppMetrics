using System;

namespace App.Metrics.Formatters.SQLServer.Internal
{
    public class SqlMetricRow
    {
        public DateTime Timestamp { get; set; }

        public string Context { get; set; }

        public string Name { get; set; }

        public string Field { get; set; }

        public object Value { get; set; }

        public string Tags { get; set; }
    }
}
