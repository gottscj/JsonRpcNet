<template>
  <div id="ApiMethod">
    <button
      class="accordion"
      v-bind:style="{ 'border-radius': accordionBorderRadius }"
      @click="toggleAccordion"
    >
      <div class="method-name">{{ method.name }}</div>
      <div class="method-description">{{ method.description }}</div>
    </button>
    <div v-if="expanded" class="panel">TODO: method</div>
  </div>
</template>

<script>
export default {
  name: "ApiMethod",
  data: function() {
    return {
      expanded: false,
      accordionBorderRadius: "5px"
    };
  },
  props: {
    method: {
      name: "string",
      description: "string",
      returns: "string",
      parameters: [
        {
          name: "string",
          type: "string"
        }
      ]
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
#ApiMethod {
  color: map-get($primary-color, 400);

  .accordion {
    background-color: map-get($secondary-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 5px;
    width: 100%;
    text-align: left;
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-radius: 5px 5px 0px 0px;
    border-style: solid;
    border-color: map-get($secondary-color, A200);
    border-width: 1px;
  }

  .panel {
    padding: 10px;
    display: block;
    background-color: map-get($secondary-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($secondary-color, A200);
    border-width: 1px;
    border-top: none;
  }

  .service-arrow {
    height: 15px;
    padding: 10px 10px 0px 10px;
  }

  .method-name {
    color: $light-text;
    font-weight: bold;
    background-color: map-get($secondary-color, 400);
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($secondary-color, 400);
    border-width: 1px;
  }

  .method-description {
    padding: 7px;
  }
}
</style>
