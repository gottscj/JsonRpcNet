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
            right
            size="sm"
            v-model="selectedServer"
            v-bind:options="selectServerOptions"
            v-on:change="selectServer"
          />
          <BInputGroupAppend>
            <BButton v-on:click="showAddServerDialog = !showAddServerDialog"
              >+</BButton
            >
            <BButton
              v-if="
                selectedServerInfo !== void 0 &&
                  selectedServerInfo.origin === 'local'
              "
              v-on:click="removeSelectedServer"
              v-bind:style="{ 'background-color': '#c46464' }"
              >-</BButton
            >
          </BInputGroupAppend>
        </BInputGroup>
      </BNavbarNav>
    </BNavbar>

    <AddServerFormDialog
      v-bind:show="showAddServerDialog"
      v-bind:existingServerNames="allServerNames"
      v-on:close="showAddServerDialog = false"
      v-on:addServer="addServer($event)"
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
          <div
            v-for="(service, index) in apiInfo.services"
            v-bind:key="service.path"
          >
            <ApiService
              v-bind:color="colorsService.getColor(index)"
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
import { ColorsService } from "./services/Colors.service";
import AddServerFormDialog from "./components/AddServerFormDialog";
import { Validator } from "jsonschema";

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
            validator: function(value) {
              return ["config", "local"].indexOf(value) !== -1;
            }
          }
        }
      ],
      searchString: "",
      showNotifications: false,
      showAddServerDialog: false,
      colorsService: new ColorsService()
    };
  },
  methods: {
    getJson(url) {
      return new Promise(function(resolve, reject) {
        let xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = () => {
          if (xhr.status >= 200 && xhr.status < 300) {
            try {
              const json = JSON.parse(xhr.response);
              resolve(json);
            } catch (e) {
              reject(e.message);
            }
          } else {
            reject(xhr.statusText);
          }
        };
        xhr.onerror = () => reject(xhr.statusText);
        xhr.send();
      });
    },
    selectServer: async function() {
      this.apiInfo = void 0;
      this.apiInfoErrorMessage = void 0;
      this.$root.$data.notificationsService.clearAll();

      if (this.selectedServerInfo !== void 0) {
        localStorage.selectedServer = this.selectedServerInfo.name;
      } else {
        localStorage.removeItem("selectedServer");
        return;
      }

      const docUrl = `${this.selectedServerInfo.url}/${this.selectedServerInfo.docs}`;
      try {
        const apiDoc = await this.getJson(docUrl);
        const serviceSchema = await this.getJson("./serviceSchema.json");
        let jsonSchemaValidator = new Validator();
        let validationResult = jsonSchemaValidator.validate(
          apiDoc,
          serviceSchema
        );
        if (validationResult.errors.length > 0) {
          throw new Error(validationResult.toString());
        }

        this.apiInfo = apiDoc;
        this.$root.$data.typeDefinitionsService = new TypeDefinitionsService(
          this.apiInfo
        );
      } catch (e) {
        this.apiInfoErrorMessage = `:~( could not retrieve api documentation from ${docUrl}. ${
          e.message != void 0 ? e.message : ""
        }`;
      }
    },
    toggleNotificationPanel() {
      this.showNotifications = !this.showNotifications;
    },
    addServer(server) {
      server.origin = "local";
      this.servers.push(server);
      this.saveLocalServers();
      this.selectServerByName(server.name);
    },
    removeSelectedServer() {
      let serverToRemove = this.selectedServer;
      for (let i = 0; i < this.servers.length; ++i) {
        if (this.servers[i].name === serverToRemove) {
          if (this.servers.length > 1) {
            if (i === 0) {
              this.selectedServer = this.servers[1].name;
            }
            this.selectedServer = this.servers[i - 1].name;
          } else {
            this.selectedServer = void 0;
          }
          this.selectServer();
          this.servers.splice(i, 1);
          this.saveLocalServers();
          break;
        }
      }
    },
    selectServerByName(serverName) {
      this.selectedServer = serverName;
      this.selectServer();
    },
    getLocalServers() {
      if (localStorage.getItem("localServers")) {
        try {
          return JSON.parse(localStorage.getItem("localServers"));
        } catch (e) {
          localStorage.removeItem("localServers");
        }
      }
    },
    saveLocalServers() {
      const localServers = this.servers.filter(s => s.origin === "local");
      if (localServers.length > 0) {
        const localServersStr = JSON.stringify(
          this.servers.filter(s => s.origin === "local")
        );
        localStorage.setItem("localServers", localServersStr);
      } else {
        localStorage.removeItem("localServers");
      }
    }
  },
  computed: {
    selectServerOptions: function() {
      if (!this.servers) {
        return [];
      }

      return this.servers.map(s => {
        return {
          value: s.name,
          text: `${s.name} (${s.url})${s.origin === "local" ? " *" : ""}`
        };
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
          `None or more than 1 server with name ${this.selectedServer} were found. The server name must be unique.`
        );
      }

      return selectedServerInfo[0];
    },
    allServerNames: function() {
      return this.servers ? this.servers.map(s => s.name) : [];
    }
  },
  async mounted() {
    this.apiInfo = void 0;
    const configFile = "./config.json";
    const errorMessage = `Failed to load configuration file from ${configFile}.`;
    try {
      const config = await this.getJson(configFile);
      this.servers = config.servers.map(s => {
        return { ...s, origin: "config" };
      });

      const localServers = this.getLocalServers();
      if (localServers !== void 0) {
        this.servers.push(...localServers);
      }

      const lastSelectedServer = localStorage.selectedServer;
      if (this.servers.length > 0) {
        this.selectedServer =
          lastSelectedServer === void 0
            ? this.selectServerOptions[0].value
            : lastSelectedServer;
        this.selectServer();
      }
    } catch (e) {
      this.configErrorMessage = errorMessage + " " + e.message;
    }
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
