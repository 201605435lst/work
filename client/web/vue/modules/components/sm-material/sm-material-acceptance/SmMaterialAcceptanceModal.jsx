import {
  ModalStatus,
  MemberType,
  MaterialAcceptanceTypeEnable,
  TestingResultType,
  MaterialAcceptanceTestStatus,
} from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import ApiAcceptance from '../../sm-api/sm-material/Acceptance';
import ApiComponentCategory from '../../sm-api/sm-std-basic/ComponentCategory';
import FileUpload from '../../sm-file/sm-file-upload';
import DataDictionary from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
import MaterialSelectModal from '../sm-material-material-select-modal';
import MaterialPurchaseListModal from '../sm-material-purchase-list-select/SmMaterialPurchaseListSelect';
import {
  requestIsSuccess,
  getMaterialAcceptanceTypeEnable,
  getModalTitle,
  getTestingResultType,
} from '../../_utils/utils';
let apiAcceptance = new ApiAcceptance();
let apiComponentCategory = new ApiComponentCategory();
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'testingOrganizationId', //检测机构
  'code', //报告编号
  'testingType', //检测类型
  'receptionTime', //验收日期
  'creatorId',//登记人
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
      files: [],//要上传的文件
      modalVisible: false,
      materialList: [],
      TracklList: [],
      qrCode: '',//跟踪构件二维码
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
          title: '数量',
          ellipsis: true,
          dataIndex: 'count',
          scopedSlots: { customRender: 'count' },
        },
        {
          title: '检测结果',
          dataIndex: 'testResult',
          width: 200,
          ellipsis: true,
          scopedSlots: { customRender: 'testResult' },
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
    columnsTrack() {
      let columns = [
        {
          title: '构件名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '单位',
          dataIndex: 'unit',
          ellipsis: true,
          scopedSlots: { customRender: 'unit' },
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
  },
  methods: {
    initAxios() {
      apiAcceptance = new ApiAcceptance(this.axios);
      apiComponentCategory = new ApiComponentCategory(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.isAcceptance = false;
      this.$nextTick(() => {
        this.form.resetFields();
        let code = this.generatedCode();
        this.form.setFieldsValue({ code: code });
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
      this.record.creatorId = [{ id: record ? record.creatorId : undefined, type: MemberType.User }];
      this.isAcceptance =
        record && record.testingStatus == MaterialAcceptanceTestStatus.ForAcceptance ? true : false;
      this.record.receptionTime = this.record ? moment(this.record.receptionTime) : null;
      this.materialPlan = record ? record : '';
      this.materialList = record ? record.materialAcceptanceRltMaterials : [];
      this.TracklList = record ? record.materialAcceptanceRltQRCodes : [];
      this.files = this.materialPlan && this.materialPlan.materialAcceptanceRltFiles && this.materialPlan.materialAcceptanceRltFiles.length > 0 ? this.materialPlan.materialAcceptanceRltFiles.map(item => item.file) : [];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.files = [];
      this.materialPlan = null;
      this.materialList = [];
      this.TracklList = [];
      this.isAcceptance = false;
      this.status = ModalStatus.Hide;
      this.modalVisible = false;
      this.$emit('success');
    },

    // 数据提交
    save(isSubmit) {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          await this.$refs.fileUpload.commit();
          let materials = this.materialList;
          let Trackls = this.TracklList;
          let _materials = [];
          let _Trackls = [];
          materials.map(async item => {
            _materials.push({
              materialId: item.materialId,
              testState: item.testState,
              number: item.number,
            });
          });
          Trackls.map(async item => {
            _Trackls.push({
              qrCode: item.qrCode,
            });
          });

          let data = {
            ...values,
            creatorId: Object.assign(...values.creatorId) ? Object.assign(...values.creatorId).id : '',
            testingStatus: !isSubmit ? MaterialAcceptanceTestStatus.ForAcceptance : MaterialAcceptanceTestStatus.Approved,
            materialAcceptanceRltFiles: this.files.length > 0 ? this.files.map(item => { return { fileId: item.id }; }) : [],
            materialAcceptanceRltMaterials: _materials,
            materialAcceptanceRltQRCodes: _Trackls,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiAcceptance.create(data);
          }
          if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiAcceptance.update({ id: this.record.id, ...data });
          }
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
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
      default: ; break;
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
    testResultState(res) {
      let tar = null;
      switch (res) {
      case TestingResultType.Qualified:
        tar = <a-tag color="green">合格</a-tag>;
        break;
      case TestingResultType.Disqualification:
        tar = <a-tag color="red">不合格</a-tag>;
        break;
      default: ; break;
      }
      return tar;
    },
    //添加采购清单
    addMaterials(Val) {
      let purchaseList = Val ? Val : [];
      purchaseList = purchaseList.map(item => {
        return {
          ...item,
          number: item.number <= 0 || item.number == undefined || item.number == null ? 1 : item.number,
          testState: 2,
        };
      });
      purchaseList.forEach(item => {
        let isTrue = false;
        this.materialList.forEach(item_ => {
          if (item.id === item_.id) { isTrue = true; };
        });
        isTrue ? '' : this.materialList.push(item);
      });
    },
    //添加跟踪构件信息
    async addComponent() {
      if (!this.TracklList.find(item => item.qrCode === this.qrCode)) {
        let array = this.qrCode.split('@');
        if (array.length > 0 && array[0]) {
          let response = await apiComponentCategory.getByCode(array[0]);
          if (utils.requestIsSuccess(response)) {
            let target = {
              qrCode: this.qrCode,
              componentCategory: {
                name: response.data.name,
                unit: response.data.unit,
              },
            };
            this.TracklList.push(target);
          }
        }

      }
    },
  },
  render() {
    let TypeOptions = [];
    for (let item in MaterialAcceptanceTypeEnable) {
      TypeOptions.push(
        <a-select-option key={MaterialAcceptanceTypeEnable[item]}>
          {getMaterialAcceptanceTypeEnable(MaterialAcceptanceTypeEnable[item])}
        </a-select-option>,
      );
    };
    let TestOptions = [];
    for (let item in TestingResultType) {
      TestOptions.push(
        <a-select-option key={TestingResultType[item]}>
          {getTestingResultType(TestingResultType[item])}
        </a-select-option>,
      );
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
              <a-form-item label="采购清单" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <MaterialPurchaseListModal
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : "请选择采购清单(多选)"}
                  onChange={Val => { this.addMaterials(Val); }}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="报告编号" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-input
                  disabled={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请输入报告编号'}
                  style="width:100%"
                  v-decorator={[
                    'code',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请输入报告编号',
                          whitespace: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="登记人" label-col={{ span: 5 }} wrapper-col={{ span: 19 }} class="acceptance-modal-select-creator">
                <SmSystemMemberSelect
                  height={32}
                  userMultiple={false}
                  showUserTab={true}
                  shouIconSelect={true}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择登记人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'creatorId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择登记人' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item label="检测机构" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <DataDictionary
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择检测机构'}
                  groupCode={'TestingOrganization'}
                  style="width:100%"
                  v-decorator={[
                    'testingOrganizationId',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择检测机构',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>


            <a-col sm={12} md={12}>
              <a-form-item label="检测类型" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-select
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择检测类型'}
                  style="width:100%"
                  onChange={value => {
                    this.record && this.record.testingType ? this.record.testingType = value : '';
                  }}
                  v-decorator={[
                    'testingType',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择检测类型',
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
              <a-form-item label="报告日期" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-date-picker
                  placeholder={this.status == ModalStatus.View ? '' : '请选择报告日期'}
                  disabled={this.status == ModalStatus.View}
                  style="width:100%"
                  v-decorator={[
                    'receptionTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择报告日期',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="资料上传" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
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
                          return {
                            // 两个物资材料id同存
                            id: item.id + this.generatedCode(),
                            materialId: item.id,
                            material: { name: item.name, spec: item.spec, type: { name: item.type.name } },
                            number: 1,
                            testState: 2,
                          };
                        });
                        this.materialList.push(...iValue);
                        this.modalVisible = Visible;
                      }}
                    />
                  </span>
                ) : ''}
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
                        let result = record && record.material ? record.material.name : '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      spec: (text, record) => {
                        let result = record && record.material ? record.material.spec : '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      type: (text, record) => {
                        let result =
                          record && record.material && record.material.type
                            ? record.material.type.name
                            : '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },

                      count: (text, record) => {
                        let result = record ? record.number : '';
                        if (this.status == ModalStatus.View) {
                          return (
                            <a-tooltip placement="topLeft" title={result}>
                              <span> {result}</span>
                            </a-tooltip>
                          );
                        } else {
                          return (
                            <a-input-number value={result} min={1} size="small" onChange={Val => { record.number = Val; }} />
                          );
                        }
                      },
                      testResult: (text, record) => {
                        let result;
                        if (this.status == ModalStatus.Add || this.status == ModalStatus.Edit) {
                          result = (
                            <div>
                              <a-select
                                placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                                style="width:100px"
                                size="small"
                                defaultValue={record.testState}
                                onChange={value => {
                                  record.testState = value;
                                }}
                              >
                                {TestOptions}
                              </a-select>
                            </div>
                          );
                        } else {
                          result = this.testResultState(record.testState);
                        }

                        return <span> {result}</span>;
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
              {this.status == ModalStatus.View ?
                <div style="padding-bottom: 12px;">构件跟踪:</div> :
                <a-col sm={24} md={24} class='entry-qrcode'>
                  <a-form-item label="构件跟踪" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} >
                    <div class="entry-qrcode-item">
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
              <a-form-item>
                {/* 跟踪构件 */}
                <a-table
                  scroll={{ y: 300 }}
                  columns={this.columnsTrack}
                  rowKey={records => records.id}
                  dataSource={this.TracklList}
                  size="small"
                  bordered={this.bordered}
                  pagination={false}
                  {...{
                    scopedSlots: {
                      name: (text, record) => {
                        let result = record && record.componentCategory ? record.componentCategory.name : '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      unit: (text, record) => {
                        let result = record && record.componentCategory && record.componentCategory.unit ? record.componentCategory.unit.name : '';
                        return (
                          <a-tooltip placement="topLeft" title={result}>
                            <span> {result}</span>
                          </a-tooltip>
                        );
                      },
                      operations: (text, record, index) => {
                        return [
                          <span
                            onClick={() => {
                              this.TracklList.splice(index, 1);
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
                {!(this.status == ModalStatus.View) ? (
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
                    this.record && this.record.testingStatus == MaterialAcceptanceTestStatus.Approved
                  }
                  onClick={() => {
                    this.save(true);
                  }}
                >
                  提交
                </a-button>
              </span>
            </div>
          ) : ''}
        </a-form>
      </a-modal>
    );
  },
};
