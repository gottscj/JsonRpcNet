<template>
  <address id="Contact">
    Contact:
    <a
      class="contact-element"
      v-if="!!email && !!name"
      v-bind:href="`mailto:${email}`"
      >{{ name }}</a
    >
    <template class="contact-element" v-if="!!email && !!name && !!url">
      -
    </template>
    <a class="contact-element" v-if="!!url" v-bind:href="url">{{ url }}</a>
  </address>
</template>

<script>
export default {
  name: "Contact",
  props: {
    name: String,
    email: String,
    url: String
  }
};
</script>

<style lang="scss" scoped>
#Contact {
  font-size: 12px;
  color: map-get($primary-color, 400);

  .contact-element {
    color: map-get($primary-color, 400);
  }
}
</style>
