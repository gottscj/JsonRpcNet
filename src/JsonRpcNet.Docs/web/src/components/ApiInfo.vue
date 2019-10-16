<template>
  <div id="ApiInfo">
    <div class="api-title">
      {{ apiInfo.title }}
      <small>
        <pre class="api-version">{{ apiInfo.version }}</pre>
      </small>
    </div>
    <div class="api-endpoint">
      [ Endpoint: {{ apiInfo.jsonRpcApiEndpoint }} ]
    </div>
    <div class="api-description">{{ apiInfo.description }}</div>
  </div>
</template>

<script>
export default {
  name: "ApiInfo",
  props: {
    apiInfo: {
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
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
#ApiInfo {
  .api-title {
    color: map-get($primary-color, 500);
    font-size: 36px;
    font-family: sans-serif;
    font-weight: bold;

    small {
      font-size: 10px;
      position: relative;
      top: -5px;
      display: inline-block;
      margin: 0 0 0 5px;
      padding: 2px 8px;
      vertical-align: super;
      border-radius: 57px;
      background: map-get($secondary-color, 400);

      pre {
        margin: 0;
        padding: 1px;
        font-family: sans-serif;
        color: map-get($secondary-color, 50);
        font-size: 14px;
      }
    }
  }

  .api-endpoint {
    font-size: 12px;
    font-weight: 300 !important;
    margin: 0;
    font-family: monospace;
    font-weight: 600;
    color: map-get($secondary-color, 400);
  }

  .api-description {
    margin: 10px;
  }
}
</style>
