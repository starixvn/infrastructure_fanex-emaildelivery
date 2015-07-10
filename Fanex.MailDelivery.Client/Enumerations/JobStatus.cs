namespace Fanex.MailDelivery.Client.Enumerations
{
    /// <summary>
    /// Job Status.
    /// </summary>
    public enum JobStatus : int
    {
        /// <summary>
        /// The waiting status.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// The running status.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The data is not ready status.
        /// </summary>
        DataIsNotReady = 2,

        /// <summary>
        /// The failed status.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The success status.
        /// </summary>
        Success = 4,

        /// <summary>
        /// The retry status.
        /// </summary>
        Retry = 5,

        /// <summary>
        /// The abort status.
        /// </summary>
        Abort = 6,

        /// <summary>
        /// The success without send mail status.
        /// </summary>
        SuccessWithoutSendmail = 7,

        /// <summary>
        /// The missing job status.
        /// </summary>
        Missing = 8
    }
}
