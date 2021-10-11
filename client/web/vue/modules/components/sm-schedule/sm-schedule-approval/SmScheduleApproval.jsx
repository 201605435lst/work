import './style';
import { requestIsSuccess, objFilterProps, CreateGuid } from '../../_utils/utils';
import ApiApproval from '../../sm-api/sm-schedule/Approval';
import ApiMaterial from '../../sm-api/sm-material/Material';
import ApiOrganization from '../../sm-api/sm-system/Organization';
import ApiAccount from '../../sm-api/sm-system/Account';
import { PageState } from '../../_utils/enum';
import moment from 'moment';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmSystemMenbersSelect from '../../sm-system/sm-system-member-select';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ScheduleSelect from '../sm-schedule-schedules-select';

let apiApproval = new ApiApproval();
let apiMaterial = new ApiMaterial();
let apiOrganization = new ApiOrganization();
let apiAccount = new ApiAccount();

const formValues = [
  'organization', //施工单位 --基本信息
  'code', //编号
  'time', //施工日期
  'scheduleId', //施工计划
  'professionId', //施工专业 --
  'location', //施工部位 --
  'name', //任务名称 --
  'director', //负责人      'approvalRltMembers', //负责人+施工员
  'builders', //施工员
  'memberNum', //劳务人员个数
  'approvalRltFiles', //技术资料 --辅助信息
  'temporaryEquipmentId', //临时设施
  'safetyCautionId', //安全注意事项
  'remark', //备注import
];

