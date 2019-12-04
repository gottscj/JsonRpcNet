export class Notification {
  title;
  content;
  timestamp;
  serviceName;
  color;
  constructor(color, serviceName, title, content) {
    this.color = color;
    this.serviceName = serviceName;
    this.title = title;
    this.content = content;
    this.timestamp = new Date();
  }

  timestampStr() {
    const d = this.timestamp;

    /* eslint-disable */
    return `${d.getDate().toString().padStart(2, '0')}/${
      (d.getMonth() + 1).toString().padStart(2, '0')}/${
      d.getFullYear().toString().padStart(4, '0')} ${
      d.getHours().toString().padStart(2, '0')}:${
      d.getMinutes().toString().padStart(2, '0')}:${
      d.getSeconds().toString().padStart(2, '0')}`;
    /* eslint-enable */
  }

  isExpired(timeout) {
    return this.getElapsed() >= timeout;
  }

  getElapsed() {
    return new Date() - this.timestamp; // in ms
  }
}
