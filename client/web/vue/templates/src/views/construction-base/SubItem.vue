<template>
  <a-card>
    <SmConstructionBaseSubItem :axios="axios" :permission="permissions" :bordered="false" @edit="edit" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseSubItem
  from 'snweb-module/lib/sm-construction-base/sm-construction-base-sub-item/SmConstructionBaseSubItem';

export default {
  name: 'SubItem',
  components: {SmConstructionBaseSubItem},
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    edit(id) {
      console.log('订阅',id);
      this.$router.push({
        name: 'subItemDraw',
        params: {
          id,
        },
      });
    },
  },
};
</script>

<style scoped>

</style>