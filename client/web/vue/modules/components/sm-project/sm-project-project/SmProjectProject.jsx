// 项目管理基础模块
import "./style/index";
import { PageState, ProjectState, BuildUnitType } from '../../_utils/enum';
import SmSystemAllUserSelect from '../../sm-system/sm-system-member-modal';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import SmSystemSelect from '../../sm-system/sm-system-user-select';
import SmArea from '../../sm-common/sm-area-module';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SnMapAmap from '../../sn-components/sn-map-amap';
import * as utils from '../../_utils/utils';
import ApiProject from "../../sm-api/sm-project/Project";
import moment from 'moment';
import Contracts from '../../sm-oa/sm-oa-contracts';


let apiProject = new ApiProject();

const formValues = [
  'code',//编号
  'name',//名称
  'typeId',//类型
  'organizationId',//分支机构
  'state',//状态
  'managerId',//负责人
  'plannedStartTime',
  'plannedEndTime',
  'area',//区域
  'address',//地址
  'description',//项目简介
  'unit',//参建单位
  'scale',//项目规模
  'remark',
  'cost',//造价
  'lat',//纬度
  "lng",//经度
  "projectRltFileIds",
];
export default {
  name: 'SmProjectProject',
  components: {},
  props: {
    axios: { type: Function, default: null },
    pageState: { type: String, default: PageState.Add },
    permissions: { type: Array, default: () => [] },
    id: { type: String, default: null },
  },
  data() {
    return {
      loading: false,
      userSelectVisible: false,//人员选择
      form: {},
      queryParams: {
        mapValue: {},
        userList: [],//项目成员信息
        unitList: [],//参建单位信息
        contractList: [],//合同信息
        detailAddress: null,//详细地址
        projectCode: "",
      },
      labelCol: { span: 6 },
      wrapperCol: { span: 18 },
      tableUserListIds: [],//批量添加人员信息
      selectedUserIds: [],//批量选择人员
      selectedUnitIds: [],//批量参建单位
      selectedContractIds: [],//批量合同
      contractVisible: false,
      tableUserKey: null,
      adcode: null,//行政区编码
      center: [],
    };
  },
  computed: {
    isShow() {
      return this.pageState == PageState.View;
    },
    //用户表格 表头
    userColumns() {
      return !this.isShow ? [
        {
          title: "序号",
          dataIndex: "index",
          width: 90,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: "姓名",
          dataIndex: "name",
          width: 300,
          scopedSlots: { customRender: 'name' },
        }, {
          title: "岗位",
          dataIndex: "post",
          width: 300,
          scopedSlots: { customRender: 'post' },
        }, {
          title: "电话",
          width: 300,
          dataIndex: "telephone",
          scopedSlots: { customRender: 'telephone' },
        }, {
          title: "操作",
          width: 75,
          dataIndex: "operations",
          scopedSlots: { customRender: 'operations' },
        }] :
        [
          {
            title: "序号",
            dataIndex: "index",
            width: 90,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: "姓名",
            dataIndex: "name",
          }, {
            title: "岗位",
            dataIndex: "post",
          },
          {
            title: "电话",
            width: 200,
            dataIndex: "telephone",
          },
        ];
    },
    //建设单位 表头
    unitColumns() {
      return !this.isShow ? [
        {
          title: "序号",
          dataIndex: "index",
          width: 110,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: "参建单位名称",
          dataIndex: "name",
          width: 300,
          scopedSlots: { customRender: 'name' },
        }, {
          title: "单位代码",
          dataIndex: "code",
          width: 300,
          scopedSlots: { customRender: 'code' },
        }, {
          title: "单位类型",
          width: 300,
          dataIndex: "type",
          scopedSlots: { customRender: 'type' },
        }, {
          title: "银行代码",
          width: 300,
          dataIndex: "bankCode",
          scopedSlots: { customRender: 'bankCode' },
        }, {
          title: "银行名称",
          width: 300,
          dataIndex: "bnakName",
          scopedSlots: { customRender: 'bankName' },
        }, {
          title: "银行账号",
          width: 300,
          dataIndex: "bankAccount",
          scopedSlots: { customRender: 'bankAccount' },
        }, {
          title: "操作",
          width: 200,
          dataIndex: "operations",
          scopedSlots: { customRender: 'operations' },
        },
      ] : [
        {
          title: "序号",
          dataIndex: "index",
          width: 110,
        },
        {
          title: "参建单位名称",
          dataIndex: "name",
          width: 300,
        }, {
          title: "单位代码",
          dataIndex: "code",
          width: 300,
        }, {
          title: "单位类型",
          width: 300,
          dataIndex: "type",
        }, {
          title: "银行代码",
          width: 300,
          dataIndex: "bankCode",
        }, {
          title: "银行名称",
          width: 300,
          dataIndex: "bnakName",
        }, {
          title: "银行账号",
          width: 300,
          dataIndex: "bankAccount",
        },
      ];
    },
    //合同  表头
    contractColumns() {
      return [
        {
          title: "序号",
          dataIndex: "index",
          width: 90,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: "合同编号",
          dataIndex: "code",
          width: 300,
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        }, {
          title: "合同名称",
          dataIndex: "name",
          ellipsis: true,
          width: 300,
          scopedSlots: { customRender: 'name' },
        }, {
          title: "合同类型",
          width: 300,
          ellipsis: true,
          dataIndex: "type",
          scopedSlots: { customRender: 'telephone' },
        }, {
          title: "操作",
          width: 75,
          ellipsis: true,
          dataIndex: "operations",
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    id: {
      handler: function (value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
    },
    pageState: {
      handler: function (value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
    },
  },
  created() {
    this.initAxios();
    this.refresh();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiProject = new ApiProject(this.axios);
      // apiAccount = new ApiAccount(this.axios);
    },
    save() {
      this.form.validateFields(async (error, value) => {
        if (!error) {
          let _values = value;
          let data = {
            ..._values,
            area: _values.area ? _values.area.pop() : "",
            plannedStartTime: moment(_values.plannedStartTime).format("YYYY-MM-DD"),
            plannedEndTime: moment(_values.plannedEndTime).format("YYYY-MM-DD"),
            units: this.queryParams.unitList,
            projectRltMemberIds: this.queryParams.userList.length > 0 ? this.queryParams.userList.map(item => item.id) : [],
            projectRltFileIds: _values.projectRltFileIds.map(item => item.id),
            ProjectRltContractIds: this.queryParams.contractList.map(item => item.id),
            lat: this.queryParams.mapValue.lat,
            lng: this.queryParams.mapValue.lng,
            address: this.queryParams.mapValue.address,
            detailAddress: this.queryParams.detailAddress,
          };

          this.loading = true;
          if (this.pageState == PageState.Add) {
            let response = await apiProject.create(data);
            if (utils.requestIsSuccess(response)) {
              this.$message.info('操作成功');
              this.form.resetFields();
              this.queryParams = {
                mapValue: {
                  address: " ",
                },
              };
              this.$emit('ok', this.id);
            }
          } else if (this.pageState == PageState.Edit) {
            let _data = { id: this.id, ...data };
            let response = await apiProject.update(_data);
            if (utils.requestIsSuccess(response)) {
              this.$message.info('操作成功');
              this.form.resetFields();
              this.queryParams = {
                mapValue: {
                  address: " ",
                },
              };
              this.$emit('ok', this.id);
            }
          }
        }
        this.loading = false;
      });
    },
    //添加项目成员
    addUserInfo(value) {
      if (value !== null) {
        //单行修改
        let userkey = this.queryParams.userList.filter(item => item.key == this.tableUserKey);
        if (userkey.length > 0) {
          let user = value.shift();
          userkey.map(val => {
            val.name = user.name,
            val.key = this.tableUserKey,
            val.id = user.id,
            val.post = '',
            val.telephone = '';
          });
          this.tableUserKey = null;
        } else {//批量添加
          value.map(item => {
            let userData = {
              key: utils.CreateGuid(),
              name: item.name,
              id: item.id,
              post: '',
              telephone: '',
            };
            this.queryParams.userList.push(userData);
          });
        }
      }
      else {//空白添加
        this.queryParams.userList.push({
          key: utils.CreateGuid(),
          post: '',
          telephone: '',
          name: '',
        });
      }

      console.log(this.queryParams.userList);
    },

    //数据
    async refresh() {
      if (this.pageState !== PageState.Add && this.id !== null) {
        let response = await apiProject.get(this.id);
        if (utils.requestIsSuccess(response) && response.data) {
          let projectData = response.data;
          let _project = {
            ...projectData,
            projectRltFileIds: projectData.projectRltFiles.map(item => { return item.file; }),
            area: projectData.area == null ? [] : [projectData.area],
          };
          this.queryParams.detailAddress = projectData.detailAddress;
          this.queryParams.projectCode = projectData.code;
          this.queryParams.mapValue.address = projectData.address;
          this.queryParams.mapValue.lng = projectData.lng;
          this.queryParams.mapValue.lat = projectData.lat;
          this.center.push(projectData.lng);
          this.center.push(projectData.lat);
          this.adcode = projectData.area;
          //成员表格
          projectData.projectRltMembers.map(item => {
            this.queryParams.userList.push({
              key: utils.CreateGuid(),
              name: item.manager.name,
              post: item.manager.position,
              telephone: item.manager.phoneNumber,
              id: item.manager.id,
            });
          });
          //参建单位
          projectData.projectRltUnits.map(item => {
            this.queryParams.unitList.push({
              key: utils.CreateGuid(),
              name: item.unit.name,
              code: item.unit.code,
              bankAccount: item.unit.bankAccount,
              bankName: item.unit.bankName,
              bankCode: item.unit.bankCode,
              telephone: item.unit.telephone,
              type: item.unit.type,
            });
          });

          //合同
          projectData.projectRltContracts.map(item => {
            this.queryParams.contractList.push({
              key: utils.CreateGuid(),
              id: item.contractId,
              code: item.contract.code,
              name: item.contract.name,
              type: item.contract.type.name,
            });
          });

          this.$nextTick(() => {
            let values = utils.objFilterProps(_project, formValues);
            this.form.setFieldsValue(values);
          });
        }
      }
    },
    //添加参建单位
    addUnitInfo() {
      let newUnitDataList = {
        key: utils.CreateGuid(),
        name: '',
        code: '',
        type: BuildUnitType.Owner,
        bankName: '',
        bankCode: '',
        bankAccount: '',
      };
      this.queryParams.unitList = [...this.queryParams.unitList, newUnitDataList];
    },

    //选择合同
    addContract(value) {
      let newContractList = [];
      value.map(item => {
        let newContractData = {
          key: utils.CreateGuid(),
          code: item.code,
          name: item.name,
          type: item.type.name,
          id: item.id,
        };
        newContractList.push(newContractData);
      });

      let ids = [];//已经存在数据
      this.queryParams.contractList.map(item => {
        ids.push(item.id);
      });

      let arr = [];
      for (let i = 0; i < newContractList.length; i++) {
        if (ids.indexOf(newContractList[i].id) == -1) {
          arr.push(newContractList[i]);
        }
      }
      this.queryParams.contractList = [...this.queryParams.contractList, ...arr];
    },
    //单个删除
    deleteUser(key, type) {
      switch (type) {
      case "user":
        this.queryParams.userList = this.queryParams.userList.filter(item => item.key != key);
        break;
      case "unit":
        this.queryParams.unitList = this.queryParams.unitList.filter(item => item.key != key);
        break;
      case "contract":
        this.queryParams.contractList = this.queryParams.contractList.filter(item => item.key != key);
        break;
      }
    },
    //批量删除
    batchDeletion(type) {
      switch (type) {
      case "user":
        this.selectedUserIds.map(id => {
          this.queryParams.userList = this.queryParams.userList.filter(item => item.key != id);
        });
        break;
      case "unit":
        this.selectedUnitIds.map(id => {
          this.queryParams.unitList = this.queryParams.unitList.filter(item => item.key != id);
        });
        break;
      case "contract":
        this.selectedContractIds.map(id => {
          this.queryParams.contractList = this.queryParams.contractList.filter(item => item.key != id);
        });
      }
    },
    cancel() {
      this.form.resetFields();
      this.queryParams = {
        mapValue: {
          address: " ",
        },
      };
      this.$emit('cancel');
    },
  },
  render() {
    //位置类别
    let projectStateOption = [];
    for (let item in ProjectState) {
      projectStateOption.push(
        <a-select-option key={ProjectState[item]} >
          {utils.getProjectStateTitle(ProjectState[item])}
        </a-select-option>,
      );
    }
    //参建单位类型
    let buildUnitStateOption = [];
    for (let item in BuildUnitType) {
      buildUnitStateOption.push(
        <a-select-option key={BuildUnitType[item]}>
          {utils.getBuildTypeTitle(BuildUnitType[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="SmProjectProject">
        <a-form form={this.form}>
          <a-row gutter={24} >
            <a-col sm={12} md={8}>
              <a-form-item
                label="项目编号"
                label-col={this.labelCol} wrapper-col={this.wrapperCol}
              >
                <a-input
                  disabled={true}
                  value={this.queryParams.projectCode}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={16}>
              <a-form-item
                label="项目名称"
                label-col={{ span: 3 }} wrapper-col={{ span: 21 }}
              >
                <a-input
                  disabled={this.isShow}
                  v-decorator={[
                    'name',
                    {
                      initialValue: '',
                      rules: [
                        {
                          required: true,
                          message: '请输入项目名称',
                        },
                        {
                          whitespace: true,
                          message: "请输入项目名称",
                        },
                      ],
                    },
                  ]}
                ></a-input>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="工程类型"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <DataDictionaryTreeSelect
                  disabled={this.isShow}
                  axios={this.axios}
                  groupCode={'ProjectType'}
                  v-decorator={[
                    'typeId',
                    {
                      initialValue: null,
                      rules:
                        [
                          {
                            required: true,
                            message: "请选择工程类型",
                          },
                        ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="建设单位"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <OrganizationTreeSelect
                  axios={this.axios}
                  disabled={this.isShow}
                  v-decorator={[
                    'organizationId',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: "请选择建设单位",
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="项目状态"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-select
                  disabled={this.isShow}
                  v-decorator={
                    [
                      "state",
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: "请选择项目状态",
                          },
                        ],
                      },
                    ]
                  }
                >
                  {projectStateOption}
                </a-select>
              </a-form-item>
            </a-col>
            {/* <a-col sm={12} md={8}>
              <a-form-item
                label="分支机构"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <OrganizationTreeSelect
                  axios={this.axios}
                  v-decorator={[
                    'organizationId',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col> */}
            <a-col sm={12} md={8}>
              <a-form-item
                label="负责人员"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                {/* <a-input
                  disabled={this.isShow}
                  onClick={ ()=>{ this.userSelectVisible = true; }} 
                ></a-input> */}
                <SmSystemSelect
                  axios={this.axios}
                  disabled={this.isShow}
                  v-decorator={[
                    'managerId',
                    {
                      initialValue: null,
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
            <a-col sm={12} md={8}>
              <a-form-item
                label="建设规模"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-input
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'scale',
                      {
                        initialValue: "",
                        rules: [
                          {
                            required: true,
                            message: "请输入建设规模",
                          },
                          {
                            whitespace: true,
                            message: "请输入建设规模",
                          },
                        ],
                      },
                    ]
                  }
                ></a-input>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="计划开工"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-date-picker
                  style="width: 100%;"
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'plannedStartTime',
                      {
                        initialValue: undefined,
                        rules: [
                          {
                            required: true,
                            message: "请选择时间",
                          },
                        ],
                      },
                    ]
                  }
                >
                </a-date-picker>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="计划竣工"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-date-picker
                  style="width: 100%;"
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'plannedEndTime',
                      {
                        initialValue: undefined,
                        rules: [
                          {
                            required: true,
                            message: "请选择时间",
                          },
                        ],
                      },
                    ]
                  }
                >
                </a-date-picker>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="项目造价"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-input
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'cost',
                      {
                        initialValue: "",
                      },
                    ]
                  }
                ></a-input>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="区划信息"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <SmArea
                  disabled={this.isShow}
                  axios={this.axios}
                  deep={2}
                  onChange={
                    item => {
                      this.adcode = item[item.length - 1];
                    }
                  }
                  v-decorator={
                    [
                      'area',
                      {
                        initialValue: null,
                      },
                    ]
                  }
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="项目地址"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <SnMapAmap
                  visable={this.visable}
                  disabled={this.isShow}
                  onChange={value => { this.queryParams.mapValue = value; this.queryParams.detailAddress = value.address; }}
                  address={this.queryParams.mapValue.address}
                  adcode={this.adcode}
                  centerValue={this.center}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={8}>
              <a-form-item
                label="经纬度"
                label-col={this.labelCol}
                wrapper-col={this.wrapperCol}
              >
                <a-row>
                  <a-col span={11}>
                    <a-input
                      placeholder="经度"
                      disabled={this.isShow}
                      // v-decorator={}
                      value={this.queryParams.mapValue != null ? this.queryParams.mapValue.lng : ""}
                      onChange={value => {
                        this.queryParams.mapValue.lng = value;
                      }}
                    >

                    </a-input>
                  </a-col>
                  <a-col span={2}></a-col>
                  <a-col span={11}>
                    <a-input
                      placeholder="纬度"
                      disabled={this.isShow}
                      value={this.queryParams.mapValue != null ? this.queryParams.mapValue.lat : ""}
                      onChange={value => {
                        this.queryParams.mapValue.lat = value;
                      }}
                    >
                    </a-input>
                  </a-col>
                </a-row>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item
                label="详细地址"
                label-col={{ span: 2 }} wrapper-col={{ span: 22 }}
              >
                <a-input
                  disabled={this.isShow}
                  value={this.queryParams.detailAddress}
                  onChange={value => { this.queryParams.detailAddress = value.target.value; }}
                ></a-input>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item
                label="项目简介"
                label-col={{ span: 2 }} wrapper-col={{ span: 22 }}
              >
                <a-input
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'description',
                      {
                        initialValue: "",
                      },
                    ]
                  }
                ></a-input>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="备注"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <a-textarea
                  disabled={this.isShow}
                  v-decorator={
                    [
                      'remark',
                      {
                        initialValue: "",
                      },
                    ]
                  }
                ></a-textarea>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="附件"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <SmFileManageSelect
                  class="ant-input"
                  style="margin-top: 3px;"
                  axios={this.axios}
                  disabled={this.isShow}
                  height={40}
                  bordered={true}
                  v-decorator={[
                    'projectRltFileIds',
                    {
                      initialValue: [],
                    },
                  ]}
                />

              </a-form-item>
            </a-col>
          </a-row>

          {/* 项目成员 */}
          <div class="divider">
            <span>项目成员</span>
            <a-divider class="dividerItem" />
          </div>
          <div class="userTable">
            <a-button type="primary" icon="plus" size="small" class="userButton" onClick={() => this.addUserInfo(null)} disabled={this.isShow}>新增</a-button>
            <a-button type="danger" icon="delete" size="small" class="userButton" onClick={() => this.batchDeletion("user")} disabled={this.selectedUserIds.length === 0 || this.isShow}>删除</a-button>
            <a-button type="primary" icon="user-add" size="small" class="userButton" onClick={() => this.userSelectVisible = true} disabled={this.isShow}>选择人员</a-button>
          </div>
          <a-table
            columns={this.userColumns}
            dataSource={this.queryParams.userList}
            rowSelection={{
              columnWidth: 30,
              onChange: selectedRowKeys => {
                this.selectedUserIds = selectedRowKeys;
              },
            }}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1; //+ 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                },
                name: (text, record, index) => {
                  if (this.pageState !== PageState.View) {
                    return <a-input
                      value={record.name}
                      class="cellWidth"
                      onClick={() => {
                        this.tableUserKey = record.key;
                        this.userSelectVisible = true;
                      }}
                      onChange={value => {
                        record.name = value.target.value;
                      }}
                    ></a-input>;
                  }
                },
                post: (text, record, index) => {
                  return <a-input
                    class="cellWidth"
                    value={record.post}
                    onChange={value => {
                      record.post = value.target.value;
                    }}
                  ></a-input>;
                },
                telephone: (text, record, index) => {
                  return <a-input
                    class="cellWidth"
                    value={record.telephone}
                    onChange={value => {
                      record.telephone = value.target.value;
                    }}
                  ></a-input>;
                },
                operations: (text, record, index) => {
                  return <a onClick={() => this.deleteUser(record.key, "user")}>
                    <a-icon type="delete" style="color: red;fontSize: 16px;" />
                  </a>;
                },

              },
            }
            }
          >

          </a-table>
          {/* 参建单位 */}
          <div class="divider">
            <span>参建单位</span>
            <a-divider class="dividerItem" />
          </div>
          <div class="userTable">
            <a-button type="primary" icon="plus" size="small" class="userButton" onClick={() => { this.addUnitInfo(); }} disabled={this.isShow}>新增</a-button>
            <a-button type="danger" icon="delete" size="small" class="userButton" onClick={() => { this.batchDeletion("unit"); }} disabled={this.selectedUnitIds.length === 0 || this.isShow}>删除</a-button>
          </div>
          <a-table
            columns={this.unitColumns}
            dataSource={this.queryParams.unitList}
            rowSelection={{
              columnWidth: 30,
              onChange: selectedRowKeys => {
                this.selectedUnitIds = selectedRowKeys;
              },
            }}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1; //+ 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                },
                name: (text, record, index) => {
                  return ([
                    <a-input class="cellWidth"
                      disabled={this.isShow}
                      value={record.name}
                      onChange={value => {
                        record.name = value.target.value;
                      }}>
                    </a-input>,
                  ]);
                },
                code: (text, record, index) => {
                  return ([
                    <a-input class="cellWidth" value={record.code}
                      value={record.code}
                      disabled={this.isShow}
                      onChange={value => {
                        record.code = value.target.value;
                      }}>
                    </a-input>,
                  ]);
                },
                type: (text, record, index) => {
                  return ([
                    <a-select class="cellWidth"
                      disabled={this.isShow}
                      value={record.type == 0 ? BuildUnitType.Owner : record.type}//
                      onChange={value => {
                        record.type = value;
                      }}>
                      {buildUnitStateOption}
                    </a-select>,
                  ]);
                },
                bankName: (text, record, index) => {
                  return ([
                    <a-input class="cellWidth" value={record.bankName}
                      disabled={this.isShow}
                      value={record.bankName}
                      onChange={value => {
                        record.bankName = value.target.value;
                      }}>
                    </a-input>,
                  ]);
                },
                bankCode: (text, record, index) => {
                  return ([
                    <a-input class="cellWidth" value={record.bankCode}
                      disabled={this.isShow}
                      value={record.bankCode}
                      onChange={value => {
                        record.bankCode = value.target.value;
                      }}>
                    </a-input>,
                  ]);
                },
                bankAccount: (text, record, index) => {
                  return ([
                    <a-input class="cellWidth" value={record.bankAccount}
                      disabled={this.isShow}
                      value={record.bankAccount}
                      onChange={value => {
                        record.bankAccount = value.target.value;
                      }}>
                    </a-input>,
                  ]);
                },
                operations: (text, record, index) => {
                  return [
                    <a onClick={() => this.deleteUser(record.key, "unit")}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                  ];
                },
              },
            }}
          >
          </a-table>
          {/* 项目合同 */}
          {/* <div class="divider">
            <span>项目合同</span>
            <a-divider class="dividerItem" />
          </div>
          <div class="userTable">
            <a-button type="primary" icon="plus" size="small" class="userButton" onClick={() => { this.contractVisible = true; }} disabled={this.isShow}>选择合同</a-button>
            <a-button type="danger" icon="delete" size="small" class="userButton" onClick={() => { this.batchDeletion("contract");}} disabled={this.selectedContractIds.length === 0 || this.isShow}>删除</a-button>
          </div>
          <a-table
            columns={this.contractColumns}
            dataSource={this.queryParams.contractList}
            rowSelection={{
              columnWidth: 30,
              onChange: selectedRowKeys => {
                this.selectedContractIds = selectedRowKeys;
              },
            }}
            {...{
              scopedSlots: {
                index:(text,record,index)=>{
                  return index+1;
                },
                operations: (text, record, index) => {
                  return !this.isShow ?[
                    <a onClick={() => this.deleteUser(record.key, "contract")} v-show={!this.isShow}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                    <a-divider type="vertical" />,
                    <a onClick={() => { this.$emit("contractView", record.id); }}><a-icon type="info-circle" style="color:#1890FF;fontSize: 16px;" /></a>,
                  ] : [
                    <a onClick={() => { this.$emit("contractView", record.id); }}><a-icon type="info-circle" style="color:#1890FF;fontSize: 16px;" /></a>,
                  ];
                },
              },
            }}
          >
          </a-table> */}
        </a-form>
        <SmSystemAllUserSelect
          axios={this.axios}
          visible={this.userSelectVisible}
          showUserTab={true}
          onChange={iValue => {
            this.userSelectVisible = iValue;
          }}
          onGetMemberInfo={
            value => {
              this.addUserInfo(value);
            }
          }
        />
        {/* 项目合同选择 */}
        <a-modal
          visible={this.contractVisible}
          width={1250}
          maskClosable={true}
          onCancel={
            () => {
              this.contractVisible = false;
            }
          }
          // onOk={}
          title="合同选择"
          footer={null}
        >
          <Contracts
            style="marginBottom: 50px;"
            axios={this.axios}
            isContract={false}
            permissions={this.permissions}
            onView={value => { this.$emit("contractView", value); }}
            onApply={value => { this.addContract(value); this.contractVisible = false; }}
          />
        </a-modal>
        <div style="float: right;marginTop: 15px;">
          {
            !this.isShow ?
              <a-button style="marginRight: 15px;" type="primary" onClick={() => { this.save(); }} loading={this.loading}>保存</a-button> : null
          }
          <a-button type="danger" onClick={() => { this.cancel(); }}>退出</a-button>
        </div>
      </div>
    );
  },
};
