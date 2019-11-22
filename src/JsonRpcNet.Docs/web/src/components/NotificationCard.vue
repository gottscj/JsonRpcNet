<template>
  <div id="NotificationCard">
    <div class="notification-card" v-on:click="toggleShowNotification">
      {{ notification.title }}
      <div class="notification-timestamp">
        {{ notification.timestampStr() }}
      </div>
    </div>
    <NotificationDialog
      v-if="showNotification"
      v-on:close="toggleShowNotification"
    />
  </div>
</template>

<script>
import { Notification } from "../services/Notification.model";
import NotificationDialog from "./NotificationDialog.vue";

export default {
  name: "NotificationCard",
  components: {
    NotificationDialog
  },
  props: {
    notification: Notification
  },
  data: function() {
    return {
      showNotification: false
    };
  },
  methods: {
    toggleShowNotification() {
      this.showNotification = !this.showNotification;
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
