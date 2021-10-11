<template>
  <a-card>
    <SmResourceStoreEquipments :axios="axios" :modal-state="modalState" :columns="columns" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceStoreEquipments from 'snweb-module/es/sm-resource/sm-resource-store-equipments';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreEquipments',
  components: { SmResourceStoreEquipments },
  data() {
    return {
      columns: [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          width: 90,
          ellipsis: true,
          fixed: 'left',
        },
        {
          title: '产品分类',
          dataIndex: 'productCategory',
          scopedSlots: { customRender: 'productCategory' },
          ellipsis: true,
        },
        {
          title: '规格',
          dataIndex: 'model',
          scopedSlots: { customRender: 'model' },
          width: 180,
          ellipsis: true,
        },
        {
          title: '库存编号',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
          width: 130,
          ellipsis: true,
        },
        {
          title: '状态',
          dataIndex: 'state',
          scopedSlots: { customRender: 'state' },
          ellipsis: true,
          width: 110,
        },
        {
          title: '厂家名称',
          dataIndex: 'manufacturerName',
          scopedSlots: { customRender: 'manufacturerName' },
          ellipsis: true,
          width: 130,
        },
        {
          title: '出厂日期',
          dataIndex: 'manufactureDate',
          scopedSlots: { customRender: 'manufactureDate' },
          ellipsis: true,
          width: 130,
        },
        {
          title: '入库日期',
          dataIndex: 'inboundDate',
          scopedSlots: { customRender: 'inboundDate' },
          ellipsis: true,
          width: 130,
        },
        {
          title: '入库人员',
          dataIndex: 'userName',
          scopedSlots: { customRender: 'userName' },
          width: 110,
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 80,
          scopedSlots: { customRender: 'operations' },
          ellipsis: true,
          fixed: 'right',
        },
      ],
      modalState: true,
    };
  },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
