using NewGaras.Infrastructure.Models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(MailData mailData);
    }
}
