import { ModalStatus, MemberType, MaterialAcceptanceTestStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import ApiAcceptance from '../../sm-api/sm-material/Acceptance';
import FileUpload from '../../sm-file/sm-file-upload';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
import MaterialSelectModal from '../sm-material-material-select-modal';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';
import ApiMaterialOfBill from '../../sm-api/sm-material/MaterialOfBill';
import { requestIsSuccess, getModalTitle } from '../../_utils/utils';
let apiAcceptance = new ApiAcceptance();
let apiSection = new ApiSection();
let apiMaterialOfBill = new ApiMaterialOfBill();
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'constructionTeam', //施工队
  'sectionId', //施工区段
  'time', //领料时间
  'userId', //领料人
  'remark', //备注
];
export default {
  name: 'SmMaterialAcceptanceModal',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      modalTitle: '', //
      materialPlan: { materialAcceptanceRltMaterials: [] }, //用料计划
      isAcceptance: false, //是否验收
      files: [], //要上传的文件
      materialList: [],
      modalVisible: false,
      // constructionTeam: '',// 施工队
      partialSection: [], // 施工区段
      formData: {},
    };
  },
  computed: {
    columnsMaterial() {
      let columns = [
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '型号/规格',
          ellipsis: true,
          dataIndex: 'spec',
          scopedSlots: { customRender: 'spec' },
        },
        {
          title: '类别',
          ellipsis: true,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '领料量',
          ellipsis: true,
          dataIndex: 'count',
          scopedSlots: { customRender: 'count' },
        },
      ];
      let operations = {
        title: '操作',
        dataIndex: 'operations',
        scopedSlots: { customRender: 'operations' },
      };
      this.status == ModalStatus.View ? '' : columns.push(operations);
      return columns;
    },
    title() {
      // 计算模态框的标题变量
      return getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    apiMaterialOfBill = new ApiMaterialOfBill(this.axios);
    this.getPartialSection();
  },
  methods: {
    initAxios() {
      apiAcceptance = new ApiAcceptance(this.axios);
      apiSection = new ApiSection(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.isAcceptance = false;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    async view(record) {
      this.enter('View', record);
    },
    async edit(record) {
      this.enter('Edit', record);
    },
    enter(status, record) {
      this.status = ModalStatus[status];
      this.record = record;
      this.record.time = this.record ? moment(this.record.time) : null;
      let userId = [{ id: record ? record.creatorId : undefined, type: MemberType.User }];

      this.materialList = record.materialOfBillRltMaterials.map(material => {
        if (
          material &&
          material.inventory &&
          material.inventory.material &&
          material.inventory.materialId
        ) {
          return {
            id: material.inventory.materialId || '',
            inventoryId: material.inventoryId,
            materialId: material.inventory.materialId || '',
            name: material.inventory.material.name ? material.inventory.material.name : '',
            spec: material.inventory.material.spec ? material.inventory.material.spec : '',
            type:
              material.inventory.material.type && material.inventory.material.type.name
                ? material.inventory.material.type.name
                : '',
            count: material && material.count ? material.count : null,
          };
        }
      });
      // this.materialList = record ? record.materialOfBillRltMaterials : [];
      this.files =
        record &&
        record.materialOfBillRltAccessories &&
        record.materialOfBillRltAccessories.length > 0
          ? record.materialOfBillRltAccessories.map(item => item.file)
          : [];
      this.$nextTick(() => {
        this.form.setFieldsValue({
          ...utils.objFilterProps({ ...this.record, userId }, formFields),
        });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.files = [];
      this.materialPlan = null;
      this.materialList = [];
      this.isAcceptance = false;
      this.status = ModalStatus.Hide;
      this.modalVisible = false;
      this.$emit('success');
    },

    // 数据提交
    async save(isSubmit) {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // await this.$refs.fileUpload.commit();
          // let materials = this.materialList;
          // let _materials = [];
          // materials.map(async item => {
          //   _materials.push({
          //     materialId: item.materialId,
          //     testState: item.testState,
          //     count: item.count,
          //   });
          // });
          if (this.files.length > 0) {
            this.$refs.fileUpload.commit();
          }
          let MaterialOfBillRltAccessories = this.files.map(item => {
            return {
              fileId: item.id,
            };
          });
          let MaterialOfBillRltMaterials = this.materialList.map(item => {
            return {
              count: item.count,
              inventoryId: item.inventoryId,
            };
          });
          let state;
          if (this.status == ModalStatus.Add) {
            state = isSubmit ? 1 : 0;
          } else {
            state = isSubmit ? 3 : 2;
          }
          let data = {
            ...values,
            state: state,
            id: this.record.id,
            MaterialOfBillRltAccessories: MaterialOfBillRltAccessories,
            MaterialOfBillRltMaterials: MaterialOfBillRltMaterials,
          };
          let response;
          if (isSubmit) {
            // 提交数据
            if (this.status == ModalStatus.Add) {
              response = await apiMaterialOfBill.create(data);
              if (requestIsSuccess(response)) {
                this.$message.info('提交成功');
                this.close();
                this.$emit('success');
              }
            } else {
              response = await apiMaterialOfBill.update(data);
              if (requestIsSuccess(response)) {
                this.$message.info('审核已通过');
                this.close();
                this.$emit('success');
              }
            }
          } else {
            // 保存更新数据
            console.log(values);
            response = await apiMaterialOfBill.update(data);
            if (requestIsSuccess(response)) {
              this.$message.info('保存成功');
              this.close();
              this.$emit('success');
            }
          }
        }
      });
      this.loading = false;
    },
    inputChange(record, e) {
      if (e <= record.inventory.amount && e > 0) {
        record.count = e;
      } else {
        this.$message.info('领料量不在库存范围内');
      }
    },
    getDate(record) {
      let time = null;
      switch (record.TestingStatus) {
      case MaterialAcceptanceTestStatus.ForAcceptance:
        time = record.submissionTime;
        break;
      case MaterialAcceptanceTestStatus.Approved:
        time = record.receptionTime;
        break;
      default:
        break;
      }
      return time;
    },
    async submit(_status, _submissionTime, _receptionTime) {
      let data = {
        receptionTime: _receptionTime,
        submissionTime: _submissionTime,
        testingStatus: _status,
      };
      let response = await apiAcceptance.submit({ id: this.record.id, ...data });
      if (requestIsSuccess(response)) {
        this.$message.success('操作成功');
        this.close();
        this.$emit('success');
      }
    },
    // 生成编码
    generatedCode() {
      let num = '';
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      num = date.replaceAll('-', '');
      let code = 'BG-' + num;
      return code;
    },
    // 获取位置分区信息
    async getPartialSection() {
      let section = await apiSection.getTreeList({ isAll: true });
      if (requestIsSuccess(section)) {
        this.partialSection = section.data.items;
      }
    },
  },
  render() {
    let TypeOptions = [];
    for (let item of this.partialSection) {
      TypeOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
    }
    return (
      <a-modal
        class="sm-material-acceptance-modal"
        title={`${this.title}验收`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        footer={null}
        width={900}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label="施工队" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-input
                  disabled={this.status != ModalStatus.Add}
                  placeholder="请输入施工队"
                  style="width:100%"
                  v-decorator={[
                    'constructionTeam',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择施工队',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item label="施工区段" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-select
                  disabled={this.status != ModalStatus.Add}
                  placeholder="请选择施工区段"
                  style="width:100%"
                  v-decorator={[
                    'sectionId',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择施工区段',
                        },
                      ],
                    },
                  ]}
                >
                  {TypeOptions}
                </a-select>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="领料人"
                label-col={{ span: 5 }}
                wrapper-col={{ span: 19 }}
                class="acceptance-modal-select-creator"
              >
                <SmSystemMemberSelect
                  height={32}
                  userMultiple={false}
                  showUserTab={true}
                  shouIconSelect={true}
                  bordered={true}
                  simple={true}
                  placeholder="请选择领料人"
                  axios={this.axios}
                  disabled={this.status != ModalStatus.Add}
                  v-decorator={[
                    'userId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择领料人' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="领料时间" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-date-picker
                  placeholder="请选择领料时间"
                  disabled={this.status != ModalStatus.Add}
                  style="width:100%"
                  v-decorator={[
                    'time',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择领料时间',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="附件上传" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <FileUpload
                  height={65}
                  axios={this.axios}
                  ref="fileUpload"
                  fileList={this.files}
                  multiple={true}
                  onSelected={value => {
                    this.files = value;
                  }}
                  mode={this.status == ModalStatus.View ? 'view' : 'edit'}
                  download
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="备注" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-textarea
                  rows={2}
                  placeholder={this.status == ModalStatus.View ? '' : '请输入备注'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'remark',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <div style="padding-bottom: 12px;">
                <span>物资列表:</span>
                {!(this.status == ModalStatus.View) ? (
                  <span>
                    <a-button
                      style="float: right;"
                      type="primary"
                      size="small"
                      onClick={() => {
                        this.modalVisible = true;
                      }}
                    >
                      添加
                    </a-button>

                    <MaterialSelectModal
                      visible={this.modalVisible}
                      axios={this.axios}
                      onChange={(Visible, Val) => {
                        let iValue = Val.map(item => {
                          console.log(item);
                          return {
                            // 两个物资材料id同存
                            id: item.id,
                            materialId: item.id,
                            name: item.name,
                            spec: item.spec,
                            type: item && item.type ? item.type.name : '',
                            count: 1,
                          };
                        });
                        this.materialList.push(...iValue);
                        this.modalVisible = Visible;
                      }}
                    />
                  </span>
                ) : (
                  ''
                )}
              </div>
              <a-form-item>
                {/* 物资列表 */}
                <a-table
                  scroll={{ y: 300 }}
                  columns={this.columnsMaterial}
                  rowKey={record => record.id}
                  dataSource={this.materialList}
                  bordered={this.bordered}
                  size="small"
                  pagination={false}
                  {...{
                    scopedSlots: {
                      name: (text, record) => {
                        let result = record.name || '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      spec: (text, record) => {
                        let result = record.spec || '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      type: (text, record) => {
                        let result = record.type || '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      count: (text, record) => {
                        let result = record && record.count ? record.count : '';
                        if (this.status == ModalStatus.View) {
                          return (
                            <a-tooltip placement="topLeft" title={result}>
                              <span> {result}</span>
                            </a-tooltip>
                          );
                        } else {
                          return (
                            <a-input-number
                              value={result}
                              min={1}
                              size="small"
                              onChange={Val => {
                                record.count = Val;
                              }}
                            />
                          );
                        }
                      },
                      operations: (text, record, index) => {
                        return [
                          <span
                            onClick={() => {
                              this.materialList.splice(index, 1);
                            }}
                          >
                            <a-icon type="delete" style={{ fontSize: '24px', color: '#ff0000' }} />
                          </span>,
                        ];
                      },
                    },
                  }}
                ></a-table>
              </a-form-item>
            </a-col>
          </a-row>
          {!(this.status == ModalStatus.View) ? (
            <div class="action-button">
              <span>
                <a-button type="primary" size="small" onClick={() => this.close()}>
                  取消
                </a-button>
              </span>
              <span>
                {!(this.status == ModalStatus.Add) ? (
                  <a-button
                    type="primary"
                    size="small"
                    onClick={() => {
                      this.save(false);
                    }}
                  >
                    保存
                  </a-button>
                ) : (
                  ''
                )}
              </span>
              <span>
                <a-button
                  type="primary"
                  size="small"
                  disabled={
                    this.record &&
                    this.record.testingStatus == MaterialAcceptanceTestStatus.Approved
                  }
                  onClick={() => {
                    this.save(true);
                  }}
                >
                  提交
                </a-button>
              </span>
            </div>
          ) : (
            ''
          )}
        </a-form>
      </a-modal>
    );
  },
};
