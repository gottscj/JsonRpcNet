<template>
  <div id="JsonRpcDocs">
    <ApiDoc v-bind:apiInfo="apiInfo" />
  </div>
</template>

<script>
import ApiDoc from "./components/ApiDoc.vue";

export default {
  name: "JsonRpcDocs",
  components: {
    ApiDoc
  },
  data: function() {
    return {
      apiInfo: {
        info: {
          title: ""
        }
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
    //   return this.$http.get("./jsonRpcApi.json").then(response => this.apiInfo = response.data);
    // }
    this.readTextFile("./jsonRpcApi.json", text => {
      this.apiInfo = JSON.parse(text);
    });
  }
  // asyncComputed: {
  //   apiInfo2() {
  //     return this.$http.get("./jsonRpcApi.json").then(response => {
  //       // console.log(response)
  //       //this.apiInfo = response.data;
  //     });
  //   }
  // }
};
</script>

<style lang="scss">
#JsonRpcDocs {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
