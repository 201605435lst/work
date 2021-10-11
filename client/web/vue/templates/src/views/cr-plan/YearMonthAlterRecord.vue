<template>
  <a-card>
    <SmCrPlanYearMonthAlterRecord
      :axios="axios"
      :repair-tag-key="repairTagKey"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmCrPlanYearMonthAlterRecord from 'snweb-module/es/sm-cr-plan/sm-cr-plan-year-month-alter-record';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name:"YearMonthAlterRecord",
  components: { SmCrPlanYearMonthAlterRecord },
  props: ['repairTagKey'],
  computed: {
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
  methods:{
    onView(record,orgId,orgName){
      console.log(this.repairTagKey);
       this.$router.push({
        name: this.routerPrefix + 'year-month-change',
        params: {
          alterRecordId:record.id,
          planType:record.planType,
          orgId, 
          orgName, 
          repairTagKey: this.repairTagKey,
          isCreateRecord:true,
        },
      });
    },
  },

};
</script>