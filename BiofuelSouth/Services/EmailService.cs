using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BiofuelSouth.Services
{
    enum MsgType
    {
        ConfirmationMessages = 1, Comments = 2 
    }

    public static class EMailService
    {
        //readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*
            <!-- Email Settings -->
                <add key="FromEmailId" value="benktesh2@gmail.com" />
                <add key="FromEmailPassword" value="TestAccount1" />
                <add key="SMTPHost" value="smtp.gmail.com" />
                <add key="Port" value="587" />
                <add key="EnableSsl" value="true" />
                <add key="DefaultToEmail" value="benktesh.sharma@egov.com;benktesh2@gmail.com" />
            <!-- End Email Settings -->
         * 
         */

        //TODO
        //Read settings for to email from the list
        //Send email

        /// <summary>
        /// Sends a formatted message to recipients. 
        /// Reads the values for smtp servers and from addresses from xml data
        /// </summary>
        /// <param name="messageBody"> Formatted email body message </param>
        /// <param name ="toEmail"> To email addresses separated by comma</param>
        /// <param name="subjectLine"> Subject line for email</param>
        public static void SendEmail(String messageBody, string toEmail, string subjectLine)
        {
            //For test, this email is being sent to local test account anyway.
            var fromEmail = ConfigurationManager.AppSettings["FromEmailId"];
            var password = ConfigurationManager.AppSettings["FromEmailPassword"];
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"];
            var port = ConfigurationManager.AppSettings["Port"];
            var enableSsl = ConfigurationManager.AppSettings["EnableSsl"];

            using (var message = new MailMessage(fromEmail, toEmail))
            {
                try
                {
                    var toEmailList = ConfigurationManager.AppSettings["DefaultToEmail"].Split(',');

                    foreach (var s in toEmailList)
                    {
                        message.Bcc.Add(s);
                        message.CC.Add(s);
                    }

                    
                    message.Subject = subjectLine;
                    message.Body = messageBody;
                    using (var client = new SmtpClient
                    {
                        EnableSsl = bool.Parse(enableSsl),
                        Host = smtpHost,
                        Port = Int32.Parse(port),
                        Credentials = new NetworkCredential(fromEmail, password)
                    })
                        client.Send(message);
                }
                catch (SmtpFailedRecipientException ex)
                {
                    var msg = (ex.StackTrace + "\n\t" + message.To + " \n" + message.Body);
                    Debug.Print(msg);

                    //Logger.Error(msg);
                    
                }
                catch (Exception ex)
                {
                    var msg = (ex.StackTrace + " test\n\t" + message.To + " \n" + message.Body);
                    Debug.Print(msg);
                    //Logger.Error(msg);
                    
                }
            }
        }

        /// <summary>
        /// Creates a message body for a BusinessProcess.The footer of the message is specified in the XML file.
        /// Fetches Business Name from BusinessApplicationViewModel, and user's first and last names from BusinessApplicationDataViewModel 
        /// </summary>
        /// <returns>Formatted Email Body Message</returns>
        private static string GetMsgBody(MsgType type)
        {
            var msgbody = new StringBuilder();

            if (type == MsgType.Comments)
            {
                msgbody.Append("We have received your feedback. Thank you.");
            }
            else if (type == MsgType.ConfirmationMessages)
            {
                msgbody.Append("We have received your feedback. Thank you.");
            }
         
            return msgbody.ToString();
        }

    }
}