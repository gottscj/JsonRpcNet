<template>
  <div id="NotificationPanel">
    <div class="panel">
      <div
        class="panel-notification"
        v-for="(note, index) in notifications"
        v-bind:key="`note-${index}`"
      >
        [{{ note.timestampStr() }}] {{ note.title }}
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
    padding: 5px;
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
}
</style>
