<template>
  <div id="NotificationCard">
    <div class="notification-progress" v-bind:style="{ opacity: opacity }">
      <div class="notification-card" v-on:click="toggleShowNotification">
        <div
          class="notification-card-color"
          v-bind:style="{ background: notification.color }"
        />
        <div class="notification-card-service-title">
          {{ notification.serviceName }}
          <div class="notification-card-title">
            {{ notification.title }}
          </div>
        </div>
        <div class="notification-timestamp">
          {{ notification.timestampStr() }}
        </div>
      </div>
      <div class="progress">
        <div class="progressbar" v-bind:style="{ width: `${progress}%` }" />
      </div>
    </div>
    <ModalDialog v-if="showNotification" v-on:close="toggleShowNotification">
      <div class="notification-fullcard-title">
        <div
          class="notification-fullcard-color"
          v-bind:style="{ background: notification.color }"
        />
        <div class="notification-card-service-title">
          {{ notification.serviceName }}
          <div class="notification-card-title">
            {{ notification.title }}
          </div>
        </div>
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
      updateTimer: void 0,
      opacity: 1,
      progress: 100
    };
  },
  watch: {
    notification: function() {
      this.opacity = this.calculateOpacity();
      this.progress = this.calculateProgress();
    }
  },
  mounted() {
    const self = this;
    this.opacity = this.calculateOpacity();
    this.progress = this.calculateProgress();
    this.updateTimer = setInterval(() => {
      self.opacity = self.calculateOpacity();
      self.progress = self.calculateProgress();
    }, 2000);
  },
  beforeDestroy() {
    clearInterval(this.updateTimer);
  },
  methods: {
    toggleShowNotification() {
      this.showNotification = !this.showNotification;
    },
    calculateOpacity: function() {
      return (
        1 - 0.8 * (this.notification.getElapsed() / this.notificationTimeout)
      );
    },
    calculateProgress: function() {
      return (
        100 - 100 * (this.notification.getElapsed() / this.notificationTimeout)
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
  min-width: 300px;

  .notification-progress {
    margin-top: 4px;
    margin-bottom: 6px;
  }

  .notification-card-color {
    float: left;
    height: 38px;
    width: 10px;
    border-radius: 4px 0px 0px 4px;
  }

  .notification-card-service-title {
    margin-top: 2px;
    margin-left: 10px;
    font-size: 10px;
  }

  .notification-card-title {
    font-size: 14px;
  }

  .notification-card {
    display: flex;
    height: 40px;
    background-color: map-get($accent-color, 30);
    color: inherit;
    cursor: pointer;
    width: 100%;
    text-align: left;
    outline: none;
    font-size: 12px;
    border-style: solid;
    border-width: 1px;
    border-radius: 5px 5px 5px 5px;
    border-color: map-get($accent-color, 400);
    transition: opacity 2s;
  }

  .notification-fullcard-color {
    float: left;
    height: 38px;
    width: 10px;
    border-radius: 4px 0px 0px 0px;
  }

  .notification-fullcard-title {
    @extend .notification-card;
    cursor: default;
    background-color: map-get($accent-color, 400);
    font-size: 14px;
    border-radius: 5px 5px 0px 0px;
    margin-top: 0px;
    margin-bottom: 0px;
  }

  .notification-fullcard-content {
    @extend .notification-card;
    display: block;
    cursor: default;
    border-radius: 0px 0px 5px 5px;
    margin-top: 0px;
    margin-bottom: 0px;
    padding: 5px;
    border-top: 0px;
    max-height: 500px;
    height: auto;
    overflow: auto;
  }

  .notification-fullcard-content pre {
    margin: 0px;
    font-size: 14px;
    color: map-get($primary-color, 900);
  }

  .notification-timestamp {
    margin-top: 10px;
    margin-right: 10px;
    margin-left: auto;
    //float: right;
    height: 20px;
    background-color: map-get($accent-color, 100);
    color: map-get($primary-color, 900);
    padding-left: 4px;
    padding-right: 4px;
    border-radius: 3px 3px 3px 3px;
  }

  .progress {
    margin: -1px 3px 0px 3px;
    height: 1px;
    background-color: map-get($accent-color, 30);
  }

  .progressbar {
    width: 100%;
    background-color: map-get($accent-color, 400);
    transition: width 5s;
  }
}
</style>
