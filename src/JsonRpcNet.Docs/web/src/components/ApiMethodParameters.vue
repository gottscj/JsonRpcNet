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
      v-bind:rows="parametersCodeRows"
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
      parametersCodeRows: 1,
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
      let parametersJson = {};
      this.parameters.forEach(param => {
        parametersJson[param.name] = param.type;
      });

      this.parametersCodeRows = this.parameters.length + 2;

      return JSON.stringify(parametersJson, null, 2);
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
  }
};
</script>

<style scoped lang="scss">
#ApiMethodParameters {
  .method-parameters-code-ok {
    width: 800px;
    border-color: map-get($secondary-color, A200);
  }

  .method-parameters-code-error {
    width: 800px;
    border-color: $error-color;
  }

  .method-parameters-error {
    font-size: 12px;
    color: $error-color;
  }
}
</style>
