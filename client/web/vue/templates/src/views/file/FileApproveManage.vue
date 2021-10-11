<template>
  <a-card>
    <SmFileManage :height="height" :axios="axios" :permissions="permissions" :is-approve="true" static-key="FileManageApprove" />
  </a-card>
</template>
<script>
import SmFileManage from 'snweb-module/es/sm-file/sm-file-manage';
import { mapGetters } from 'vuex';

export default {
  name: 'FileApproveManage',
  components: { SmFileManage },
  data() {
    return {
      data: [],
      innerHeight: 0,
    };
  },
  computed: {
    height() {
      return this.innerHeight;
    },
    ...mapGetters(['permissions']),
  },
  created() {
    this.getHeight();
    let _this = this;
    window.onresize = function() {
      _this.getHeight();
    };
  },

  methods: {
    getHeight() {
      let height = window.innerHeight;
      this.innerHeight = height - height / 4;
    },
  },
};
</script>
