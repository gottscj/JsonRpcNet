<template>
  <div id="JsonRpcDocs">
    <ApiInfo v-bind:info="apiInfo.info" />
    <div v-for="service in apiInfo.services" v-bind:key="service.path">
      <ApiService v-bind:service="service" />
    </div>
  </div>
</template>

<script>
import ApiInfo from "./components/ApiInfo.vue";
import ApiService from "./components/ApiService.vue";

export default {
  name: "JsonRpcDocs",
  components: {
    ApiInfo,
    ApiService
  },
  data: function() {
    return {
      apiInfo: {
        info: {},
        services: []
      }
    };
  },
  methods: {
    readTextFile(file, callback) {
      var rawFile = new XMLHttpRequest();
      rawFile.overrideMimeType("application/json");
      rawFile.open("GET", file, true);
      rawFile.onreadystatechange = function() {
        if (rawFile.readyState === 4 && rawFile.status == "200") {
          callback(rawFile.responseText);
        }
      };
      rawFile.send(null);
    }
  },
  mounted() {
    this.readTextFile("./jsonRpcApi.json", text => {
      this.apiInfo = JSON.parse(text);
    });
  }
};
</script>

<style lang="scss">
#JsonRpcDocs {
  font-family: $font-family;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin: 20px;
}
</style>
