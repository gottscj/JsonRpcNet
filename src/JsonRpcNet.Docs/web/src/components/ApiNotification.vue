<template>
  <div id="ApiNotification">
    <div
      v-bind:class="{
        'accordion-expanded': expanded,
        'accordion-collapsed': !expanded
      }"
    >
      <button
        v-bind:class="{
          'accordion-top-expanded': expanded,
          'accordion-top-collapsed': !expanded
        }"
        @click="toggleAccordion"
      >
        <div
          v-bind:class="{
            'notification-name-expanded': expanded,
            'notification-name-collapsed': !expanded
          }"
        >
          {{ notification.name }}
        </div>
        <div class="notification-description">
          {{ notification.description }}
        </div>
        <BFormCheckbox class="notification-enable" switch v-model="listening" />
      </button>
      <div v-if="expanded" class="accordion-panel">
        <div class="notification-subtitle">Parameters definition</div>
        <div class="notification-definition">{{ notification.params }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { JsonRpcWebsocket } from "jsonrpc-client-websocket";
import { BFormCheckbox } from "bootstrap-vue";

export default {
  name: "ApiNotification",
  components: {
    BFormCheckbox
  },
  data: function() {
    return {
      expanded: false,
      listening: this.value
    };
  },
  props: {
    value: {
      type: Boolean
    },
    notification: {
      name: String,
      description: String,
      params: {}
    },
    websocket: JsonRpcWebsocket,
    color: String,
    serviceName: String
  },
  watch: {
    websocket: function() {
      this.updateNotificationListening();
    },
    listening: function() {
      this.updateNotificationListening();
      this.$emit("input", this.listening);
    },
    value: function() {
      this.listening = this.value;
    }
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
    },
    updateNotificationListening() {
      if (this.websocket) {
        if (this.listening) {
          // eslint-disable-next-line
          var func = eventArgs => {
            this.$root.$data.notificationsService.add(
              this.color,
              this.serviceName,
              this.notification.name,
              JSON.stringify(eventArgs, null, 2)
            );
          };
          this.websocket.on(this.notification.name, function(eventArgs) {
            func(eventArgs);
          });
        } else {
          // eslint-disable-next-line
          this.websocket.on(this.notification.name, function(eventArgs) {});
        }
      }
    }
  }
};
</script>

<style scoped lang="scss">
#ApiNotification {
  color: map-get($primary-color, 400);

  .accordion-collapsed {
    transition: all 0.2s linear;
  }

  .accordion-expanded {
    margin-left: -8px;
    margin-right: 8px;
    transition: all 0.2s linear;
  }

  .accordion-top {
    font-family: inherit;
    background-color: map-get($accent-color, 30);
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

  .accordion-top-expanded {
    @extend .accordion-top;
    border-radius: 5px 5px 0px 0px;
    border-color: map-get($accent-color, 400);
    box-shadow: 3px 3px 3px 0 map-get($accent-color, 100),
      3px 3px 3px 0 map-get($accent-color, 100);
  }

  .accordion-top-collapsed {
    @extend .accordion-top;
    border-radius: 5px;
    border-color: map-get($accent-color, 300);
    box-shadow: 2px 2px 2px 0 map-get($accent-color, 50),
      2px 2px 2px 0 map-get($accent-color, 50);
  }

  .accordion-panel {
    font-family: inherit;
    display: block;
    background-color: map-get($accent-color, 30);
    overflow: hidden;
    border-radius: 0px 0px 5px 5px;
    border-style: solid;
    border-color: map-get($accent-color, 400);
    border-width: 1px;
    border-top: none;
    box-shadow: 3px 3px 3px 0 map-get($accent-color, 100),
      3px 3px 3px 0 map-get($accent-color, 100);
  }

  .notification-name {
    color: $light-text;
    font-size: 15px;
    padding: 7px;
    margin-right: 10px;
    border-radius: 5px;
    border-style: solid;
    border-width: 1px;
  }

  .notification-name-expanded {
    @extend .notification-name;
    border-color: map-get($accent-color, 700);
    background-color: map-get($accent-color, 700);
  }

  .notification-name-collapsed {
    @extend .notification-name;
    border-color: map-get($accent-color, 400);
    background-color: map-get($accent-color, 400);
  }

  .notification-description {
    padding: 7px;
  }

  .notification-enable {
    padding: 7px;
    margin-left: auto;
    margin-right: 0;
  }

  .notification-subtitle {
    padding: 10px;
    font-size: 14px;
    color: map-get($primary-color, 900);
    background: map-get($accent-color, 50);
  }

  .notification-definition {
    margin: 10px;
    font-size: 14px;
    white-space: pre-wrap;
    color: map-get($primary-color, 900);
  }
}
</style>
