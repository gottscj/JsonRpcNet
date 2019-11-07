<template>
  <BInputGroup size="sm">
    <BFormInput
      :placeholder="placeholder"
      v-model="searchString"
      @input="$emit('input', searchString)"
    ></BFormInput>
    <BInputGroupAppend>
      <BButton>Search</BButton>
    </BInputGroupAppend>
  </BInputGroup>
</template>

<script>
import {
  BInputGroup,
  BInputGroupAppend,
  BFormInput,
  BButton
} from "bootstrap-vue";

export default {
  name: "SearchBox",
  components: {
    BInputGroup,
    BInputGroupAppend,
    BFormInput,
    BButton
  },
  data: function() {
    return {
      searchString: this.value
    };
  },
  props: ["value", "placeholder"]
};
</script>

<style lang="scss" scoped></style>