export default {
  name: 'SmScheduleApproval',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    pageState: { type: String, default: PageState.Add },
  },
  data() {
    return {
      form: {},
      directorId: [], //授权责任用户
      buildersIds: [], //授权抄送用户
      physicalList: {
        //物资信息
        auxiliaryMaterials: [], //辅材
        appliances: [], //器具
        mechanics: [], //机械
        securityProducts: [], //安全防护用品
      },
      approvalRltMaterials: [], //关联物资信息
      listMessage: [], //物资表格数据，选择哪个tab，就是哪个的数据
      materialList: [], //添加和编辑模式下，查询到的所有物资。
      loading: false,
      physicalType: 'auxiliaryMaterials', //物资信息类型
      scheduleInfo: null, //所选计划信息
    };
  },
  computed: {
    isShow() {
      return this.pageState == PageState.View;
    },
    columns() {
      return this.physicalType == 'auxiliaryMaterials'
        ? [
          {
            title: '#',
            dataIndex: 'index',
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '材料名称',
            dataIndex: 'materialName',
            width: 380,
            scopedSlots: { customRender: 'materialName' },
          },
          {
            title: '规格型号',
            dataIndex: 'model',
            width: 340,
            scopedSlots: { customRender: 'model' },
          },
          {
            title: '单位',
            dataIndex: 'unit',
            scopedSlots: { customRender: 'unit' },
          },
          {
            title: '数量',
            dataIndex: 'number',
            scopedSlots: { customRender: 'number' },
          },
          {
            title: '操作',
            dataIndex: 'operations',
            width: 169,
            scopedSlots: { customRender: 'operations' },
            fixed: 'right',
          },
        ]
        : this.physicalType == 'appliances'
          ? [
            {
              title: '#',
              dataIndex: 'index',
              scopedSlots: { customRender: 'index' },
            },
            {
              title: '使用器具',
              dataIndex: 'appliance',
              width: 400,
              scopedSlots: { customRender: 'appliance' },
            },
            {
              title: '单位',
              dataIndex: 'unit',
              scopedSlots: { customRender: 'unit' },
            },
            {
              title: '数量',
              dataIndex: 'number',
              scopedSlots: { customRender: 'number' },
            },
            {
              title: '操作',
              dataIndex: 'operations',
              width: 169,
              scopedSlots: { customRender: 'operations' },
              fixed: 'right',
            },
          ]
          : this.physicalType == 'mechanics'
            ? [
              {
                title: '#',
                dataIndex: 'index',
                scopedSlots: { customRender: 'index' },
              },
              {
                title: '使用机械',
                dataIndex: 'mechanical',
                width: 400,
                scopedSlots: { customRender: 'mechanical' },
              },
              {
                title: '单位',
                dataIndex: 'unit',
                scopedSlots: { customRender: 'unit' },
              },
              {
                title: '数量',
                dataIndex: 'number',
                scopedSlots: { customRender: 'number' },
              },
              {
                title: '操作',
                dataIndex: 'operations',
                width: 169,
                scopedSlots: { customRender: 'operations' },
                fixed: 'right',
              },
            ]
            : [
              {
                title: '#',
                dataIndex: 'index',
                scopedSlots: { customRender: 'index' },
              },
              {
                title: '安全防护用品',
                dataIndex: 'safetyArticles',
                width: 400,
                scopedSlots: { customRender: 'safetyArticles' },
              },
              {
                title: '单位',
                dataIndex: 'unit',
                scopedSlots: { customRender: 'unit' },
              },
              {
                title: '数量',
                dataIndex: 'number',
                scopedSlots: { customRender: 'number' },
              },
              {
                title: '操作',
                dataIndex: 'operations',
                width: 169,
                scopedSlots: { customRender: 'operations' },
                fixed: 'right',
              },
            ];
    },
    relevantDataColumns() {
      return [
        {
          title: '#',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '模型名称',
          dataIndex: 'modelName',
          width: 380,
          scopedSlots: { customRender: 'modelName' },
        },
        {
          title: '所属站点',
          dataIndex: 'station',
          width: 380,
          scopedSlots: { customRender: 'station' },
        },
        {
          title: '图纸名称',
          dataIndex: 'drawingName',
          width: 380,
          scopedSlots: { customRender: 'drawingName' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
        },
      ];
    },
  },
  watch: {
    id: {
      handler: function (value, oldValue) {
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
    },
    pageState: {
      handler: function (value, oldValue) {
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.refresh();
    if (!this.isShow) {
      this.getMaterialData();
    }
  },
  methods: {
    initAxios() {
      apiApproval = new ApiApproval(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
      apiOrganization = new ApiOrganization(this.axios);
      apiAccount = new ApiAccount(this.axios);
    },
    async refresh() {
      if (this.pageState !== PageState.Add) {
        let response = await apiApproval.get(this.id);
        if (requestIsSuccess(response)) {
          let _approvals = response.data;
          if (_approvals.approvalRltMaterials.length > 0) {
            this.addPhysicalInfo(_approvals.approvalRltMaterials);
          }

          let _builders = [];
          let _director = [];
          _approvals.approvalRltMembers.map(item => {
            if (item.type == 3) {
              _builders.push({ id: item.memberId, type: 3 });
            } else if (item.type == 2) {
              _director.push({ id: item.memberId, type: 3 });
            }
          });

          let approvals = {
            ..._approvals,
            time:
              moment(_approvals.time).format('YYYY-MM-DD') != '0001-01-01'
                ? moment(_approvals.time)
                : null,
            director: _director,
            builders: _builders,
            temporaryEquipmentId: _approvals.temporaryEquipment
              ? _approvals.temporaryEquipmentId
              : null,
            safetyCautionId: _approvals.safetyCaution ? _approvals.temporaryEquipmentId : null,
            approvalRltFiles: _approvals.approvalRltFiles.map(item => {
              return item.file;
            }),
          };

          this.$nextTick(() => {
            let values = objFilterProps(approvals, formValues);
            this.form.setFieldsValue(values);
          });
        }
      } else {
        let response = await apiAccount.getAppConfig();
        if (requestIsSuccess(response)) {
          let resOrganization = await apiOrganization.getCurrentUserOrganizations(
            response.data.currentUser.id,
          );
          let resCode = await apiApproval.getCode();
          if (requestIsSuccess(resOrganization) && requestIsSuccess(resCode)) {
            this.$nextTick(() => {
              console.log(resOrganization);
              this.form.setFieldsValue({ organization: resOrganization.data.length > 0 ? resOrganization.data[0].name : '' });
              this.form.setFieldsValue({ code: resCode.data });
            });
          } else {
            this.$message.error('加载失败');
          }
        }
      }
    },

    async getMaterialData() {
      let response = await apiMaterial.getAll();
      if (requestIsSuccess(response) && response.data) {
        this.materialList = response.data.items;
      }
    },

    getInfo(id, record) {
      let info = this.materialList.filter(item => item.id == id);
      if (this.physicalType == 'auxiliaryMaterials') {
        this.listMessage.map(item => {
          if (item.key == record.key) {
            record.materialName = info[0].name;
            item.model = info[0].spec + ' ' + info[0].model;
            item.unit = info[0].unit;
          }
        });
      } else if (this.physicalType == 'appliances') {
        this.listMessage.map(item => {
          if (item.key == record.key) {
            record.appliance = info[0].name;
            item.unit = info[0].unit;
          }
        });
      } else if (this.physicalType == 'mechanics') {
        this.listMessage.map(item => {
          if (item.key == record.key) {
            record.mechanical = info[0].name;
            item.unit = info[0].unit;
          }
        });
      } else {
        this.listMessage.map(item => {
          if (item.key == record.key) {
            record.safetyArticles = info[0].name;
            item.unit = info[0].unit;
          }
        });
      }
    },

    save(isSubmit) {
      this.form.validateFields(async (error, value) => {
        if (!error) {
          let _values = value;
          _values.director.map(item => this.directorId.push({ id: item.id, type: 2 }));
          _values.builders.map(item => this.buildersIds.push({ id: item.id, type: 3 }));

          this.physicalList.auxiliaryMaterials.map(item => {
            this.approvalRltMaterials.push({ ...item, specModel: item.model });
          });
          this.physicalList.appliances.map(item => {
            this.approvalRltMaterials.push({ ...item, materialName: item.appliance });
          });
          this.physicalList.mechanics.map(item => {
            this.approvalRltMaterials.push({ ...item, materialName: item.mechanical });
          });
          this.physicalList.securityProducts.map(item => {
            this.approvalRltMaterials.push({ ...item, materialName: item.safetyArticles });
          });
          let data = {
            ..._values,
            approvalRltMembers: this.directorId.concat(this.buildersIds),
            approvalRltFiles: [],
            approvalRltMaterials: this.approvalRltMaterials,
            state: isSubmit ? 2 : 1,
          };
          this.loading = true;
          let response;
          if (this.pageState == PageState.Add) {
            response = await apiApproval.create(data);
          } else {
            response = await apiApproval.update({ ...data, id: this.id });
          }

          if (requestIsSuccess(response)) {
            this.$message.info('操作成功');
            this.form.resetFields();
            this.emptyData();
            this.$emit('ok');
          }
        }
        this.loading = false;
      });
    },

    isSubmit() {
      let _this = this;
      this.$confirm({
        title: '提交任务',
        content: h => <div style="color:red;">{'确定直接提交此审核信息吗？'}</div>,
        okType: 'warning',
        onOk() {
          _this.save(true);
        },
        onCancel() { },
      });
    },

    setSchedule(value) {
      this.scheduleInfo = value;
      if (value) {
        this.form.setFieldsValue({ professionId: value[0].professionId });
        this.form.setFieldsValue({ location: value[0].location });
        this.form.setFieldsValue({ name: value[0].name });
      } else {
        this.form.setFieldsValue({ professionId: null });
        this.form.setFieldsValue({ location: null });
        this.form.setFieldsValue({ name: null });
      }
    },

    addPhysicalInfo(value) {
      let newPhysicalDataList = [];
      if (value !== null) {
        value.map(item => {
          if (item.type == 1) {
            this.physicalList.auxiliaryMaterials.push({
              key: CreateGuid(),
              materialName: item.materialName,
              model: item.specModel,
              unit: item.unit,
              number: item.number,
              type: 1, //对应后端固定物资类型（4种）
            });
          } else if (item.type == 2) {
            this.physicalList.appliances.push({
              key: CreateGuid(),
              appliance: item.materialName,
              unit: item.unit,
              number: item.number,
              type: 2, //对应后端固定物资类型（4种）
            });
          } else if (item.type == 3) {
            this.physicalList.mechanics.push({
              key: CreateGuid(),
              mechanical: item.materialName,
              unit: item.unit,
              number: item.number,
              type: 3, //对应后端固定物资类型（4种）
            });
          } else {
            this.physicalList.securityProducts.push({
              key: CreateGuid(),
              safetyArticles: item.materialName,
              unit: item.unit,
              number: item.number,
              type: 4, //对应后端固定物资类型（4种）
            });
          }
        });
      } else {
        //空白添加
        newPhysicalDataList =
          this.physicalType == 'auxiliaryMaterials'
            ? [
              {
                key: CreateGuid(),
                materialName: '', //这个
                model: '',
                unit: '',
                number: 1,
                type: 1, //对应后端固定物资类型（4种）
              },
            ]
            : this.physicalType == 'appliances'
              ? [
                {
                  key: CreateGuid(),
                  appliance: '',
                  unit: '',
                  number: 1,
                  type: 2,
                },
              ]
              : this.physicalType == 'mechanics'
                ? [
                  {
                    key: CreateGuid(),
                    mechanical: '',
                    unit: '',
                    number: 1,
                    type: 3,
                  },
                ]
                : [
                  {
                    key: CreateGuid(),
                    safetyArticles: '',
                    unit: '',
                    number: 1,
                    type: 4,
                  },
                ];
      }
      if (this.physicalType == 'auxiliaryMaterials') {
        this.physicalList.auxiliaryMaterials = [
          ...this.physicalList.auxiliaryMaterials,
          ...newPhysicalDataList,
        ];
        this.listMessage = this.physicalList.auxiliaryMaterials;
      } else if (this.physicalType == 'appliances') {
        this.physicalList.appliances = [...this.physicalList.appliances, ...newPhysicalDataList];
        this.listMessage = this.physicalList.appliances;
      } else if (this.physicalType == 'mechanics') {
        this.physicalList.mechanics = [...this.physicalList.mechanics, ...newPhysicalDataList];
        this.listMessage = this.physicalList.mechanics;
      } else {
        this.physicalList.securityProducts = [
          ...this.physicalList.securityProducts,
          ...newPhysicalDataList,
        ];
        this.listMessage = this.physicalList.securityProducts;
      }
    },

    deletePhysical(selectedRowKeys) {
      if (this.physicalType == 'auxiliaryMaterials') {
        selectedRowKeys.map(id => {
          this.physicalList.auxiliaryMaterials = this.physicalList.auxiliaryMaterials.filter(
            item => item.key != id,
          );
        });
        this.listMessage = this.physicalList.auxiliaryMaterials;
      } else if (this.physicalType == 'appliances') {
        selectedRowKeys.map(id => {
          this.physicalList.appliances = this.physicalList.appliances.filter(
            item => item.key != id,
          );
        });
        this.listMessage = this.physicalList.appliances;
      } else if (this.physicalType == 'mechanics') {
        selectedRowKeys.map(id => {
          this.physicalList.mechanics = this.physicalList.mechanics.filter(item => item.key != id);
        });
        this.listMessage = this.physicalList.mechanics;
      } else {
        selectedRowKeys.map(id => {
          this.physicalList.securityProducts = this.physicalList.securityProducts.filter(
            item => item.key != id,
          );
        });
        this.listMessage = this.physicalList.securityProducts;
      }
    },

    tabChange(activeKey) {
      this.physicalType = activeKey;
      this.listMessage =
        activeKey == 'auxiliaryMaterials'
          ? this.physicalList.auxiliaryMaterials
          : activeKey == 'appliances'
            ? this.physicalList.appliances
            : activeKey == 'mechanics'
              ? this.physicalList.mechanics
              : this.physicalList.securityProducts;
    },

    //关闭单页
    cancel() {
      this.form.resetFields();
      this.emptyData();
      this.$emit('cancel');
    },
    emptyData() {
      this.directorId = [];
      this.buildersIds = [];
      this.physicalList.auxiliaryMaterials = [];
      this.physicalList.appliances = [];
      this.physicalList.mechanics = [];
      this.physicalList.securityProducts = [];
      this.approvalRltMaterials = [];
      this.listMessage = [];
    },
  },
  render() {
    //辅助材料
    let auxiliaryOptions = [];
    this.materialList.map(item => {
      if (item.type.category == 1) {
        auxiliaryOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
    });
    //使用器具
    let applianceOptions = [];
    this.materialList.map(item => {
      if (item.type.category == 2) {
        applianceOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
    });
    //使用机械
    let mechanicalOptions = [];
    this.materialList.map(item => {
      if (item.type.category == 3) {
        mechanicalOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
    });
    //安全防护用品
    let safetyArticleOptions = [];
    this.materialList.map(item => {
      if (item.type.category == 4) {
        safetyArticleOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
    });

    let dataMessage = (
      <div class="dataTable">
        <a-table
          columns={this.relevantDataColumns}
          dataSource={[]}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1;
              },
              modelName: (text, record) => {
                return [];
              },
              station: (text, record) => {
                return [];
              },
              drawingName: (text, record) => {
                return [];
              },
              operations: (text, record, index) => {
                return [
                  <a onClick={() => this.deletePhysical([record.key])}>
                    <a-icon type="delete" style="color: red;fontSize: 16px;" />
                  </a>,
                ];
              },
            },
          }}
        ></a-table>
      </div>
    );
    let physicalMessage = (
      <div class="physicalTable">
        <a-button
          type="primary"
          icon="plus"
          size="small"
          shape="round"
          class="physicalButton"
          onClick={() => this.addPhysicalInfo(null)}
          disabled={this.isShow}
        >
          新增
        </a-button>
        <a-table
          columns={this.columns}
          dataSource={this.listMessage}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1;
              },
              materialName: (text, record) => {
                return [
                  <a-select
                    axios={this.axios}
                    placeholder={this.isShow ? '' : '请输入材料名称'}
                    value={record.materialName}
                    onChange={value => {
                      this.getInfo(value, record);
                    }}
                  >
                    {auxiliaryOptions}
                  </a-select>,
                ];
              },
              model: (text, record) => {
                return [
                  <a-input
                    placeholder={this.isShow ? '' : '请输入规格型号'}
                    value={record.model}
                    onChange={event => {
                      record.model = event.target.value;
                    }}
                  ></a-input>,
                ];
              },
              unit: (text, record) => {
                return [
                  <a-input
                    placeholder={this.isShow ? '' : '请输入单位'}
                    value={record.unit}
                    onChange={event => {
                      record.unit = event.target.value;
                    }}
                  ></a-input>,
                ];
              },
              number: (text, record) => {
                return [
                  <a-input-number
                    placeholder={this.isShow ? '' : '请输入数量'}
                    min={1}
                    precision={0} //精度为0，只能为整数
                    value={record.number}
                    onChange={value => {
                      record.number = value;
                    }}
                  ></a-input-number>,
                ];
              },
              appliance: (text, record) => {
                return [
                  <a-select
                    axios={this.axios}
                    placeholder={this.isShow ? '' : '请输入使用器具名称'}
                    value={record.appliance}
                    onChange={value => {
                      this.getInfo(value, record);
                    }}
                  >
                    {applianceOptions}
                  </a-select>,
                ];
              },
              mechanical: (text, record) => {
                return [
                  <a-select
                    axios={this.axios}
                    placeholder={this.isShow ? '' : '请输入使用机械名称'}
                    value={record.mechanical}
                    onChange={value => {
                      this.getInfo(value, record);
                    }}
                  >
                    {mechanicalOptions}
                  </a-select>,
                ];
              },
              safetyArticles: (text, record) => {
                return [
                  <a-select
                    axios={this.axios}
                    placeholder={this.isShow ? '' : '请输入安全防护用品名称'}
                    value={record.safetyArticles}
                    onChange={value => {
                      this.getInfo(value, record);
                    }}
                  >
                    {safetyArticleOptions}
                  </a-select>,
                ];
              },
              operations: (text, record, index) => {
                return [
                  <a onClick={() => this.deletePhysical([record.key])}>
                    <a-icon type="delete" style="color: red;fontSize: 16px;" />
                  </a>,
                ];
              },
            },
          }}
        ></a-table>
      </div>
    );
    return (
      <div class="sm-schedule-approval">
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-tabs defaultActiveKey="basic">
              <a-tab-pane key="basic" tab="基本信息">
                <a-col sm={12} md={12}>
                  <a-form-item label="施工单位" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      placeholder={this.PageState == PageState.View ? '' : '请输入施工单位'}
                      disabled={this.isShow}
                      v-decorator={[
                        'organization',
                        {
                          initialValue: '',
                          rules: [
                            {
                              required: true,
                              message: '请输入施工单位',
                              whitespace: true,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="编号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      placeholder={this.PageState == PageState.View ? '' : '请输入编号'}
                      disabled={true}
                      v-decorator={[
                        'code',
                        {
                          initialValue: '',
                          rules: [
                            {
                              required: true,
                              message: '请输入编号',
                              whitespace: true,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="施工日期" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-date-picker
                      style="width:100%"
                      format="YYYY-MM-DD"
                      disabled={this.isShow}
                      disabledDate={current => {
                        return current < Date.now() - 8.64e7;
                      }}
                      v-decorator={[
                        'time',
                        {
                          initialValue: null,
                          rules: [
                            {
                              required: true,
                              message: '请选择施工日期时间',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>

                <a-col sm={12} md={12} class="schedule">
                  <a-form-item label="施工计划" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <ScheduleSelect
                      axios={this.axios}
                      placeholder={this.isShow ? '' : '请选择施工计划'}
                      disabled={this.isShow}
                      multiple={false}
                      height={33}
                      margin={[4, 0, 0, 0]}
                      onInput={value => this.setSchedule(value)}
                      v-decorator={[
                        'scheduleId',
                        {
                          initialValue: undefined, //这块要为undefined，为[]会报错
                          rules: [{ required: true, message: '请选择施工计划' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="所属专业" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <DataDictionaryTreeSelect
                      axios={this.axios}
                      groupCode={'Profession'}
                      disabled={this.isShow}
                      placeholder={this.isShow ? '' : '请选择故障等级'}
                      v-decorator={[
                        'professionId',
                        {
                          initialValue: null,
                          rules: [{ required: true, message: '请选择所属专业' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="施工部位" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      placeholder={this.isShow ? '' : '请输入施工部位'}
                      disabled={this.isShow}
                      v-decorator={[
                        'location',
                        {
                          initialValue: '',
                          rules: [{ required: true, message: '请输入施工部位', whitespace: true }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item label="任务名称" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <a-input
                      placeholder={this.isShow ? '' : '请输入任务名称'}
                      disabled={this.isShow}
                      v-decorator={[
                        'name',
                        {
                          initialValue: '',
                          rules: [
                            {
                              required: true,
                              message: '请输入任务名称',
                              whitespace: true,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="负责人" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <SmSystemMenbersSelect
                      axios={this.axios}
                      showUserTab={true}
                      placeholder={'请选择用户'}
                      disabled={this.isShow}
                      v-decorator={[
                        'director',
                        {
                          initialValue: [],
                          rules: [
                            {
                              required: true,
                              message: '请选择负责人',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="劳务人员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input-number
                      placeholder={this.isShow ? '' : '请输入劳务人员个数'}
                      disabled={this.isShow}
                      min={0}
                      v-decorator={[
                        'memberNum',
                        {
                          initialValue: null,
                          rules: [
                            {
                              required: true,
                              message: '请输入劳务人员个数',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item label="施工员" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <SmSystemMenbersSelect
                      axios={this.axios}
                      showUserTab={true}
                      placeholder={'请选择用户'}
                      disabled={this.isShow}
                      v-decorator={[
                        'builders',
                        {
                          initialValue: [],
                          rules: [
                            {
                              required: true,
                              message: '请选择施工员',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item label="关联资料" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <a-tabs type="card">
                      <a-tab-pane key="drawing" tab="图纸">
                        {dataMessage}
                      </a-tab-pane>
                      <a-tab-pane key="equipment" tab="设备">
                        {dataMessage}
                      </a-tab-pane>
                      <a-tab-pane key="engineering" tab="隐蔽工程">
                        {dataMessage}
                      </a-tab-pane>
                    </a-tabs>
                  </a-form-item>
                </a-col>
              </a-tab-pane>
            </a-tabs>
            <a-tabs defaultActiveKey="auxiliary">
              <a-tab-pane key="auxiliary" tab="辅助信息">
                <a-col sm={24} md={24}>
                  <a-form-item label="技术资料" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <SmFileManageSelect
                      class="ant-input"
                      axios={this.axios}
                      disabled={this.isShow}
                      //multiple={false}
                      enableDownload={true}
                      height={40}
                      bordered={true}
                      v-decorator={[
                        'approvalRltFiles',
                        {
                          initialValue: [],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item label="临时设施" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <DataDictionaryTreeSelect
                      axios={this.axios}
                      groupCode={'Profession'}
                      disabled={this.isShow}
                      placeholder={this.isShow ? '' : '请选择临时设施'}
                      v-decorator={[
                        'temporaryEquipmentId',
                        {
                          initialValue: null,
                          //rules: [{ required: true, message: '请选择临时设施' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item
                    label="安全注意事项"
                    label-col={{ span: 2 }}
                    wrapper-col={{ span: 22 }}
                  >
                    <DataDictionaryTreeSelect
                      axios={this.axios}
                      groupCode={'Profession'}
                      disabled={this.isShow}
                      placeholder={this.isShow ? '' : '请选择安全注意事项'}
                      v-decorator={[
                        'safetyCautionId',
                        {
                          initialValue: null,
                          //rules: [{ required: true, message: '请选择安全注意事项' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <a-textarea
                      axios={this.axios}
                      disabled={this.isShow}
                      placeholder={this.isShow ? '' : '请输入备注'}
                      v-decorator={[
                        'remark',
                        {
                          initialValue: null,
                          //rules: [{ required: true, message: '请输入备注' }],
                        },
                      ]}
                    ></a-textarea>
                  </a-form-item>
                </a-col>
              </a-tab-pane>
            </a-tabs>
            <a-tabs defaultActiveKey="physical">
              <a-tab-pane key="physical" tab="物资信息">
                <a-tabs type="card" onChange={activeKey => this.tabChange(activeKey)}>
                  <a-tab-pane key="auxiliaryMaterials" tab="辅材">
                    {physicalMessage}
                  </a-tab-pane>
                  <a-tab-pane key="appliances" tab="器具">
                    {physicalMessage}
                  </a-tab-pane>
                  <a-tab-pane key="mechanics" tab="机械">
                    {physicalMessage}
                  </a-tab-pane>
                  <a-tab-pane key="securityProducts" tab="安防用品">
                    {physicalMessage}
                  </a-tab-pane>
                </a-tabs>
              </a-tab-pane>
            </a-tabs>
          </a-row>
        </a-form>
        <div style="float: right;marginTop: 15px;">
          <a-button
            style="marginRight: 15px;"
            type="primary"
            onClick={() => {
              this.save(false);
            }}
            loading={this.loading}
          >
            保存
          </a-button>
          {this.pageState == PageState.Add ? (
            <a-button
              style="marginRight: 15px;"
              type="primary"
              onClick={() => {
                this.isSubmit();
              }}
              loading={this.loading}
            >
              提交
            </a-button>
          ) : (
            undefined
          )}
          <a-button
            type="danger"
            onClick={() => {
              this.cancel();
            }}
          >
            返回
          </a-button>
        </div>
      </div>
    );
  },
};
