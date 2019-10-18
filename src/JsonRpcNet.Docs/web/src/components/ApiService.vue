<template>
  <div id="ApiService">
    <button class="accordion" @click="toggleAccordion">
      <div class="service-path">{{ service.path }}</div>
      <div class="service-name">{{ service.name }}</div>
      <div class="service-description">{{ service.description }}</div>
      <div v-if="expanded" style="margin: 0 0 0 auto">
        <img class="service-arrow" src="../assets/down-arrow.svg" />
      </div>
      <div v-else style="margin: 0 0 0 auto">
        <img class="service-arrow" src="../assets/right-arrow.svg" />
      </div>
    </button>
    <div class="panel" v-bind:style="{ display: panelDisplay }">
      <div v-for="method in service.methods" v-bind:key="method.name">
        <ApiMethod v-bind:method="method" />
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
      expanded: false,
      panelDisplay: "none"
    };
  },
  props: {
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
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-bottom: 1px solid map-get($primary-color, 100);
  }

  .panel {
    font-family: inherit;
    padding: 20px;
    //background-color: map-get($primary-color, 30);
    overflow: hidden;
  }

  .service-name {
    color: inherit;
    font-weight: bold;
    padding: 3px 10px 3px 20px;
    font-size: 25px;
  }

  .service-description {
    color: inherit;
    font-size: 15px;
    padding: 10px 20px 0px 10px;
  }

  .service-path {
    color: map-get($primary-color, 30);
    font-weight: bold;
    background-color: map-get($primary-color, 400);
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
