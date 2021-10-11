<template>
  <a-card>
    <SmCrPlanPlanChanges
      ref="SmCrPlanPlanChanges"
      :axios="axios"
      :repair-tag-key="repairTagKey"
      :org-key="organziationKey"
      :permissions="permissions"
      @view="onView"
      @edit="onEdit"
      @add="onAdd"
    />
  </a-card>
</template>

<script>
import SmCrPlanPlanChanges from 'snweb-module/es/sm-cr-plan/sm-cr-plan-plan-changes';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'PlanChanges',
  components: { SmCrPlanPlanChanges },
  props: ['repairTagKey'],
  data(){
    return{
      organziationKey:window.config.orgKey,
    };
  },
  computed: {
    ...mapGetters(['permissions']),
    routerPrefix: function() {
      let prefix = this.$route.name.split('.')[0];
      if (routePrefixes.includes(prefix)) {
        prefix += '.';
      } else {
        prefix = '';
      }
      return prefix;
    },
  },
  watch: {
    $route: function() {      
      if (this.$route.path.indexOf('plan-changes') > -1) {
        this.$refs.SmCrPlanPlanChanges.refresh();
      }
    },
  },
  methods: {
    onAdd(organizationId) {
      // console.log('onAdd', id);
      // this.$message.info(`onAdd: ${id}`);
      this.$router.push({
        name: this.routerPrefix + 'plan-change-add',
        params: {
          organizationId,
          repairTagKey: this.repairTagKey,
        },
      });
    },
    onView(id) {
      // console.log('onView', id);
      // this.$message.info(`onView: ${id}`);
      this.$router.push({
        name: this.routerPrefix + 'plan-change-view',
        params: {
          pageState: 'view',
          id,
          repairTagKey: this.repairTagKey,
        },
      });
    },
    onEdit(id, organizationId) {
      // console.log('onEdit', id);
      // this.$message.info(`onEdit: ${id}`);
      this.$router.push({
        name: this.routerPrefix + 'plan-change-edit',
        params: {
          pageState: 'edit',
          id,
          organizationId,
          repairTagKey: this.repairTagKey,
        },
      });
    },
  },
};
</script>
