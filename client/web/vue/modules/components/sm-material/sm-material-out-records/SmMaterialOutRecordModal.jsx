import * as utils from '../../_utils/utils';
import { ModalStatus, MemberType } from '../../_utils/enum';
import ApiOutRecord from '../../sm-api/sm-material/OutRecord';
import ApiComponentCategory from '../../sm-api/sm-std-basic/ComponentCategory';
import SmFileUpload from '../../sm-file/sm-file-upload';
import SmMaterialPartitalTreeSelect from '../sm-material-partital-tree-select';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import SmMaterialInventoryModal from '../sm-material-inventory-modal';
import moment from 'moment';

let apiOutRecord = new ApiOutRecord();
let apiComponentCategory = new ApiComponentCategory();

// 定义表单字段常量
const formFields = ['time', 'partitionId', 'creators', 'remark'];
export default {
  name: 'SmMaterialOutRecordModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      confirmLoading: false, //确定按钮加载状态
      materials: [],//当前出库材料清单
      components: [],//当前跟踪构件清单
      qrCode: '',//跟踪构件二维码
      fileList: [],
      iVisible: false,//是否显示库存模态框
      partitionId: null,
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },

    materialColumns() {
      return [
        {
          title: '材料名称',
          ellipsis: true,
          dataIndex: 'material.name',
        },
        {
          title: '规格型号',
          ellipsis: true,
          dataIndex: 'material.spec',
        },
        {
          title: '单位',
          ellipsis: true,
          dataIndex: 'material.unit',
        },
        {
          title: '供应商',
          dataIndex: 'supplier.name',
          ellipsis: true,
        },
        {
          title: '价格',
          ellipsis: true,
          dataIndex: 'price',
        },
        {
          title: '库存',
          ellipsis: true,
          dataIndex: 'amount',
        },
        {
          title: '出库数量',
          dataIndex: 'count',
          scopedSlots: { customRender: 'count' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
        },
        this.status === ModalStatus.View ? { width: 0 } :
          {
            title: '操作',
            dataIndex: 'operations',
            width: 90,
            scopedSlots: { customRender: 'operations' },
          },
      ];
    },

    componentColumns() {
      return [
        {
          title: '构件名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '单位',
          ellipsis: true,
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        this.status === ModalStatus.View ? { width: 0 } :
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiOutRecord = new ApiOutRecord(this.axios);
      apiComponentCategory = new ApiComponentCategory(this.axios);
    },

    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    // 详情
    async view(record) {
      this.status = ModalStatus.View;
      let response = await apiOutRecord.get(record.id);
      if (utils.requestIsSuccess(response) && response.data) {
        let _record = response.data;
        this.record = _record;
        this.record.creators = [{
          id: _record ? _record.creator.id : undefined,
          type: MemberType.User,
        }];
        this.record.time = _record.time ? moment(_record.time) : null;
        this.fileList = _record && _record.outRecordRltFiles && _record.outRecordRltFiles.length > 0 ?
          _record.outRecordRltFiles.map(item => item.file) : [];
        this.materials = _record.outRecordRltMaterials && _record.outRecordRltMaterials.length > 0 ?
          _record.outRecordRltMaterials.map(item => {
            return {
              ...item.inventory,
              count: item.count,
            };
          }) : [];
        this.components = _record.outRecordRltQRCodes;
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.fileList = [];
      this.materials = [];
      this.components = [];
      this.qrCode = '';
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.partitionId = null;
      this.confirmLoading = false;
    },

    //添加跟踪构件信息
    async addComponent() {
      if (!this.components.find(item => item.qrCode === this.qrCode)) {
        let array = this.qrCode.split('@');
        if (array.length > 0 && array[0]) {
          let response = await apiComponentCategory.getByCode(array[0]);
          if (utils.requestIsSuccess(response)) {
            let target = {
              componentCategory: response.data,
              componentCategoryId: response.data.id,
              qrCode: this.qrCode,
              children: null,
            };
            this.components.push(target);
          }
        }

      }
    },

    //删除出库材料
    removeMaterial(record) {
      let index = this.materials.indexOf(record);
      if (index > -1) {
        this.materials.splice(index, 1);
      }
    },

    //删除跟踪构件
    removeComponent(record) {
      let index = this.components.indexOf(record);
      if (index > -1) {
        this.components.splice(index, 1);
      }
    },

    //添加库存材料
    addMaterials(value) {
      this.materials = value && value.length > 0 ? value.map(item => {
        return {
          ...item,
          count: 1,
        };
      }) : [];
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          await this.$refs.fileUpload.commit();
          if (!!this.materials.find(item => item.count <= 0)) {
            this.$message.warn('出库数量不能小于零');
            return;
          }
          let response = null;
          if (this.status === ModalStatus.View) {
            this.close();
          } else if (this.status === ModalStatus.Add) {
            // 添加
            let data = {
              ...values,
              creatorId: values.creators[0].id,
              time: moment(values.time).format(),
              outRecordRltFiles: this.fileList.length > 0 ?
                this.fileList.map(item => { return { fileId: item.id }; })
                : [],
              outRecordRltMaterials: this.materials.map(item => {
                return {
                  materialId: item.materialId,
                  inventoryId: item.id,
                  count: item.count,
                  price: item.price,
                  supplierId: item.supplierId,
                  remark: item.remark,
                };
              }),
              outRecordRltQRCodes: this.components.length > 0 ?
                this.components.map(item => { return { qrCode: item.qrCode }; })
                : [],
            };
            if (data.outRecordRltMaterials.length <= 0) {
              this.$message.warn('请添加出库材料');
              return;
            }
            this.confirmLoading = true;
            response = await apiOutRecord.create(data);
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.close();
              this.$emit('success');
            }
            this.confirmLoading = false;

          }
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        class="sm-material-out-record-modal"
        title={`${this.title}出库单`}
        visible={this.visible}
        width={900}
        confirmLoading={this.confirmLoading}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form} size="small">
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label="仓库名称" label-col={{ span: 5 }} wrapper-col={{ span: 19 }} >
                <SmMaterialPartitalTreeSelect
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择仓库'}
                  onChange={value => {
                    this.partitionId = value;
                  }}
                  v-decorator={[
                    'partitionId',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择仓库',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="出库时间" label-col={{ span: 5 }} wrapper-col={{ span: 19 }} >
                <a-date-picker
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择出库时间'}
                  style="width:100%"
                  v-decorator={[
                    'time',
                    {
                      initialValue: moment(),
                      rules: [
                        {
                          required: true,
                          message: '请选择出库时间',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="录入人" label-col={{ span: 5 }} wrapper-col={{ span: 19 }} >
                <SmSystemMemberSelect
                  height={32}
                  shouIconSelect={true}
                  showUserTab={true}
                  userMultiple={false}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择登记人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'creators',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择登记人' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="附件上传" label-col={{ span: 5 }} wrapper-col={{ span: 19 }} >
                <SmFileUpload
                  ref="fileUpload"
                  disabled={this.status == ModalStatus.View}
                  mode={this.status == ModalStatus.View ? "view" : "edit"}
                  axios={this.axios}
                  multiple
                  placeholder={this.status == ModalStatus.View ? '' : '请上传附件'}
                  fileList={this.fileList}
                  onSelected={(values) => {
                    this.fileList = values;
                  }}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} >
                <a-textarea
                  rows="2"
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : '请输入备注'}
                  v-decorator={[
                    'remark',
                    {
                      initialValue: '',
                      rules: [{ max: 500, message: '备注最多输入 500 字符' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            {this.status == ModalStatus.View ? undefined :
              <a-col sm={24} md={24}>
                <a-button
                  style="float:right;margin-bottom: 10px;"
                  type="primary"
                  size="small"
                  onClick={value => this.iVisible = true}
                >
                  添加材料
                </a-button>
              </a-col>}
            <a-col sm={24} md={24}>
              <a-form-item label="出库材料" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} >
                <a-table
                  columns={this.materialColumns}
                  dataSource={this.materials}
                  rowKey={record => record.id}
                  size="small"
                  pagination={false}
                  scroll={{ y: 200 }}
                  {...{
                    scopedSlots: {
                      name: (text, record) => {
                        return record.material ? record.material.name : '';
                      },
                      spec: (text, record) => {
                        return record.material ? record.material.spec : '';
                      },
                      unit: (text, record) => {
                        return record.material ? record.material.unit : '';
                      },
                      supplier: (text, record) => {
                        return record.supplier ? record.supplier.name : '';
                      },
                      count: (text, record) => {
                        return this.status == ModalStatus.View ? text :
                          <a-input-number
                            disabled={this.status == ModalStatus.View}
                            style="margin: -10px 0"
                            size="small"
                            placeholder='请输入出库数量'
                            min={0}
                            max={record.amount}
                            precision={3}
                            value={record.count}
                            onChange={value => {
                              record.count = value;
                            }}
                          />;
                      },

                      remark: (text, record) => {
                        return this.status == ModalStatus.View ? text :
                          <a-input
                            disabled={this.status == ModalStatus.View}
                            style="margin: -10px 0"
                            placeholder='请输入备注'
                            size="small"
                            value={record.remark}
                            onChange={event => {
                              record.remark = event.target.value;
                            }}
                          />;
                      },

                      operations: (text, record) => {
                        return <a
                          onClick={() => {
                            this.removeMaterial(record);
                          }}
                        >
                          删除
                        </a>;
                      },
                    },
                  }}
                ></a-table>
              </a-form-item>
            </a-col>
            {this.status == ModalStatus.View ? undefined :
              <a-col sm={24} md={24}>
                <a-form-item label="二维码" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} >
                  <div class="entry-qrcode">
                    <a-input
                      placeholder={this.status == ModalStatus.View ? '' : '请输入或扫描二维码'}
                      value={this.qrCode}
                      onInput={event => this.qrCode = event.target.value}
                    >
                      <a-icon slot="prefix" type="scan" />
                    </a-input>
                    <a-button
                      style="margin-left:20px;"
                      type="primary"
                      size="small"
                      onClick={this.addComponent}
                    >
                      确认
                    </a-button>
                  </div>
                </a-form-item>
              </a-col>}

            <a-col sm={24} md={24}>
              <a-form-item label="跟踪构件" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} >
                <a-table
                  columns={this.componentColumns}
                  dataSource={this.components}
                  rowKey={record => record.qrCode}
                  size="small"
                  pagination={false}
                  scroll={{ y: 200 }}
                  {...{
                    scopedSlots: {
                      name: (text, record) => {
                        return record && record.componentCategory ? record.componentCategory.name : '';
                      },
                      unit: (text, record) => {
                        return record && record.componentCategory ? record.componentCategory.unit : '';
                      },
                      operations: (text, record) => {
                        return <a
                          onClick={() => {
                            this.removeComponent(record);
                          }}
                        >
                          删除
                        </a>;
                      },
                    },
                  }}
                ></a-table>
              </a-form-item>
            </a-col>
          </a-row>

        </a-form>
        <SmMaterialInventoryModal
          ref="SmMaterialInventoryModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.materials}
          size="small"
          multiple
          partitionId={this.partitionId}
          onOk={this.addMaterials}
          onChange={visible => (this.iVisible = visible)}
        />
      </a-modal >
    );
  },
};
