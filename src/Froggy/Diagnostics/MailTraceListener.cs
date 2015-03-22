using System;
using System.Diagnostics;
using System.Net.Mail;

namespace Froggy.Diagnostics
{
    /// <summary>
    /// Trace listener thats send a email to each log entry
    /// </summary>
    public class MailTraceListener : TraceListener
    {
        readonly string _toAddress;
        readonly string _fromAddress;
        readonly string _subject;
        readonly string _smtpHost;
        readonly int? _smtpPort;

        /// <summary>
        /// Initializes a new instance of <see cref="MailTraceListener"/>.
        /// </summary>
        public MailTraceListener(string subject)
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="MailTraceListener"/>.
        /// </summary>
        public MailTraceListener()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MailTraceListener"/> with a toAddress, fromAddress, 
        /// subject, smtpServer and the smtpPort
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subject">Text for the _subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        public MailTraceListener(string toAddress, string fromAddress, string subject, string smtpServer, int smtpPort)
            : this()
        {
            _toAddress = toAddress;
            _fromAddress = fromAddress;
            _smtpHost = smtpServer;
            _smtpPort = smtpPort;
            _subject = subject;
        }

        /// <summary>
        /// Sends an email body given a predefined string
        /// </summary>
        /// <param name="body">The string to write as the email message</param>
        public override void Write(string body)
        {
            var mailMessage = new MailMessage();
            if (!String.IsNullOrEmpty(_fromAddress))
                mailMessage.From = new MailAddress(_fromAddress);
            mailMessage.To.Add(_toAddress);
            mailMessage.Subject = _subject;
            mailMessage.Body = body;

            var smtpClient = new SmtpClient();
            if (!String.IsNullOrEmpty(_smtpHost))
                smtpClient.Host = _smtpHost;
            if (_smtpPort.HasValue)
                smtpClient.Port = _smtpPort.Value;
            smtpClient.SendAsync(mailMessage, null);
        }

        /// <summary>
        /// Sends an email body given a predefined string
        /// </summary>
        /// <param name="body">The string to write as the email message</param>
        public override void WriteLine(string body)
        {
            Write(body);
        }


        /// <summary>
        /// Delivers the trace data as an email message.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is string)
            {
                Write(data);
            }
            else
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
        }
    }
}