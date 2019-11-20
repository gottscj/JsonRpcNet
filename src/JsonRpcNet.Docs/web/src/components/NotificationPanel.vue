<template>
  <div id="NotificationPanel">
    <div class="panel">
      <div
        class="panel-notification"
        v-for="(note, index) in notifications"
        v-bind:key="`note-${index}`"
      >
        {{ note.title }}
        <div class="notification-timestamp">{{ note.timestampStr() }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "NotificationPanel",
  props: {
    notifications: Array
  }
};
</script>

<style lang="scss" scoped>
#NotificationPanel {
  .panel {
    margin-left: 4px;
    margin-right: 4px;
  }

  .panel-notification {
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
