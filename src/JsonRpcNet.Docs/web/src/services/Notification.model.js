export class Notification {
  title;
  content;
  timestamp;
  constructor(title, content) {
    this.title = title;
    this.content = content;
    this.timestamp = new Date();
  }

  timestampStr() {
    const d = this.timestamp;

    /* eslint-disable */
    return `${(d.getMonth() + 1).toString().padStart(2, '0')}/${
      d.getDate().toString().padStart(2, '0')}/${
      d.getFullYear().toString().padStart(4, '0')} ${
      d.getHours().toString().padStart(2, '0')}:${
      d.getMinutes().toString().padStart(2, '0')}:${
      d.getSeconds().toString().padStart(2, '0')}`;
    /* eslint-enable */
  }

  isExpired(timeout) {
    const elapsed = new Date() - this.timestamp; // in ms
    return elapsed >= timeout;
  }
}
