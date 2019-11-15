<template>
  <ul>
    <div v-for="(note, index) in notifications" v-bind:key="`note-${index}`">
      [{{ note.timestampStr() }}] {{ note.title }}
    </div>
  </ul>
</template>

<script>
export default {
  name: "NotificationPanel",
  props: {
    notifications: Array
  }
};
</script>

<style lang="scss" scoped></style>
