<template>
  <a-card>
    <SmProjects :axios="axios" :permissions="permissions" @add="onAdd" @edit="onEdit" @view="onView" />
  </a-card>
</template>
<script>
import SmProjects from 'snweb-module/es/sm-project/sm-project-projects';
import { mapGetters } from 'vuex';
import { PageState, State } from '../../common/enums';

export default {
  name:"Projects",
  components:{SmProjects},
  computed: {
    ...mapGetters(['permissions']),
  },
  methods:{
    onAdd(){
      this.$router.push({
        name:"project-project-add",
      });
    },
    onEdit(id){
      this.$router.push({
        name:"project-project-edit",
        params:{
          id,
        },
      });
    },
    onView(id){
      this.$router.push({
        name:'project-project-view',
        params:{
          id,
        },
      });
    },
  },
};
</script>