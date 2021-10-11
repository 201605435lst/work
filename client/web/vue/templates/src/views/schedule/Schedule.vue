<template>
  <a-card>
    <SmScheduleSchedule
      :id="id"
      ref="SmScheduleSchedule"
      :axios="axios"
      :page-state="pageState"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmScheduleSchedule from 'snweb-module/es/sm-schedule/sm-schedule-schedule';
import { mapGetters } from 'vuex';
import { PageState } from '../../common/enums';

export default {
  name: 'Supplier',
  components: { SmScheduleSchedule },
  props: ['pageState', 'id'],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: {
      handler: function (value, oldValue) {
        if (value.path.indexOf('schedule-schedule') === -1 && this.pageState !== PageState.Add) {
          this.$refs.SmScheduleSchedule.refresh(this.id);
        }
      },
    },
  },
  created() {},
  methods: {
    onOk() {
      this.$router.back();
    },

    onCancel() {
      this.$router.back();
    },
  },
};
</script>
