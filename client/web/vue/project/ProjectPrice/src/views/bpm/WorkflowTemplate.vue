<template>
  <a-card>
    <!-- 参数列表：<br>
    pageState:{{ pageState }} <br>
    id: {{ id }} <br>
    <br> -->
    <SmBpmWorkflowTemplate
      :id="id"
      style="width: 100%; height: 700px"
      :page-state="pageState"
      :axios="axios"
      :border="false"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmBpmWorkflowTemplate from 'snweb-module/es/sm-bpm/sm-bpm-workflow-template';
import { mapGetters } from 'vuex';

export default {
  name: 'Workflow',
  components: { SmBpmWorkflowTemplate },
  props: ['pageState', 'id'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onOk() {
      // this.$router.back();
    },
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
