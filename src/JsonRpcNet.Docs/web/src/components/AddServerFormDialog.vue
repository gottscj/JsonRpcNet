<template>
  <BModal
    ref="modal"
    title="Add a server to your local configuration"
    v-on:show="resetForm"
    v-on:hidden="close"
    v-on:ok="handleOk"
  >
    <BForm v-on:submit.stop.prevent="handleSubmit">
      <BFormGroup
        description="Server name to be used for display"
        label="Name"
        label-for="name-input"
        invalid-feedback="Name is required"
      >
        <BFormInput
          id="name-input"
          v-model="$v.form.name.$model"
          v-bind:state="$v.form.name.$dirty ? !$v.form.name.$error : null"
          aria-describedby="input-1-live-feedback"
          placeholder="My server"
        ></BFormInput>
      </BFormGroup>

      <BFormGroup
        description="Base server URL with port number"
        label="Server URL"
        label-for="url-input"
        invalid-feedback="Server URL is required"
      >
        <BFormInput
          id="url-input"
          v-model="$v.form.url.$model"
          v-bind:state="$v.form.url.$dirty ? !$v.form.url.$error : null"
          aria-describedby="input-1-live-feedback"
          placeholder="http://localhost:5000"
        ></BFormInput>
      </BFormGroup>

      <BFormGroup
        description="Relative URL to the json api documentation to be concatenated with the server URL"
        label="Docs relative URL"
        label-for="docs-input"
        invalid-feedback="Docs relative URL is required"
      >
        <BFormInput
          id="docs-input"
          v-model="$v.form.docs.$model"
          v-bind:state="$v.form.docs.$dirty ? !$v.form.docs.$error : null"
          aria-describedby="input-1-live-feedback"
          placeholder="jsonrpc/jsonRpcApi.json"
        ></BFormInput>
      </BFormGroup>

      <BFormGroup
        description="Websocket base url with port number"
        label="Websocket URL"
        label-for="ws-input"
        invalid-feedback="Websocket URL is required"
      >
        <BFormInput
          id="ws-input"
          v-model="$v.form.ws.$model"
          v-bind:state="$v.form.ws.$dirty ? !$v.form.ws.$error : null"
          aria-describedby="input-1-live-feedback"
          placeholder="ws://localhost:5000"
        ></BFormInput>
      </BFormGroup>
    </BForm>
  </BModal>
</template>

<script>
import { BForm, BModal, BFormGroup, BFormInput } from "bootstrap-vue";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  name: "AddServerFormDialog",
  components: {
    BModal,
    BForm,
    BFormGroup,
    BFormInput
  },
  props: {
    show: Boolean
  },
  data() {
    return {
      form: {
        name: null,
        url: null,
        docs: null,
        ws: null
      }
    };
  },
  validations: {
    form: {
      name: {
        required
      },
      url: {
        required
      },
      docs: {
        required
      },
      ws: {
        required
      }
    }
  },
  watch: {
    show: function() {
      if (this.show) {
        this.$refs.modal.show();
      } else {
        this.close();
      }
    }
  },
  methods: {
    resetForm() {
      this.form.name = "";
      this.form.url = "";
      this.form.ws = "";
      this.form.docs = "";
      this.$v.$reset();
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    handleSubmit() {
      // Exit when the form isn't valid
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        return;
      }

      this.$emit("addServer", {
        name: this.form.name,
        url: this.form.url,
        ws: this.form.ws,
        docs: this.form.docs
      });

      // Hide the modal manually
      this.$nextTick(() => {
        this.close();
      });
    },
    close() {
      this.$refs.modal.hide();
      this.$emit("close");
      this.resetForm();
    }
  }
};
</script>

<style lang="scss" scoped></style>
