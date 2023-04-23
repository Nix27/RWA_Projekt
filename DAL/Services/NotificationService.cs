using DAL.DTO;
using DAL.IRepositories;
using DAL.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
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
