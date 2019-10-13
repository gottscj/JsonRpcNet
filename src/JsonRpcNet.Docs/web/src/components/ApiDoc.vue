<template>
  <h1>{{ apiInfo.info.title }}</h1>
</template>

<script>
export default {
  name: "ApiDoc",
  props: {
    apiInfo: {
      info: {
        description: String,
        version: String,
        title: String,
        contact: {
          name: String,
          email: String,
          url: String
        },
        jsonRpcApiEndpoint: String
      }
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
