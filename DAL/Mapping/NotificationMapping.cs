using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class NotificationMapping
    {
        public static IEnumerable<NotificationDto> MapToDto(IEnumerable<Notification> notifications) => notifications.Select(n => MapToDto(n));

        public static NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                ReceiverEmail = notification.ReceiverEmail,
                Subject = notification.Subject,
                Body = notification.Body
            };
        }

        public static IEnumerable<Notification> FromDto(IEnumerable<NotificationDto> notificationDtos) => notificationDtos.Select(n => FromDto(n));

        public static Notification FromDto(NotificationDto notificationDto)
        {
            return new Notification
            {
                Id = notificationDto.Id ?? 0,
                ReceiverEmail = notificationDto.ReceiverEmail,
                Subject = notificationDto.Subject,
                Body = notificationDto.Body
            };
        }
    }
}
