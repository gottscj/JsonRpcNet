<template>
  <div id="ApiMethod">
    <button
      class="accordion"
      v-bind:style="{ 'border-radius': accordionBorderRadius }"
      @click="toggleAccordion"
    >
      <div class="method-name">{{ method.name }}</div>
      <div class="method-description">{{ method.description }}</div>
    </button>
    <div v-if="expanded" class="panel">
      <div class="method-subtitle">Parameters</div>
      <div class="method-parameters">
        <ApiMethodParameters
          v-bind:parameters="method.parameters"
          v-on:parametersChanged="onParametersChanged"
        />
        <!-- TODO: put button on its own component -->
        <div class="call-method">
          <template v-if="showCallInProgressLoader">
            <div class="call-method-loader-box">
              <div class="call-method-loader" />
            </div>
          </template>
          <template v-else>
            <a class="call-method-button" @click="callMethod" href="#">
              <div class="face-primary">Try me!</div>
              <div class="face-secondary">Go!</div>
            </a>
          </template>
          <div v-if="websocketTriedToConnect" class="call-method-status">
            <div v-if="websocketError" class="call-method-error">
              <img class="call-method-status-icon" src="../assets/error.svg" />
              {{ websocketError }}
            </div>
            <div v-else class="call-method-success">
              <img
                class="call-method-status-icon"
                src="../assets/success.svg"
              />
            </div>
          </div>
        </div>
      </div>
      <!--TODO: create json text area component shared among parameters and response -->
      <div v-if="hasResponse" class="method-subtitle">Response</div>
      <div class="websocket-response">
        <textarea
          readonly
          v-if="hasResponse"
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
import * as ws from "jsonrpc-client-websocket";

export default {
  name: "ApiMethod",
  components: {
    ApiMethodParameters
  },
  data: function() {
    return {
      expanded: false,
      accordionBorderRadius: "5px",
      parametersJson: "",
      websocketError: null,
      websocketResponseOk: null,
      websocketResponseError: null,
      websocketTriedToConnect: false,
      showCallInProgressLoader: false,
      callInProgress: false
    };
  },
  props: {
    method: {
      name: String,
      description: String,
      returns: String,
      parameters: Array
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.accordionBorderRadius = this.expanded ? "5px 5px 0px 0px" : "5px";
    },
    onParametersChanged(value) {
      this.parametersJson = value;
    },
    async callMethod() {
      if (this.callInProgress) return;

      this.callInProgress = true;
      var timeout = setTimeout(() => {
        this.showCallInProgressLoader = true;
      }, 1000);

      this.websocketError = null;
      this.websocketResponseOk = null;
      this.websocketResponseError = null;
      this.websocketTriedToConnect = false;

      // TODO: provide base url
      const websocket = new ws.JsonRpcWebsocket(
        "ws://localhost:54624/chat",
        2000
      );

      try {
        await websocket.open();
      } catch (error) {
        this.websocketError = "Failed to establish connection";
      }

      this.websocketTriedToConnect = true;

      if (websocket.state !== ws.WebsocketReadyStates.OPEN) {
        clearTimeout(timeout);
        this.callInProgress = this.showCallInProgressLoader = false;
        return;
      }

      if (!this.hasResponse) {
        websocket.notify(this.method.name, this.parametersJson);
        clearTimeout(timeout);
        this.callInProgress = this.showCallInProgressLoader = false;
        websocket.close();
      } else {
        websocket
          .call(this.method.name, this.parametersJson)
          .then(response => {
            this.websocketResponseOk = JSON.stringify(response, null, 2);
            websocket.close();
            clearTimeout(timeout);
            this.callInProgress = this.showCallInProgressLoader = false;
          })
          .catch(error => {
            this.websocketResponseError = JSON.stringify(error, null, 2);
            websocket.close();
            clearTimeout(timeout);
            this.callInProgress = this.showCallInProgressLoader = false;
          });
      }
    }
  },
  computed: {
    hasResponse: function() {
      return this.method.returns.toLowerCase() !== "void";
    },
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
    border-radius: 5px 5px 0px 0px;
    border-style: solid;
    border-color: map-get($secondary-color, A200);
    border-width: 1px;
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
    border-color: map-get($secondary-color, A200);
    border-width: 1px;
    border-top: none;
    box-shadow: 2px 2px 2px 0 map-get($secondary-color, 50),
      2px 2px 2px 0 map-get($secondary-color, 50);
  }

  .service-arrow {
    height: 15px;
    padding: 10px 10px 0px 10px;
  }

  .method-name {
    color: $light-text;
    font-weight: bold;
    background-color: map-get($secondary-color, 400);
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($secondary-color, 400);
    border-width: 1px;
  }

  .method-description {
    padding: 7px;
  }

  .method-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($secondary-color, 500);
    background: map-get($secondary-color, 50);
  }

  .method-parameters {
    margin: 10px;
  }

  .call-method {
    height: 40px;
    display: inline-block;
    vertical-align: middle;
    $height: 30px;
    $width: 100px;
    $buttonColor: map-get($accent-color, 500);

    .call-method-button {
      margin: 10px 0px 0px 0px;

      height: $height;
      width: $width;
      display: inline-block;
      vertical-align: middle;
      border: 1px solid $buttonColor;
      font-size: 14px;
      font-weight: bold;
      text-align: center;
      text-decoration: none;
      color: $buttonColor;
      overflow: hidden;
      border-radius: 5px;

      -webkit-touch-callout: none; /* iOS Safari */
      -webkit-user-select: none; /* Safari */
      -khtml-user-select: none; /* Konqueror HTML */
      -moz-user-select: none; /* Old versions of Firefox */
      -ms-user-select: none; /* Internet Explorer/Edge */
      user-select: none; /* Non-prefixed version, currently
                            supported by Chrome, Opera and Firefox */

      .face-primary,
      .face-secondary {
        display: block;
        line-height: $height;
        transition: margin 0.2s;
      }

      .face-primary {
        background-color: $buttonColor;
        color: $light-text;
      }

      .face-secondary {
        background-color: map-get($accent-color, 50);
      }

      .face-secondary:active {
        background-color: map-get($accent-color, 100);
      }

      &:hover .face-primary {
        margin-top: -$height;
      }
    }

    .call-method-status {
      display: inline-block;
      vertical-align: middle;
      margin: 10px 0px 0px 10px;
    }

    .call-method-error {
      color: $error-color;
      vertical-align: middle;
    }

    .call-method-status-icon {
      vertical-align: middle;
      height: 20px;
    }

    .call-method-loader-box {
      margin: 10px 0px 0px 0px;
      width: $width;
      height: $height;
      display: inline-block;
      vertical-align: middle;
      border: 1px solid $buttonColor;
      overflow: hidden;
      border-radius: 5px;
      background-color: map-get($accent-color, 100);

      .call-method-loader {
        margin: 2px 0px 0px 37px;
        border: 3px solid map-get($accent-color, 500);
        border-top: 3px solid map-get($primary-color, 500);
        border-radius: 50%;
        width: 20px;
        height: 20px;
        animation: spin 1s linear infinite;
        display: inline-block;
      }

      @keyframes spin {
        0% {
          transform: rotate(0deg);
        }
        100% {
          transform: rotate(360deg);
        }
      }
    }
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
