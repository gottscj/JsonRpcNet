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
    <!--TODO: Move parameters to another component and add syntax highlighting -->
    <div v-if="expanded" class="panel">
      <div class="method-subtitle">
        Parameters
      </div>
      <textarea
        class="method-parameters-code"
        v-model="parameters"
        placeholder="add multiple lines"
        v-bind:style="{
          'font-size': parameterCodeFontSizePx,
          height: parametersCodeHeightPx
        }"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiMethod",
  data: function() {
    return {
      expanded: false,
      accordionBorderRadius: "5px",
      parameters: "",
      parametersCodeFontSize: 14,
      parametersCodeHeight: 100
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
  mounted() {
    this.parameters = this.createParametersJsonTemplate();
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.accordionBorderRadius = this.expanded ? "5px 5px 0px 0px" : "5px";
    },
    createParametersJsonTemplate() {
      let parametersJson = "{\n";
      let indent = "  ";

      const params = [];
      this.method.parameters.forEach(param => {
        params.push(`${indent}${param.name}: "${param.type}"`);
      });
      parametersJson += params.join(",\n");
      parametersJson += "\n}";

      this.parametersCodeHeight =
        (this.parametersCodeFontSize + 4) * (this.method.parameters.length + 2);

      return parametersJson;
    }
  },
  computed: {
    parameterCodeFontSizePx: function() {
      return `${this.parametersCodeFontSize}px`;
    },
    parametersCodeHeightPx: function() {
      return `${this.parametersCodeHeight}px`;
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

  .method-parameters-code {
    margin: 10px;
    width: 400px;
  }
}
</style>
