<template>
  <div id="ActionButtonWithStatus">
    <template v-if="!enabled">
      <div class="action-button-disabled">{{ text }}</div>
    </template>
    <template v-else>
      <a class="action-button" @click="$emit('click')">
        <div class="face-primary">{{ text }}</div>
        <div class="face-secondary">{{ hoverText }}</div>
      </a>
    </template>
    <div v-if="status !== 'none'" class="action-button-status">
      <div v-if="status === 'error'" class="action-button-error">
        <img class="action-button-status-icon" src="../assets/error.svg" />
        {{ statusText }}
      </div>
      <div v-if="status === 'ok'" class="action-button-success">
        <img class="action-button-status-icon" src="../assets/success.svg" />
        {{ statusText }}
      </div>
      <div v-if="status === 'loading'">
        <div class="action-button-spinner" />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ActionButtonWithStatus",
  props: {
    enabled: {
      type: Boolean,
      default: true
    },
    text: String,
    hoverText: String,
    statusText: String,
    status: {
      validator: function(value) {
        return ["none", "loading", "ok", "error"].indexOf(value) !== -1;
      }
    }
  }
};
</script>

<style scoped lang="scss">
#ActionButtonWithStatus {
  height: 40px;
  display: inline-block;
  vertical-align: middle;
  $height: 30px;
  $width: 100px;
  $buttonColor: map-get($accent-color, 500);

  .action-button {
    margin: 10px 0px 0px 0px;
    cursor: pointer;

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

  .action-button-status {
    display: inline-block;
    vertical-align: middle;
    margin: 10px 0px 0px 10px;
  }

  .action-button-error {
    color: $error-color;
    vertical-align: middle;
  }

  .action-button-success {
    color: $success-color;
    vertical-align: middle;
  }

  .action-button-status-icon {
    vertical-align: middle;
    height: 20px;
  }

  .action-button-disabled {
    margin: 10px 0px 0px 0px;

    height: $height;
    width: $width;
    display: inline-block;
    vertical-align: middle;
    font-size: 14px;
    font-weight: bold;
    text-align: center;
    text-decoration: none;
    color: map-get($primary-color, 200);
    overflow: hidden;
    border-radius: 5px;
    line-height: $height;
    border: 1px solid map-get($primary-color, 100);
    background-color: map-get($primary-color, 50);

    -webkit-touch-callout: none; /* iOS Safari */
    -webkit-user-select: none; /* Safari */
    -khtml-user-select: none; /* Konqueror HTML */
    -moz-user-select: none; /* Old versions of Firefox */
    -ms-user-select: none; /* Internet Explorer/Edge */
    user-select: none; /* Non-prefixed version, currently
                          supported by Chrome, Opera and Firefox */
  }

  .action-button-spinner {
    vertical-align: middle;
    margin: 2px 0px 0px 0px;
    border: 3px solid map-get($secondary-color, 800);
    border-top: 3px solid map-get($accent-color, A700);
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
</style>
