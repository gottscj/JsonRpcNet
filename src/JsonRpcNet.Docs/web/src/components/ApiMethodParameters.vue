<template>
  <div id="ApiMethodParameters">
    <textarea
      v-bind:class="
        !jsonError
          ? 'method-parameters-code-ok'
          : 'method-parameters-code-error'
      "
      v-model="parametersJson"
      placeholder="json parameters"
      @change="emitParametersChange"
      v-bind:style="{
        'font-size': parameterCodeFontSizePx,
        height: parametersCodeHeightPx
      }"
    />
    <div v-if="!!jsonError" class="method-parameters-error">
      {{ jsonError }}
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiMethodParameters",
  data: function() {
    return {
      parametersJson: "",
      parametersCodeFontSize: 14,
      parametersCodeHeight: 100,
      jsonError: null
    };
  },
  props: {
    parameters: Array
  },
  mounted() {
    this.parametersJson = this.createParametersJsonTemplate();
    this.emitParametersChange();
  },
  methods: {
    createParametersJsonTemplate() {
      let parametersJson = "{\n";
      let indent = "  ";

      const params = [];
      this.parameters.forEach(param => {
        params.push(`${indent}"${param.name}": "${param.type}"`);
      });
      parametersJson += params.join(",\n");
      parametersJson += "\n}";

      this.parametersCodeHeight =
        (this.parametersCodeFontSize + 4) * (this.parameters.length + 2);

      return parametersJson;
    },
    emitParametersChange() {
      this.jsonError = null;
      let paramJson;
      try {
        paramJson = JSON.parse(this.parametersJson);
      } catch (err) {
        this.jsonError = `${err.name}: ${err.message}`;
        return;
      }

      this.$emit("parametersChanged", paramJson);
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
#ApiMethodParameters {
  margin: 10px;

  .method-parameters-code-ok {
    width: 400px;
    border-color: map-get($secondary-color, A200);
  }

  .method-parameters-code-error {
    width: 400px;
    border-color: $error-color;
  }

  .method-parameters-error {
    font-size: 12px;
    color: $error-color;
  }
}
</style>
