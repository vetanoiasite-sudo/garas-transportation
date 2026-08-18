using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.Models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Domain.Services
{
    public class MailService : IMailService
    {

        private readonly MailSettings _mailSettings;

        public MailService( IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }


        public async Task<bool> SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    if(!string.IsNullOrEmpty(mailData.SenderMail))
                        emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", mailData.SenderMail));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    //using (SmtpClient mailClient = new SmtpClient())
                    //{

                    //    mailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    //    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    //    mailClient.Send(emailMessage);
                    //    mailClient.Disconnect(true);
                    //}


                    #region Send Mail

                    using var smtp = new SmtpClient();

                    //if (_settings.UseSSL)
                    //{
                    await smtp.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                    //}
                    //else if (_settings.UseStartTls)
                    //{
                    //    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                    //}
                    await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, default);
                    await smtp.SendAsync(emailMessage, default);
                    await smtp.DisconnectAsync(true, default);

                    #endregion



                    //string smtpServer = "smtp.example.com";
                    //int smtpPort = 587;
                    //string smtpUsername = _mailSettings.UserName;
                    //string smtpPassword = _mailSettings.Password;

                    //var smtpClient = new System.Net.Mail.SmtpClient(smtpServer, smtpPort)
                    //{
                    //    UseDefaultCredentials = false,
                    //    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    //    EnableSsl = true // or false, depending on your email service provider's requirements
                    //};
                    //smtpClient.Send(emailMessage);
                    //smtpClient.Disconnect(true);
                    // use the smtpClient to send emails

                    // use the smtpClient to send emails
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }


    }
}
