<template>
  <a-card>
    <SmConstructionBaseDrawSubItem
      :axios="axios"
      :permission="permissions"
      :sub-item-content-id="id"
      has-return-btn
      :bordered="false"
      @back="back"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseDrawSubItem from 'snweb-module/lib/sm-construction-base/sm-construction-base-draw-sub-item/SmConstructionBaseDrawSubItem';

// 分部分项编制
export default {
  name: 'SubItemDraw',
  components: { SmConstructionBaseDrawSubItem },
  props: ['id'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    back() {
      this.$router.back();
    },
  },
};
</script>

<style scoped>
</style>