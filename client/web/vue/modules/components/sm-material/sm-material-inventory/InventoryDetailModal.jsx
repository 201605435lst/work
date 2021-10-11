/**
 * 说明：物资入库登记
 * 作者：easten
 */

import { form as formConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess } from '../../_utils/utils';
import FileSaver from 'file-saver';
import moment from 'moment';
import ApiInventory from '../../sm-api/sm-material/Inventory';
let apiInventory = new ApiInventory();

export default {
  name: 'InventoryDetailModal',
  components: {},
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 800 },
    height: { type: Number, default: 1000 },
  },
  data() {
    return {
      form: {},
      record: null,
      files: [],
      title: '物资入库登记',
      type: 0, // 对话框类型
      status: ModalStatus.Hide,
      confirmLoading: false, //确定按钮加载状态
      selectedFileIds: '',

      materialTypes: [], // 材料类型
      materials: [], // 材料
      materialsFilters: [],
      materialSearchKeyWords: '',
      suppliers: [], // 供应商
      paritals: [], // 料库信息
      materialId: null,
      materialTypeId: null,
      partitionId: null,
      supplierId: null,
      entryDatas: [],
      outDatas: [],
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    materialData() {
      return this.materialSearchKeyWords === ''
        ? this.materialsFilters
        : this.materialsFilters.filter(a => a.name == this.materialSearchKeyWords);
    },
    entryColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'code',
          width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '入库量',
          dataIndex: 'count',
          ellipsis: true,
        },
        {
          title: '入库时间',
          dataIndex: 'time',
          ellipsis: true,
          scopedSlots: { customRender: 'time' },
        },
        {
          title: '登记人',
          dataIndex: 'entryRecord.creator.name',
          ellipsis: true,
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
        },
      ];
    },
    outColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'code',
          width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '出库量',
          dataIndex: 'count',
          ellipsis: true,
        },
        {
          title: '出库时间',
          dataIndex: 'time',
          ellipsis: true,
          scopedSlots: { customRender: 'time' },
        },
        {
          title: '登记人',
          dataIndex: 'outRecord.creator.name',
          ellipsis: true,
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
        },
      ];
    },
  },
  watch: {},

  created() {
    apiInventory = new ApiInventory(this.axios);
  },

  methods: {
    async view(record) {
      this.status = ModalStatus.View;
      this.title = '物资出入库详情';
      if (!record.id) return;
      let response = await apiInventory.get(record.id);
      if (requestIsSuccess(response) && response.data) {
        this.record = response.data;
        this.entryDatas = response.data.entryRecords;
        this.outDatas = response.data.outRecords;
      }
    },

    close() {
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },

    // 出入库详情导出
    async export() {
      let response = await apiInventory.exportDetail(this.record.id);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          this.$message.info('导出成功');
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `物资出入库记录表.docx`,
          );
        }
      }
    },

  },
  render() {
    return (
      <a-modal
        class="inventory-detail-modal"
        title={this.title}
        onOk={this.export}
        okText='导出'
        visible={this.visible}
        onCancel={this.close}
        width={this.width}
        height={this.height}
        confirmLoading={this.confirmLoading}
      >
        <a-form >
          <a-row gutter={16}>
            <a-col sm={12} md={12}>
              <a-form-item
                label="材料名称"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.material ? this.record.material.name : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="规格型号"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.material ? this.record.material.spec : ''} />
              </a-form-item>
            </a-col>

            {/* <a-col sm={12} md={12}>
              <a-form-item
                label="材料型号"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.material ? this.record.material.model : ''} />
              </a-form-item>
            </a-col> */}

            <a-col sm={12} md={12}>
              <a-form-item
                label="库存位置"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.partition ? this.record.partition.name : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="库存数量"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record ? this.record.amount : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="价格"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record ? this.record.price : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="供应商"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.supplier ? this.record.supplier.name : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="登记时间"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <a-input disabled value={this.record && this.record.entryTime ? moment(this.record.entryTime).format('YYYY-MM-DD') : ''} />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>

        {/* 出入库信息 */}
        <div>
          <a-tabs default-active-key="1" size="small">
            <a-tab-pane key="1" tab="入库信息">
              <a-table
                columns={this.entryColumns}
                size="small"
                dataSource={this.entryDatas}
                scroll={{ y: 300 }}
                pagination={false}
                bordered
                rowKey={record => record.id}
                {...{
                  scopedSlots: {
                    time: (text, record) => {
                      return record && record.entryRecord && record.entryRecord.time ? moment(record.entryRecord.time).format("YYYY-MM-DD HH:mm:ss") : '';
                    },
                    code: (text, record, index) => {
                      return index + 1;
                    },
                  },
                }}
              ></a-table>
            </a-tab-pane>
            <a-tab-pane key="2" tab="出库信息">
              <a-table
                columns={this.outColumns}
                size="small"
                dataSource={this.outDatas}
                scroll={{ y: 300 }}
                pagination={false}
                bordered
                rowKey={record => record.id}
                {...{
                  scopedSlots: {
                    time: (text, record) => {
                      return record && record.outRecord && record.outRecord.time ? moment(record.outRecord.time).format("YYYY-MM-DD HH:mm:ss") : '';
                    },
                    code: (text, record, index) => {
                      return index + 1;
                    },
                  },
                }}
              ></a-table>
            </a-tab-pane>
          </a-tabs>
        </div>
      </a-modal>
    );
  },
};
