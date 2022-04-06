namespace AWS.SDK.Samples.SES.Models
{
    public class Email
    {
        public Email(
            string subject,
            string htmlMessage,
            string from,
            IEnumerable<string> to)
        {
            Subject = subject;
            HtmlMessage = htmlMessage;
            From = from;
            To = to;
        }

        public string Subject { get; }
        public string HtmlMessage { get; }
        public string From { get; }
        public IEnumerable<string> To { get; }
    }
}
