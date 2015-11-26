namespace Fanex.MailDelivery.Services.Messaging
{
    /// <summary>
    /// Result Response.
    /// </summary>
    public class ResultMessage
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
