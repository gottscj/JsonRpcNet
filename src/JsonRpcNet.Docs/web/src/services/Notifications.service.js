import { Notification } from "../models/Notification.model";

export class NotificationsService {
  notifications = [];
  notificationTimeoutMs;
  maxNotifications;
  cleanUpIntervalMs = 5000;

  constructor(notificationTimeout = 60000, maxNotifications = 200) {
    this.notificationTimeoutMs = notificationTimeout;
    this.maxNotifications = maxNotifications;

    // cleanup loop
    setInterval(() => {
      this.notifications = [
        ...this.notifications
          .filter(n => !n.isExpired(this.notificationTimeoutMs))
          .slice(0, this.maxNotifications - 1)
      ];
    }, this.cleanUpIntervalMs);
  }

  clearAll() {
    this.notifications = [];
  }

  add(color, serviceName, title, content) {
    const notification = new Notification(color, serviceName, title, content);
    this.notifications = [notification, ...this.notifications];
  }
}
