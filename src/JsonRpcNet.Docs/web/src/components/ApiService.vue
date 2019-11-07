<template>
  <div id="ApiService">
    <button class="accordion" @click="toggleAccordion">
      <div class="service-path">{{ service.path }}</div>
      <div class="service-name">{{ service.name }}</div>
      <div class="service-description">{{ service.description }}</div>
      <div v-if="expanded" class="service-arrow" style="margin: 0 0 0 auto">
        <img class="service-arrow-icon" src="../assets/down-arrow.svg" />
      </div>
      <div v-else class="service-arrow">
        <img class="service-arrow-icon" src="../assets/right-arrow.svg" />
      </div>
    </button>
    <div class="panel" v-bind:style="{ display: panelDisplay }">
      <div v-for="method in service.methods" v-bind:key="method.name">
        <ApiMethod
          class="service-method"
          v-bind:wsPath="wsPath"
          v-bind:method="method"
        />
      </div>
    </div>
  </div>
</template>

<script>
import ApiMethod from "./ApiMethod.vue";

export default {
  name: "ApiService",
  components: {
    ApiMethod
  },
  data: function() {
    return {
      expanded: true,
      panelDisplay: "block"
    };
  },
  props: {
    serverInfo: void 0,
    service: {
      name: String,
      path: String,
      description: String,
      methods: []
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.panelDisplay = this.expanded ? "block" : "none";
    }
  },
  computed: {
    wsPath: function() {
      return this.serverInfo.ws + this.service.path;
    }
  }
};
</script>

<style scoped lang="scss">
#ApiService {
  color: map-get($primary-color, 400);

  .accordion {
    font-family: inherit;
    background-color: map-get($primary-color, 30);
    color: inherit;
    cursor: pointer;
    padding: 10px;
    width: 100%;
    border: none;
    text-align: left;
    vertical-align: middle;
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-bottom: 1px solid map-get($primary-color, 100);
  }

  .panel {
    font-family: inherit;
    padding: 10px;
    //background-color: map-get($primary-color, 30);
    overflow: hidden;
  }

  .service-name {
    height: 40px;
    line-height: 40px;
    text-align: center;
    color: inherit;
    padding: 0px 20px 0px 20px;
    font-size: 25px;
  }

  .service-description {
    height: 40px;
    line-height: 40px;
    text-align: center;
    color: inherit;
    font-size: 15px;
  }

  .service-path {
    color: map-get($primary-color, 30);
    background-color: map-get($primary-color, 400);
    font-size: 15px;
    padding: 7px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($primary-color, 400);
    border-width: 1px;
  }

  .service-arrow {
    height: 40px;
    line-height: 40px;
    margin: 0 0 0 auto;
  }

  .service-arrow-icon {
    height: 40px;
    line-height: 40px;
    height: 15px;
  }

  .service-method {
    padding: 5px;
  }
}
</style>
