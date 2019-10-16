<template>
  <div id="ApiService">
    <button class="accordion" @click="visible = !visible">
      <div class="service-path">
        {{ service.path }}
      </div>
      <div class="service-name">
        {{ service.name }}
      </div>
      <div class="service-description">
        {{ service.description }}
      </div>
      <div v-if="visible" style="margin: 0 0 0 auto">
        <img class="service-arrow" src="../assets/down-arrow.svg" />
      </div>
      <div v-else style="margin: 0 0 0 auto">
        <img class="service-arrow" src="../assets/right-arrow.svg" />
      </div>
    </button>
    <div v-if="visible" class="panel">
      <p>TODO: Methods go here</p>
    </div>
  </div>
</template>

<script>
export default {
  name: "ApiService",
  data: function() {
    return {
      visible: false
    };
  },
  props: {
    service: {
      name: String,
      path: String,
      description: String
    }
  }
};
</script>

<style scoped lang="scss">
#ApiService {
  color: map-get($primary-color, 400);

  .accordion {
    background-color: map-get($primary-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 10px;
    width: 100%;
    border: none;
    text-align: left;
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-bottom: 1px solid map-get($primary-color, 100);
  }

  .panel {
    padding: 0 18px;
    display: block;
    background-color: map-get($primary-color, 30);
    overflow: hidden;
  }

  .service-name {
    color: inherit;
    font-weight: bold;
    padding: 3px 3px 3px 20px;
    font-size: 25px;
  }

  .service-description {
    color: inherit;
    font-size: 15px;
    padding: 10px 20px 0px 40px;
  }

  .service-path {
    color: map-get($primary-color, 30);
    font-weight: bold;
    background-color: map-get($primary-color, 300);
    font-size: 15px;
    padding: 7px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($primary-color, 400);
    border-width: 1px;
  }

  .service-arrow {
    height: 15px;
    padding: 10px 10px 0px 10px;
  }
}
</style>
