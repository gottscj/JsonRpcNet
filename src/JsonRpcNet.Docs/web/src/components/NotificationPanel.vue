<template>
  <div id="NotificationPanel">
    <div class="panel">
      <NotificationCard
        v-for="(note, index) in notifications"
        v-bind:key="`note-${note.timestampStr()}-${index}`"
        v-bind:notification="note"
      />
    </div>
  </div>
</template>

<script>
import NotificationCard from "./NotificationCard.vue";

export default {
  name: "NotificationPanel",
  components: {
    NotificationCard
  },
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
}
</style>
