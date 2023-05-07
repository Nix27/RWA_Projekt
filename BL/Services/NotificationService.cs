using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public NotificationDto Create(NotificationDto notification)
        {
            var newNotification = NotificationMapping.FromDto(notification);

            _unitOfWork.Notification.Add(newNotification);
            _unitOfWork.Save();

            return NotificationMapping.MapToDto(newNotification);
        }

        public NotificationDto? Delete(int id)
        {
            var notificationForDelete = _unitOfWork.Notification.GetFirstOrDefault(n => n.Id == id);

            if(notificationForDelete == null) return null;

            _unitOfWork.Notification.Delete(notificationForDelete);
            _unitOfWork.Save();

            return NotificationMapping.MapToDto(notificationForDelete);
        }

        public NotificationDto? Get(int id)
        {
            var requestedNotification = _unitOfWork.Notification.GetFirstOrDefault(n => n.Id == id);

            if(requestedNotification == null) return null;

            return NotificationMapping.MapToDto(requestedNotification);
        }

        public ICollection<NotificationDto> GetAll()
        {
            var allNotifications = _unitOfWork.Notification.GetAll();
            return NotificationMapping.MapToDto(allNotifications).ToList();
        }

        public int GetNumberOfUnsent()
        {
            return _unitOfWork.Notification.GetAll(n => n.SentAt == null).Count();
        }

        public void Send()
        {
            var client = new SmtpClient("127.0.0.1", 25);
            var sender = "admin@gmail.com";

            try
            {
                var forSend = _unitOfWork.Notification.GetAll(n => n.SentAt == null);

                foreach (var notification in forSend)
                {
                    try
                    {
                        var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(notification.ReceiverEmail));

                        mail.Subject = notification.Subject;
                        mail.Body = notification.Body;

                        client.Send(mail);

                        notification.SentAt = DateTime.UtcNow;
                        _unitOfWork.Save();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to send notifications");
            }
        }

        public NotificationDto? Update(int id, NotificationDto notification)
        {
            var notificationForUpdate = _unitOfWork.Notification.GetFirstOrDefault(n => n.Id == id);

            if(notificationForUpdate == null) return null;

            notificationForUpdate.ReceiverEmail = notification.ReceiverEmail;
            notificationForUpdate.Subject = notification.Subject;
            notificationForUpdate.Body = notification.Body;

            _unitOfWork.Save();

            return NotificationMapping.MapToDto(notificationForUpdate);
        }
    }
}
