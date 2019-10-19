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
    <div v-if="expanded" class="panel">
      <div class="method-subtitle">
        Parameters
      </div>
      <ApiMethodParameters
        v-bind:parameters="method.parameters"
        v-on:parametersChanged="onParametersChanged"
      />
    </div>
  </div>
</template>

<script>
import ApiMethodParameters from "./ApiMethodParameters.vue";

export default {
  name: "ApiMethod",
  components: {
    ApiMethodParameters
  },
  data: function() {
    return {
      expanded: false,
      accordionBorderRadius: "5px",
      parametersJson: ""
    };
  },
  props: {
    method: {
      name: String,
      description: String,
      returns: String,
      parameters: Array
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.accordionBorderRadius = this.expanded ? "5px 5px 0px 0px" : "5px";
    },
    onParametersChanged(value) {
      this.parametersJson = value;
    }
  }
};
</script>

<style scoped lang="scss">
#ApiMethod {
  color: map-get($primary-color, 400);

  .accordion {
    font-family: inherit;
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
    box-shadow: 0 2px 2px 0 map-get($secondary-color, 50),
      0 2px 2px 0 map-get($secondary-color, 50);
  }

  .panel {
    font-family: inherit;
    display: block;
    background-color: map-get($secondary-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($secondary-color, A200);
    border-width: 1px;
    border-top: none;
    box-shadow: 0 2px 2px 0 map-get($secondary-color, 50),
      0 2px 2px 0 map-get($secondary-color, 50);
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

  .method-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($secondary-color, 500);
    background: map-get($secondary-color, 50);
  }
}
</style>
