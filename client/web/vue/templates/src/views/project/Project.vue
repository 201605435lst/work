<template>
  <a-card>
    <SmProjects :id="id" :axios="axios" :page-state="pageState" @ok="onOk" @cancel="onCancel" @contractView="onContractView" />
  </a-card>
</template>
<script>
import SmProjects from 'snweb-module/es/sm-project/sm-project-project';
import { mapGetters } from 'vuex';

export default {
    name:'Projects',
    components:{SmProjects},
     props: ['pageState', 'id'],
    data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  created() {},
  methods: {
    onOk(){
      this.$router.back();
    },
    
    onCancel(){
      this.$router.back();
   },

   onContractView(id){
     this.$router.push({
       name:"oa-contract-view",
       params:{
         id:id,
       },
     });
   },
  },
};
</script>