<template>
  <div id="JsonRpcDocs">
    <BNavbar class="navBar">
      <BNavbarBrand class="navBarTitle">JsonRpcNet</BNavbarBrand>

      <!-- search box is disabled, because the functionality isn't implemented yet -->
      <BNavbarNav class="ml-auto" v-if="apiInfo !== void 0 && false">
        <SearchBox
          placeholder="Search for a method"
          v-model="searchString"
        ></SearchBox>
      </BNavbarNav>

      <BNavbarNav class="ml-auto">
        <BInputGroup prepend="Server" size="sm">
          <BFormSelect
            v-if="selectServerOptions.length > 1"
            right
            size="sm"
            v-model="selectedServer"
            v-bind:options="selectServerOptions"
            v-on:change="selectServer"
          >
            <optgroup label="local">
              <option value="example"
                >TBD: added server (http://example.com:8080)</option
              >
            </optgroup>
          </BFormSelect>
          <BInputGroupAppend>
            <BButton v-on:click="showAddServerDialog = !showAddServerDialog"
              >+</BButton
            >
            <BButton
              v-if="
                selectedServerInfo !== void 0 &&
                  selectedServerInfo.origin === 'local'
              "
              >-</BButton
            >
          </BInputGroupAppend>
        </BInputGroup>
      </BNavbarNav>
    </BNavbar>

    <AddServerFormDialog
      v-bind:show="showAddServerDialog"
      v-on:close="showAddServerDialog = false"
    />

    <div v-if="configErrorMessage !== void 0" class="error">
      {{ this.configErrorMessage }}
    </div>

    <div v-if="apiInfo !== void 0">
      <div
        class="split left"
        v-bind:style="{ width: showNotifications ? '80%' : '100%' }"
      >
        <div class="apiInfo">
          <ApiInfo v-bind:info="apiInfo.info" />
          <div v-for="service in apiInfo.services" v-bind:key="service.path">
            <ApiService
              v-bind:serverInfo="selectedServerInfo"
              v-bind:service="service"
            />
          </div>
        </div>
        <NotificationPanelButton
          class="notifications-button"
          v-bind:style="{ left: showNotifications ? '80%' : '100%' }"
          v-bind:numberOfNotifications="
            $root.$data.notificationsService.notifications.length
          "
          v-bind:checked="false"
          v-on:click="showNotifications = !showNotifications"
        />
      </div>
      <div
        class="split right"
        v-bind:style="{ width: showNotifications ? '20%' : '0%' }"
      >
        <NotificationPanel
          v-bind:notifications="$root.$data.notificationsService.notifications"
        />
      </div>
    </div>
    <div v-else>
      <div v-if="apiInfoErrorMessage !== void 0">
        <div class="error">
          <p>{{ apiInfoErrorMessage }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiInfo from "./components/ApiInfo.vue";
import ApiService from "./components/ApiService.vue";
import {
  BButton,
  BFormSelect,
  BInputGroup,
  BInputGroupAppend,
  BNavbar,
  BNavbarNav,
  BNavbarBrand
} from "bootstrap-vue";
import NotificationPanel from "./components/NotificationPanel.vue";
import NotificationPanelButton from "./components/NotificationPanelButton.vue";
import SearchBox from "./components/SearchBox.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import AddServerFormDialog from "./components/AddServerFormDialog";

export default {
  name: "JsonRpcDocs",
  components: {
    AddServerFormDialog,
    ApiInfo,
    ApiService,
    BButton,
    BFormSelect,
    BInputGroup,
    BInputGroupAppend,
    BNavbar,
    BNavbarBrand,
    BNavbarNav,
    NotificationPanel,
    NotificationPanelButton,
    SearchBox
  },
  data: function() {
    return {
      configErrorMessage: void 0,
      apiInfoErrorMessage: void 0,
      apiInfo: {
        info: {},
        services: [],
        definitions: {}
      },
      selectedServer: null,
      servers: [
        {
          name: String,
          url: String,
          ws: String,
          docs: String,
          origin: {
            type: "config" | "local",
            default: "config",
            validator: function(value) {
              return ["config", "local"].indexOf(value) !== -1;
            }
          }
        }
      ],
      searchString: "",
      showNotifications: false,
      showAddServerDialog: false
    };
  },
  methods: {
    getJson(url, callback, errorCallback = null) {
      var xhr = new XMLHttpRequest();
      xhr.open("GET", url, true);

      xhr.onreadystatechange = function() {
        if (xhr.readyState === 4 && xhr.status === 200) {
          callback(xhr.responseText);
        }
      };

      xhr.onerror = function() {
        if (errorCallback) {
          errorCallback(xhr.responseText);
        }
      };

      xhr.send();
    },
    selectServer() {
      const selectedServerInfo = this.servers.filter(
        x => x.name === this.selectedServer
      )[0];

      this.$root.$data.notificationsService.clearAll();

      this.apiInfo = void 0;
      this.apiInfoErrorMessage = void 0;
      const docUrl = `${selectedServerInfo.url}/${selectedServerInfo.docs}`;
      this.getJson(
        docUrl,
        text => {
          this.apiInfo = JSON.parse(text);
          this.$root.$data.typeDefinitionsService = new TypeDefinitionsService(
            this.apiInfo
          );
        },
        errorText => {
          this.apiInfoErrorMessage =
            `:~( could not retrieve api documentation from ${docUrl}. ` +
            errorText;
        }
      );
    },
    toggleNotificationPanel() {
      this.showNotifications = !this.showNotifications;
    }
  },
  computed: {
    selectServerOptions: function() {
      if (!this.servers) {
        return [];
      }

      return this.servers.map(s => {
        return { value: s.name, text: `${s.name} (${s.url})` };
      });
    },
    selectedServerInfo: function() {
      if (!this.selectedServer) {
        return void 0;
      }

      const selectedServerInfo = this.servers.filter(
        x => x.name === this.selectedServer
      );
      if (selectedServerInfo.length != 1) {
        throw new Error(
          `More than 1 server with name ${this.selectedServer} were found. The server name must be unique.`
        );
      }

      return selectedServerInfo[0];
    }
  },
  mounted() {
    this.apiInfo = void 0;
    const configFile = "./config.json";
    const errorMessage = `Failed to load configuration file from ${configFile}.`;
    this.getJson(
      configFile,
      text => {
        try {
          const config = JSON.parse(text);
          this.servers = config.servers;
          this.selectedServer = this.selectServerOptions[0].value;
          this.selectServer();
        } catch (error) {
          this.configErrorMessage = errorMessage + " " + error.message;
          return;
        }
      },
      errorText => {
        this.configErrorMessage = errorMessage + " " + errorText;
      }
    );
  }
};
</script>

<style lang="scss">
#JsonRpcDocs {
  font-family: $font-family;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;

  .split {
    position: absolute;
    top: 45px;
    bottom: 0;
    z-index: 1;
    overflow: auto;
  }

  .left {
    width: 80%;
    left: 0;
    transition: width 0.2s;
  }

  .right {
    width: 20%;
    border-left: 1px solid map-get($primary-color, 200);
    right: 0;
    transition: width 0.2s;
  }

  .apiInfo {
    margin: 20px;
  }

  .error {
    margin: 20px;
    font-size: 20px;
    color: $error-color;
  }

  .navBar {
    background: map-get($primary-color, 30);
    border-bottom: 1px solid map-get($primary-color, 100);
    height: 45px;

    .navBarTitle {
      font-size: 20px;
      color: map-get($primary-color, 500);
    }
  }

  .notifications-button {
    float: right;
    position: fixed;
    border-radius: 50%;
    top: 50px;
    left: 100%;
    margin-left: -65px;
    transition: left 0.2s;
  }
}
</style>
