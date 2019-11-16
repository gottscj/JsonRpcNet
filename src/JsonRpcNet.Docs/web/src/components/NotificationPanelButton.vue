<template>
  <div id="NotificationPanelButton">
    <BButton class="notification-button" v-on:click="$emit('click')">
      <img class="notification-icon" src="../assets/notification.svg" />
      <BBadge class="notification-count" v-if="numberOfNotifications > 0">{{
        numberOfNotifications
      }}</BBadge>
    </BButton>
  </div>
</template>

<script>
import { BButton, BBadge } from "bootstrap-vue";

export default {
  name: "NotificationPanelButton",
  components: {
    BBadge,
    BButton
  },
  props: {
    numberOfNotifications: Number
  }
};
</script>

<style lang="scss" scoped>
#NotificationPanelButton {
  .btn:focus,
  .btn:active {
    outline: none !important;
    box-shadow: none;
  }

  .notification-button {
    float: left;
    position: relative;
    border-radius: 50px;
    background: none;
    border: none;
    margin-right: -15px;
  }

  .notification-icon {
    height: 35px;
  }

  .notification-count {
    background-color: map-get($accent-color, 200);
    color: map-get($primary-color, 900);
    border-radius: 50px;
    position: absolute;
    top: 0px;
    right: 7px;
  }
}
</style>
