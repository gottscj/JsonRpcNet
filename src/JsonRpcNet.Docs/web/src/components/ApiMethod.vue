<template>
  <div id="ApiMethod">
    <button
      v-bind:class="{
        'accordion-expanded': expanded,
        'accordion-collapsed': !expanded
      }"
      @click="toggleAccordion"
    >
      <div
        v-bind:class="{
          'method-name-expanded': expanded,
          'method-name-collapsed': !expanded
        }"
      >
        {{ method.name }}
      </div>
      <div class="method-description">{{ method.description }}</div>
    </button>
    <div v-if="expanded" class="panel">
      <div class="method-subtitle">Parameters</div>
      <div class="method-parameters">
        <ApiMethodParameters
          v-bind:parameters="method.params"
          v-on:parametersChanged="onParametersChanged"
        />
        <ActionButtonWithStatus
          text="Try me!"
          hoverText="Go!"
          v-bind:status="callStatus"
          v-bind:statusText="callStatusText"
          @click="callMethod"
        />
      </div>
      <!--TODO: create json text area component shared among parameters and response -->
      <div class="method-subtitle">Response</div>
      <div class="websocket-response">
        <textarea
          readonly
          v-bind:class="
            !websocketResponseError
              ? 'websocket-response-ok'
              : 'websocket-response-error'
          "
          v-model="websocketResponse"
          placeholder="JSON response will show here"
          v-bind:rows="websocketResponseRows"
        />
      </div>
    </div>
  </div>
</template>

<script>
import ApiMethodParameters from "./ApiMethodParameters.vue";
import ActionButtonWithStatus from "./ActionButtonWithStatus.vue";
import {
  JsonRpcWebsocket,
  WebsocketReadyStates
} from "jsonrpc-client-websocket";

export default {
  name: "ApiMethod",
  components: {
    ApiMethodParameters,
    ActionButtonWithStatus
  },
  data: function() {
    return {
      expanded: false,
      parametersJson: "",
      websocketResponseOk: null,
      websocketResponseError: null,
      callInProgress: false,
      callStatus: "none",
      callStatusText: null
    };
  },
  props: {
    websocket: JsonRpcWebsocket,
    method: {
      name: String,
      description: String,
      response: Object,
      parameters: Array
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
    },
    onParametersChanged(value) {
      this.parametersJson = value;
    },
    async callMethod() {
      if (this.callInProgress) return;

      this.callInProgress = true;
      this.callStatus = "none";
      this.callStatusText = null;

      var timeout = setTimeout(() => {
        this.callStatus = "loading";
      }, 1000);

      if (
        !this.websocket ||
        this.websocket.state !== WebsocketReadyStates.OPEN
      ) {
        clearTimeout(timeout);
        this.callInProgress = false;
        this.callStatus = "error";
        this.callStatusText = "The service must be connected";
        this.websocketResponseOk = null;
        this.websocketResponseError = null;
        return;
      }

      this.websocket
        .call(this.method.name, this.parametersJson)
        .then(response => {
          this.websocketResponseOk = JSON.stringify(response, null, 2);
          this.websocketResponseError = null;
          clearTimeout(timeout);
          this.callInProgress = false;
          this.callStatus = "ok";
        })
        .catch(error => {
          this.websocketResponseError = JSON.stringify(error, null, 2);
          this.websocketResponseOk = null;
          clearTimeout(timeout);
          this.callInProgress = false;
          this.callStatus = "ok";
        });
    }
  },
  computed: {
    websocketResponse: function() {
      const response = this.websocketResponseError
        ? this.websocketResponseError
        : this.websocketResponseOk;
      return response ? response : "";
    },
    websocketResponseRows: function() {
      if (!this.websocketResponse) {
        return 1;
      }
      return this.websocketResponse.split("\n").length;
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
    display: flex;
    border-style: solid;
    border-width: 1px;
  }

  .accordion-expanded {
    @extend .accordion;
    margin-left: -8px;
    border-radius: 5px 5px 0px 0px;
    border-color: map-get($secondary-color, 400);
    box-shadow: 3px 3px 3px 0 map-get($secondary-color, 100),
      3px 3px 3px 0 map-get($secondary-color, 100);
  }

  .accordion-collapsed {
    @extend .accordion;
    border-radius: 5px;
    border-color: map-get($secondary-color, A200);
    box-shadow: 2px 2px 2px 0 map-get($secondary-color, 50),
      2px 2px 2px 0 map-get($secondary-color, 50);
  }

  .panel {
    font-family: inherit;
    display: block;
    background-color: map-get($secondary-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($secondary-color, 400);
    border-width: 1px;
    margin-left: -8px;
    margin-right: 8px;
    border-top: none;
    box-shadow: 3px 3px 3px 0 map-get($secondary-color, 100),
      3px 3px 3px 0 map-get($secondary-color, 100);
  }

  .method-name {
    color: $light-text;
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-width: 1px;
  }

  .method-name-expanded {
    @extend .method-name;
    border-color: map-get($secondary-color, 800);
    background-color: map-get($secondary-color, 800);
  }

  .method-name-collapsed {
    @extend .method-name;
    border-color: map-get($secondary-color, 500);
    background-color: map-get($secondary-color, 500);
  }

  .method-description {
    padding: 7px;
  }

  .method-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($primary-color, 900);
    background: map-get($secondary-color, 50);
  }

  .method-parameters {
    margin: 10px;
  }

  .websocket-response {
    margin: 10px;
  }

  .websocket-response-error {
    width: 800px;
    font-size: 14px;
    border-color: $error-color;
    color: $error-color;
  }

  .websocket-response-ok {
    width: 800px;
    font-size: 14px;
    border-color: map-get($secondary-color, A200);
  }
}
</style>
