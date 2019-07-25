namespace App.Metrics.Reporting.SQLServer.Client
{
    public struct SqlWriteResult
    {
        public SqlWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public SqlWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}
