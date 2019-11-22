<template>
  <div id="NotificationCard">
    <div
      class="notification-card"
      v-bind:style="{ opacity: notificationOpacity }"
      v-on:click="toggleShowNotification"
    >
      {{ notification.title }}
      <div class="notification-timestamp">
        {{ notification.timestampStr() }}
      </div>
    </div>
    <ModalDialog v-if="showNotification" v-on:close="toggleShowNotification">
      <div class="notification-fullcard-title">
        {{ notification.title }}
        <div class="notification-timestamp">
          {{ notification.timestampStr() }}
        </div>
      </div>
      <div class="notification-fullcard-content">
        <pre>{{ notification.content }}</pre>
      </div>
    </ModalDialog>
  </div>
</template>

<script>
import { Notification } from "../models/Notification.model";
import ModalDialog from "./ModalDialog.vue";

export default {
  name: "NotificationCard",
  components: {
    ModalDialog
  },
  props: {
    notification: Notification
  },
  data: function() {
    return {
      showNotification: false,
      notificationOpacity: 1,
      opacityTimer: void 0
    };
  },
  watch: {
    notification: function() {
      this.notificationOpacity = this.calculateNotificationOpacity();
    }
  },
  mounted() {
    const self = this;
    this.notificationOpacity = this.calculateNotificationOpacity();
    this.opacityTimer = setInterval(() => {
      self.notificationOpacity = self.calculateNotificationOpacity();
    }, 2000);
  },
  beforeDestroy() {
    clearInterval(this.opacityTimer);
  },
  methods: {
    toggleShowNotification() {
      this.showNotification = !this.showNotification;
    },
    calculateNotificationOpacity: function() {
      return (
        1 - 0.8 * (this.notification.getElapsed() / this.notificationTimeout)
      );
    }
  },
  computed: {
    notificationTimeout: function() {
      return this.$root.$data.notificationsService.notificationTimeoutMs;
    }
  }
};
</script>

<style lang="scss" scoped>
#NotificationCard {
  .notification-card {
    background-color: map-get($accent-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 8px;
    width: 100%;
    margin-top: 4px;
    margin-bottom: 4px;
    text-align: left;
    outline: none;
    font-size: 12px;
    border-style: solid;
    border-width: 1px;
    border-radius: 5px 5px 5px 5px;
    border-color: map-get($accent-color, 400);
  }

  .notification-fullcard-title {
    @extend .notification-card;
    font-size: 14px;
    border-radius: 5px 5px 0px 0px;
    margin-top: 0px;
    margin-bottom: 0px;
  }

  .notification-fullcard-content {
    @extend .notification-card;
    border-radius: 0px 0px 5px 5px;
    margin-top: 0px;
    margin-bottom: 0px;
    border-top: 0px;
  }

  .notification-fullcard-content pre {
    margin: 0px;
    font-size: 14px;
    color: map-get($primary-color, 900);
  }

  .notification-timestamp {
    float: right;
    height: 20px;
    background-color: map-get($accent-color, 100);
    color: map-get($primary-color, 900);
    padding-left: 4px;
    padding-right: 4px;
    border-radius: 3px 3px 3px 3px;
  }
}
</style>
