<template>
  <div id="ApiNotification">
    <button
      class="accordion"
      v-bind:style="{ 'border-radius': accordionBorderRadius }"
      @click="toggleAccordion"
    >
      <div class="notification-name">{{ notification.name }}</div>
      <div class="notification-description">{{ notification.description }}</div>
    </button>
    <div v-if="expanded" class="panel">
      <div class="notification-subtitle">Parameters definition</div>
      <div class="notification-definition">{{ notification.params }}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiNotification",
  data: function() {
    return {
      expanded: false,
      accordionBorderRadius: "5px"
    };
  },
  props: {
    notification: {
      name: String,
      description: String,
      params: {}
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.accordionBorderRadius = this.expanded ? "5px 5px 0px 0px" : "5px";
    }
  }
};
</script>

<style scoped lang="scss">
#ApiNotification {
  color: map-get($primary-color, 400);

  .accordion {
    font-family: inherit;
    background-color: map-get($accent-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 5px;
    width: 100%;
    text-align: left;
    outline: none;
    font-size: 15px;
    display: flex;
    border-radius: 5px 5px 0px 0px;
    border-style: solid;
    border-color: map-get($accent-color, 300);
    border-width: 1px;
    box-shadow: 2px 2px 2px 0 map-get($accent-color, 50),
      2px 2px 2px 0 map-get($accent-color, 50);
  }

  .panel {
    font-family: inherit;
    display: block;
    background-color: map-get($accent-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($accent-color, 300);
    border-width: 1px;
    border-top: none;
    box-shadow: 2px 2px 2px 0 map-get($accent-color, 50),
      2px 2px 2px 0 map-get($accent-color, 50);
  }

  .notification-name {
    color: $light-text;
    background-color: map-get($accent-color, 400);
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($accent-color, 400);
    border-width: 1px;
  }

  .notification-description {
    padding: 7px;
  }

  .notification-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($primary-color, 900);
    background: map-get($accent-color, 50);
  }

  .notification-definition {
    margin: 10px;
    font-size: 14px;
    white-space: pre-wrap;
    color: map-get($primary-color, 900);
  }
}
</style>
