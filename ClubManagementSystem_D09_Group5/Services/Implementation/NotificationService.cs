﻿using BussinessObjects.Models;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private FptclubsContext _context;
        private INotificationRepository _NR;

        public NotificationService() 
        {
            _context = new FptclubsContext();
            _NR = new NotificationRepository();
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            return await _NR.AddNotificationAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _NR.DeleteNotificationAsync(notificationId);
        }

        public async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await _NR.GetNotificationAsync(notificationId);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            return await _NR.GetNotificationsAsync(userId);
        }

        public async Task<bool> UpdateNotificationAsync(Notification notification)
        {
            return await _NR.UpdateNotificationAsync(notification);
        }
    }
}
