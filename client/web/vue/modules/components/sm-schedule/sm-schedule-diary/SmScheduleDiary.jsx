import './style';
import { requestIsSuccess, objFilterProps, CreateGuid } from '../../_utils/utils';
import ApiDiary from '../../sm-api/sm-schedule/Diary';
import ApiApproval from '../../sm-api/sm-schedule/Approval';
import ApiMaterial from '../../sm-api/sm-material/Material';
import { PageState, MaterialType, MemberType } from '../../_utils/enum';
import moment from 'moment';
import * as utils from '../../_utils/utils';
import SmScheduleDiaryModal from '../sm-schedule-diarys/SmScheduleDiaryModal';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmSystemMenbersSelect from '../../sm-system/sm-system-member-select';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ScheduleSelect from '../sm-schedule-schedules-select';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiUser from '../../sm-api/sm-system/User';
let apiDiary = new ApiDiary();
let apiUser = new ApiUser();
let apiApproval = new ApiApproval();
let apiMaterial = new ApiMaterial();

const formResults = [
  'organization', //施工单位 --基本信息
  'code', //日志编号
  'fillTime', //填报时间
  'directors', //负责人
  'builders', //施工员 --
  'discription', //施工描述 --
  'problem', //存在的问题 --
  'memberNum', //劳务人员个数
  'evening_t',
  'afternoon_t',
  'morning_t',
  'temperature',
];
export default {
  name: 'SmScheduleDiary',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    permissions: { type: Array, default: () => [] },
    pageState: { type: String, default: PageState.Add },
  },
  data() {
    return {
      isShow: true, //是否允许修改
      talkMedia: [], //班前讲话视频
      processMedia: [], //施工过程视频
      picture: [], //讲话图片
      tabPane: 'material', //切换项
      iId: null, //施工id
      materialListoption: [], //材料选择框数据
      isTableEdit: false, //是否编辑状态
      material: {
        materialList: [], //辅材
        applianceList: [], //器具
        mechanicalList: [], //机械
        securityProtectionList: [], //安防用品
      },
      processDataSource: [], //进度填报数据
      // materialDataSource: [], //物资数据
      record: null, // 表单绑的对象,
      approvalRecord: null, // 表单绑的对象,
      queryParams: {
        keywords: null, //关键字
      },
      loading: false,
      form: {}, // 表单
      // pictureFileList: [],
      // processMediaFileList: [],
      // talkMediaFileList: [],
    };
  },
  computed: {
    materialDataSource() {
      return !this.tabPane || this.tabPane == 'material'
        ? this.material.materialList
        : this.tabPane == 'appliance'
          ? this.material.applianceList
          : this.tabPane == 'mechanical'
            ? this.material.mechanicalList
            : this.material.securityProtectionList;
    },
    processColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '模型名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '数量',
          dataIndex: 'count',
          ellipsis: true,
          scopedSlots: { customRender: 'count' },
        },
        {
          title: '开始时间',
          ellipsis: true,
          dataIndex: 'startTime',
          scopedSlots: { customRender: 'startTime' },
        },
        {
          title: '结束时间',
          dataIndex: 'endTime',
          ellipsis: true,
          scopedSlots: { customRender: 'endTime' },
        },
        {
          title: '施工进度',
          dataIndex: 'process',
          scopedSlots: { customRender: 'process' },
        },
        {
          title: '当前状态',
          dataIndex: 'currentState',
          scopedSlots: { customRender: 'currentState' },
        },
      ];
    },
    materialColumns() {
      return !this.tabPane || this.tabPane == 'material'
        ? [
          {
            title: '序号',
            dataIndex: 'index',
            ellipsis: true,
            width: 140,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '材料名称',
            dataIndex: 'materialName',
            ellipsis: true,
            scopedSlots: { customRender: 'materialName' },
          },
          {
            title: '规格型号',
            ellipsis: true,
            dataIndex: 'specModel',
            scopedSlots: { customRender: 'specModel' },
          },

          {
            title: '单位',
            ellipsis: true,
            dataIndex: 'unit',
            scopedSlots: { customRender: 'unit' },
          },
          {
            title: '数量',
            dataIndex: 'number',
            ellipsis: true,
            scopedSlots: { customRender: 'number' },
          },
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
        ]
        : [
          {
            title: '序号',
            dataIndex: 'index',
            ellipsis: true,
            scopedSlots: { customRender: 'index' },
          },
          {
            title:
                this.tabPane == 'appliance'
                  ? '使用器具'
                  : this.tabPane == 'mechanical'
                    ? '使用机械'
                    : this.tabPane == 'securityProtection'
                      ? '安防用品'
                      : '',
            dataIndex: 'materialName',
            ellipsis: true,
            scopedSlots: { customRender: 'materialName' },
          },
          {
            title: '单位',
            ellipsis: true,
            dataIndex: 'unit',
            scopedSlots: { customRender: 'unit' },
          },
          {
            title: '数量',
            dataIndex: 'number',
            ellipsis: true,
            scopedSlots: { customRender: 'number' },
          },
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
        ];
    },
  },
  watch: {
    id: {
      handler: async function(value, oldValue) {
        this.iId = value;
        this.initAxios();
        this.refresh();
        await this.getApproval();
        this.getMaterialData();
      },
      immediate: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiUser = new ApiUser(this.axios);
      apiApproval = new ApiApproval(this.axios);
      apiDiary = new ApiDiary(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
    },
    remove() {},
    save() {
      this.form.validateFields(async (error, values) => {
        if (!error) {
          let _talkMedia = [];
          let _processMedia = [];
          let _picture = [];
          let _directors = [];
          let _builders = [];

          this.talkMedia.length > 0
            ? await this.$refs.fileUploadTalk.commit()
            : this.$message.error('班前讲话视频不能为空');
          this.processMedia.length > 0
            ? await this.$refs.fileUploadPicture.commit()
            : this.$message.error('施工过程视频不能为空');
          this.picture.length > 0
            ? await this.$refs.fileUploadProcess.commit()
            : this.$message.error('班前讲话图片不能为空');

          this.talkMedia.map(item => {
            _talkMedia.push({
              fileId: item.id,
            });
          });
          this.processMedia.map(item => {
            _processMedia.push({
              fileId: item.id,
            });
          });
          this.picture.map(item => {
            _picture.push({
              fileId: item.id,
            });
          });
          values.builders.map(item => {
            _builders.push({
              builderId: item.id,
            });
          });
          values.directors.map(item => {
            _directors.push({
              builderId: item.id,
            });
          });
          let data = {
            ...values,
            talkMedias: _talkMedia, //班前讲话视频
            processMedias: _processMedia, //施工过程视频
            pictures: _picture, //讲话图片
            approvalId: this.pageState === PageState.Add ? this.iId : this.record.approvalId,
            weathers: JSON.stringify({
              evening_t: values.evening_t,
              afternoon_t: values.afternoon_t,
              morning_t: values.morning_t,
            }),
            directors: _directors,
            builders: _builders,
            materialList: this.material.materialList, //辅材
            applianceList: this.material.applianceList, //器具
            mechanicalList: this.material.mechanicalList, //机械
            securityProtectionList: this.material.securityProtectionList, //安防用品
          };
          let response = null;
          if (this.pageState === PageState.Add) {
            response = await apiDiary.create({ ...data });
          }
          if (this.pageState === PageState.Edit) {
            console.log('这是添加');
            console.log(this.pageState);
            console.log(data);
            response = await apiDiary.update({ ...data, id: this.record.id });
          }
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('ok');
            this.form.resetFields();
          }
        }
      });
    },
    //  AutoCompute : 1,//辅助材料
    // Appliance : 2,//使用器具
    // Mechanical : 3,//使用机械
    // SafetyArticle :4,//安全防护用品
    async getApproval() {
      let response = await apiApproval.get(this.iId);
      if (requestIsSuccess(response)) {
        this.approvalRecord = response.data;
      }
    },
    async refresh() {
      if (this.pageState == PageState.Add && this.iId) {
        let response = await apiApproval.get(this.iId);
        if (requestIsSuccess(response)) {
          this.record = response.data;
          let date = moment().format('YYYY-MM-DD-HH-mm-ss');
          let num = '';
          num = date.replaceAll('-', '');
          let code = 'RZ-' + num;
          //施工员
          let _builders = [];
          if (this.record && this.record.builders.length > 0) {
            this.record.builders.map(item => {
              if (item && item.id && item.memberId) {
                _builders.push({
                  type: MemberType.User,
                  id: item.memberId,
                });
              }
            });
          }
          //负责人
          let _directors = [];
          if (this.record && this.record.directors.length > 0) {
            this.record.directors.map(item => {
              if (item && item.id && item.memberId) {
                _directors.push({
                  type: MemberType.User,
                  id: item.memberId,
                });
              }
            });
           
          }
          console.log(_directors);
          this.$nextTick(() => {
            this.form.setFieldsValue({
              organization: this.record.organization,
              code: code,
              directors: _directors,
              builders: _builders,
            });
          });
        }
      }
      if (this.pageState == PageState.Edit && this.iId) {
        let response = await apiDiary.get({ approvalId: this.iId });
        if (requestIsSuccess(response)) {
          this.record = response.data;
          this.record.organization = this.record.approval.organization;
          this.record.fillTime =
            this.record && this.record.fillTime ? moment(this.record.fillTime) : null;
          //施工员
          let _builders = [];
          if (this.record && this.record.builders.length > 0) {
            this.record.builders.map(item => {
              if (item && item.id && item.builderId) {
                _builders.push({
                  type: MemberType.User,
                  id: item.builderId,
                });
              }
            });
          }
          //负责人
          let _directors = [];
          if (this.record && this.record.directors.length > 0) {
            this.record.directors.map(item => {
              if (item && item.id && item.builderId) {
                _directors.push({
                  type: MemberType.User,
                  id: item.builderId,
                });
              }
            });
          }
          let _pictureFileList = [];
          let _processMediaFileList = [];
          let _talkMediaFileList = [];

          if (this.record && this.record.talkMedias.length > 0) {
            this.record.talkMedias.map(item => {
              let file = item.file;
              if (file) {
                _talkMediaFileList.push({
                  id: file.id,
                  name: file.name,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          if (this.record && this.record.processMedias.length > 0) {
            this.record.processMedias.map(item => {
              let file = item.file;
              if (file) {
                _processMediaFileList.push({
                  id: file.id,
                  name: file.name,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          if (this.record && this.record.pictures.length > 0) {
            this.record.pictures.map(item => {
              let file = item.file;
              if (file) {
                _pictureFileList.push({
                  id: file.id,
                  name: file.name,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          this.picture = _pictureFileList;
          this.processMedia = _processMediaFileList;
          this.talkMedia = _talkMediaFileList;
          let weathers = JSON.parse(this.record.weathers);
          this.record.evening_t = weathers.evening_t;
          this.record.afternoon_t = weathers.afternoon_t;
          this.record.morning_t = weathers.morning_t;
          this.record.directors = _directors;
          this.record.builders = _builders;
          this.$nextTick(() => {
            this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formResults) });
          });
        }
      }
      this.material.materialList = this.record?this.record.materialList:[];
      this.material.applianceList =  this.record?this.record.applianceList:[];
      this.material.securityProtectionList =  this.record?this.record.securityProtectionList:[];
      this.material.mechanicalList =  this.record?this.record.mechanicalList:[];
    },

    async getMaterialData() {
      let response = await apiMaterial.getAll();
      if (requestIsSuccess(response) && response.data) {
        this.materialListoption = response.data.items;
      }
    },
    addMaterial() {
      let item = this.tabPane;
      switch (item) {
      case 'material':
        this.material.materialList.push({
          id: CreateGuid(),
          materialName: null,
          specModel: null,
          unit: null,
          number: 1,
          isEdit: false,
        });
        break;
      case 'appliance':
        this.material.applianceList.push({
          id: CreateGuid(),
          materialName: null,
          unit: null,
          number: 1,
          isEdit: false,
        });
        console.log('snd');
        break;
      case 'mechanical':
        this.material.mechanicalList.push({
          id: CreateGuid(),
          materialName: null,
          unit: null,
          number: 1,
          isEdit: false,
        });
        break;
      case 'securityProtection':
        this.material.securityProtectionList.push({
          id: CreateGuid(),
          materialName: null,
          unit: null,
          number: 1,
          isEdit: false,
        });
        break;
      }
    },
    removeMaterial(record) {
      let item = this.tabPane;
      switch (
        item //material,appliance,mechanical,securityProtection
      ) {
      case 'material':
        this.material.materialList = this.material.materialList.filter(
          item => item && record && item.id != record.id,
        );
        break;
      case 'appliance':
        this.material.applianceList = this.material.applianceList.filter(
          item => item && record && item.id != record.id,
        );
        break;
      case 'mechanical':
        this.material.mechanicalList = this.material.mechanicalList.filter(
          item => item && record && item.id != record.id,
        );
        break;
      case 'securityProtection':
        this.material.securityProtectionList = this.material.securityProtectionList.filter(
          item => item && record && item.id != record.id,
        );
        break;
      }
    },
    getInfo(id, record) {
      let info = this.materialListoption.filter(item => item.id == id);
      record.materialName = info[0].name;
      record.specModel = info[0].spec + ' ' + info[0].model;
      record.unit = info[0].unit;
    },
    // 判断当前材料
    changeTabPane(item) {
      this.tabPane = item; //material,appliance,mechanical,securityProtection
    },
    async view() {
      let _record = {};
      this.form.validateFields(async (error, values) => {
        if (!error) {
          _record = values;
          (_record.materialList = this.material.materialList), //辅材
          (_record.applianceList = this.material.applianceList), //器具
          (_record.mechanicalList = this.material.mechanicalList), //机械
          (_record.securityProtectionList = this.material.securityProtectionList), //安防用品
          (_record.talkMedias = this.talkMedia);
          _record.processMedias = this.processMedia;
          _record.pictures = this.picture;

          //施工员
          let _buildersIds = [];
          let _directorsIds = [];
          if (_record && _record.builders.length > 0) {
            _record.builders.map(item => {
              _buildersIds.push(item.id);
            });
          }
          if (_record && _record.directors.length > 0) {
            _record.directors.map(item => {
              _directorsIds.push(item.id);
            });
          }
          let _builders = null;
          let responseBuilder = await apiUser.getListByIds(_buildersIds);
          if (requestIsSuccess(responseBuilder)) {
            let _buildersData = responseBuilder.data;
            _buildersData.map(item => {
              if (!_builders) {
                _builders = item.name;
              } else {
                _builders += '、' + item.name;
              }
            });
          }
          let _directors = null;
          let responseDirectors = await apiUser.getListByIds(_directorsIds);
          if (requestIsSuccess(responseDirectors)) {
            let _directorsData = responseDirectors.data;
            _directorsData.map(item => {
              if (!_directors) {
                _directors = item.name;
              } else {
                _directors += '、' + item.name;
              }
            });
          }
          _record.buildersName = _builders;
          _record.directorsName = _directors;

          this.approvalRecord.diaryCode = _record.code;
          let data = {
            dRecord: _record,
            aRecord: this.approvalRecord,
          };
          this.$refs.SmScheduleDiaryModal.preview(data);
        }
      });
    },
    cancel() {
      this.$emit('cancel');
      this.form.resetFields();
    },
  },
  render() {
    //辅助材料
    let materialOptions = [];
    this.materialListoption.map(item => {
      if (this.tabPane == 'material' && item.type.category == MaterialType.AutoCompute) {
        materialOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
      if (this.tabPane == 'appliance' && item.type.category == MaterialType.Appliance) {
        materialOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
      if (this.tabPane == 'mechanical' && item.type.category == MaterialType.Mechanical) {
        materialOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
      if (
        this.tabPane == 'securityProtection' &&
        item.type.category == MaterialType.SafetyArticle
      ) {
        materialOptions.push(<a-select-option key={item.id}>{item.name}</a-select-option>);
      }
    });

    // 物资
    let materialInformation = (
      <div>
        <div class="button-add">
          <a-button type="primary" icon="plus" onClick={this.addMaterial} size="small">
            新增
          </a-button>
        </div>
        <a-table
          columns={this.materialColumns}
          rowKey={record => record.id}
          size="middle"
          dataSource={this.materialDataSource}
          bordered={this.bordered}
          // customRow={record => {
          //   return {
          //     on: {
          //       // 事件
          //       mouseleave: event => {
          //         record.isEdit = false;
          //       },
          //       mouseenter: event => {
          //         record.isEdit = true;
          //       }, // 鼠标移入行
          //     },
          //   };
          // }}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1;
              },
              materialName: (text, record) => {
                return !record.isEdit
                  ? [
                    <a-select
                      allowClear={true}
                      axios={this.axios}
                      placeholder={this.pageState == PageState.View ? '' : '请选择材料名称'}
                      value={record.materialName}
                      onChange={value => {
                        this.getInfo(value, record);
                      }}
                    >
                      {materialOptions}
                    </a-select>,
                    // <a-input
                    //   //   // onBlur={()=>record.isEdit=false}
                    //   allowClear={true}
                    //   placeholder={!record.isEdit ? '' : '请选择材料名称'}
                    //   value={record.materialName}
                    //   onChange={event => {
                    //     record.materialName = event.target.value;
                    //   }}
                    // ></a-input>,
                  ]
                  : record.materialName;
              },
              specModel: (text, record) => {
                return !record.isEdit
                  ? [
                    <a-input
                      allowClear={true}
                      placeholder={this.pageState == PageState.View ? '' : '请输入规格型号'}
                      value={record.specModel}
                      onChange={event => {
                        record.specModel = event.target.value;
                      }}
                    ></a-input>,
                  ]
                  : record.specModel;
              },
              // name: (text, record) => {
              //   return record.isEdit
              //     ? [
              //       <a-input
              //         allowClear={true}
              //         placeholder={!record.isEdit ? '' : '请输入名称'}
              //         value={record.name}
              //         onChange={event => {
              //           record.name = event.target.value;
              //         }}
              //       ></a-input>,
              //     ]
              //     : record.name;
              // },
              unit: (text, record) => {
                return !record.isEdit
                  ? [
                    <a-input
                      allowClear={true}
                      placeholder={this.pageState == PageState.View ? '' : '请输入单位'}
                      value={record.unit}
                      onChange={event => {
                        record.unit = event.target.value;
                      }}
                    ></a-input>,
                  ]
                  : record.unit;
              },

              number: (text, record) => {
                return !record.isEdit
                  ? [
                    <a-input-number
                      allowClear={true}
                      placeholder={this.pageState == PageState.View ? '' : '请输入数量'}
                      min={1}
                      style="width:100%"
                      precision={0} //精度为0，只能为整数
                      value={record.number}
                      onChange={value => {
                        record.number = value;
                      }}
                    ></a-input-number>,
                  ]
                  : record.number;
              },

              operations: (text, record) => {
                return [
                  <span>
                    <a
                      onClick={() => {
                        this.removeMaterial(record);
                      }}
                    >
                      删除
                    </a>
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
      </div>
    );

    return (
      <div class="sm-schedule-diary">
        {/* ----基本信息---- */}
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-tabs>
              <a-tab-pane key="basic" tab="基本信息">
                <a-col sm={12} md={12}>
                  <a-form-item label="施工单位" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      placeholder={this.pageState == PageState.Add ? '' : '请输入施工单位'}
                      disabled={true}
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
                  <a-form-item label="日志编号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      placeholder={this.pageState == PageState.Add ? '' : '请输入编号'}
                      disabled={true}
                      v-decorator={[
                        'code',
                        {
                          initialValue: null,
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
                  <a-form-item label="时间选择" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-date-picker
                      style="width:100%"
                      disabled={this.pageState == PageState.View}
                      format="YYYY-MM-DD"
                      v-decorator={[
                        'fillTime',
                        {
                          initialValue: '',
                          rules: [
                            {
                              required: true,
                              message: '请选择填报时间',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12} class="system-menbers-select">
                  <a-form-item label="负责人" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <SmSystemMenbersSelect
                      height={32}
                      shouIconSelect={true}
                      showUserTab={true}
                      userMultiple={true}
                      bordered={true}
                      axios={this.axios}
                      disabled={this.pageState == PageState.View}
                      placeholder={'请选择负责人'}
                      onChange={item => {
                        console.log(item);
                      }}
                      v-decorator={[
                        'directors',
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
                  <a-form-item label="施工员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <SmSystemMenbersSelect
                      height={32}
                      shouIconSelect={true}
                      showUserTab={true}
                      userMultiple={true}
                      bordered={true}
                      axios={this.axios}
                      disabled={this.pageState == PageState.View}
                      placeholder={'请选择施工员'}
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

                <a-col sm={12} md={12}>
                  <a-form-item label="劳务人员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input-number
                      placeholder={this.pageState == PageState.View ? '' : '请输入劳务人员个数'}
                      disabled={this.pageState == PageState.View}
                      min={0}
                      style="width:100%"
                      v-decorator={[
                        'memberNum',
                        {
                          initialValue: '',
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
                <a-col sm={12} md={12}>
                  <a-form-item label="施工描述" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-textarea
                      rows="5"
                      placeholder={this.pageState == PageState.View ? '' : '请输入施工描述'}
                      disabled={this.pageState == PageState.View}
                      v-decorator={[
                        'discription',
                        {
                          rules: [
                            {
                              whitespace: true,
                              required: true,
                              initialValue: null,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={8} md={8}>
                  <a-form-item label="天气" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                    <a-card>
                      <a-card-grid style="width:33.3333333%;text-align:center" hoverable={false}>
                        上午
                      </a-card-grid>
                      <a-card-grid style="width:33.3333333%;text-align:center" hoverable={false}>
                        下午
                      </a-card-grid>
                      <a-card-grid style="width:33.3333333%;text-align:center" hoverable={false}>
                        夜晚
                      </a-card-grid>
                      <a-card-grid
                        style="width:33.3333333%;text-align:center"
                        hoverable={false}
                        class="temperature"
                      >
                        <a-form-item>
                          <a-input
                            placeholder={this.pageState == PageState.Add ? '' : '请输入'}
                            disabled={this.pageState == PageState.View}
                            v-decorator={[
                              'morning_t',
                              {
                                initialValue: null,
                                rules: [
                                  {
                                    // required: true,
                                    message: '请填入',
                                    whitespace: true,
                                  },
                                ],
                              },
                            ]}
                          />
                        </a-form-item>
                      </a-card-grid>
                      <a-card-grid
                        style="width:33.3333333%;text-align:center"
                        hoverable={false}
                        class="temperature"
                      >
                        <a-form-item>
                          <a-input
                            placeholder={this.pageState == PageState.Add ? '' : '请输入'}
                            disabled={this.pageState == PageState.View}
                            v-decorator={[
                              'afternoon_t',
                              {
                                initialValue: null,
                                rules: [
                                  {
                                    // required: true,
                                    message: '请填入',
                                    whitespace: true,
                                  },
                                ],
                              },
                            ]}
                          />
                        </a-form-item>
                      </a-card-grid>
                      <a-card-grid
                        style="width:33.3333333%;text-align:center"
                        hoverable={false}
                        class="temperature"
                      >
                        <a-form-item>
                          <a-input
                            placeholder={this.pageState == PageState.Add ? '' : '请输入'}
                            disabled={this.pageState == PageState.View}
                            v-decorator={[
                              'evening_t',
                              {
                                initialValue: null,
                                rules: [
                                  {
                                    // required: true,
                                    message: '请填入',
                                    whitespace: true,
                                  },
                                ],
                              },
                            ]}
                          />
                        </a-form-item>
                      </a-card-grid>
                    </a-card>
                  </a-form-item>
                </a-col>
                <a-col sm={4} md={4}>
                  <a-form-item label="温度" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                    <a-input
                      suffix="℃"
                      placeholder={this.pageState == PageState.Add ? '' : '请输入温度'}
                      disabled={this.pageState == PageState.View}
                      v-decorator={[
                        'temperature',
                        {
                          initialValue: null,
                          rules: [
                            {
                              // required: true,
                              message: '请输入温度',
                              whitespace: true,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item
                    label="存在的问题"
                    label-col={{ span: 2 }}
                    wrapper-col={{ span: 22 }}
                  >
                    <a-textarea
                      rows="4"
                      placeholder={this.pageState == PageState.View ? '' : '请输入存在的问题'}
                      disabled={this.pageState == PageState.View}
                      v-decorator={[
                        'problem',
                        {
                          whitespace: true,
                          initialValue: null,
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item
                    label="班前讲话视频"
                    label-col={{ span: 4 }}
                    wrapper-col={{ span: 20 }}
                  >
                    <SmFileUpload
                      mode={this.pageState == PageState.View ? 'view' : 'edit'}
                      axios={this.axios}
                      height={73}
                      accept=".avi, .mov, .rmvb, .rm, .flv, .mp4, .3gp, .mpeg, .mpg"
                      multiple={false}
                      ref="fileUploadTalk"
                      onSelected={item => {
                        console.log(item);
                        this.talkMedia = item;
                      }}
                      placeholder={this.pageState == PageState.View ? '' : '请选择讲话视频'}
                      download={false}
                      fileList={this.talkMedia}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="讲话图片" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <SmFileUpload
                      mode={this.pageState == PageState.View ? 'view' : 'edit'}
                      axios={this.axios}
                      height={73}
                      multiple
                      accept=".jpg, .png, .tif, gif, .JPG, .PNG, .GIF, .jpeg,.JPEG"
                      ref="fileUploadPicture"
                      onSelected={item => {
                        this.picture = item;
                      }}
                      placeholder={this.pageState == PageState.View ? '' : '请选择讲话时图片'}
                      download={false}
                      fileList={this.picture}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={24}>
                  <a-form-item
                    label="施工过程视频"
                    label-col={{ span: 2 }}
                    wrapper-col={{ span: 22 }}
                  >
                    <SmFileUpload
                      mode={this.pageState == PageState.View ? 'view' : 'edit'}
                      axios={this.axios}
                      height={73}
                      multiple
                      accept=".avi, .mov, .rmvb, .rm, .flv, .mp4, .3gp, .mpeg, .mpg"
                      ref="fileUploadProcess"
                      onSelected={item => {
                        this.processMedia = item;
                      }}
                      placeholder={this.pageState == PageState.View ? '' : '请选择施工过程视频'}
                      download={false}
                      fileList={this.processMedia}
                    />
                  </a-form-item>
                </a-col>
              </a-tab-pane>
            </a-tabs>
            {/* ----进度填报---- */}
            <a-tabs>
              <a-tab-pane key="process" tab="进度填报">
                <div class="process-top">
                  <span>
                    <a-input placeholder={'请输入'} class="process-top-input"></a-input>
                  </span>
                  <span>
                    <a-button type="primary" class="process-top-search">
                      搜索
                    </a-button>
                  </span>
                  <span>
                    <a-button class="process-top-modify">批量修改</a-button>
                  </span>
                </div>
                {/* 展示区 */}
                <a-table
                  columns={this.processColumns}
                  rowKey={record => record.id}
                  dataSource={this.processDataSource}
                  bordered={this.bordered}
                  pagination={false}
                  loading={this.loading}
                  {...{
                    scopedSlots: {
                      index: (text, record, index) => {
                        return (
                          index + 1 + this.process.queryParams.maxResultCount * (this.pageIndex - 1)
                        );
                      },
                      name: (text, record) => {
                        return (
                          <a-tooltip placement="topLeft" title={record.name}>
                            <span>{record.name}</span>
                          </a-tooltip>
                        );
                      },
                      count: (text, record) => {
                        return (
                          <a-tooltip placement="topLeft" title={record.count}>
                            <span>{record.count}</span>
                          </a-tooltip>
                        );
                      },
                      startTime: (text, record) => {
                        let startTime =
                          moment(record.startTime).format('YYYY-MM-DD') != '0001-01-01'
                            ? moment(record.startTime).format('YYYY-MM-DD')
                            : '暂无';
                        return (
                          <a-tooltip placement="topLeft" title={startTime}>
                            <span>{startTime}</span>
                          </a-tooltip>
                        );
                      },

                      endTime: (text, record) => {
                        let creationTime =
                          moment(record.creationTime).format('YYYY-MM-DD') != '0001-01-01'
                            ? moment(record.creationTime).format('YYYY-MM-DD')
                            : '暂无';
                        return (
                          <a-tooltip placement="topLeft" title={creationTime}>
                            <span>{creationTime}</span>
                          </a-tooltip>
                        );
                      },
                      process: (text, record) => {
                        return;
                      },
                      currentState: (text, record) => {
                        return;
                      },
                    },
                  }}
                ></a-table>
              </a-tab-pane>
            </a-tabs>
            {/* ----物资信息----  material,appliance,mechanical,securityProtection*/}
            <a-tabs>
              <a-tab-pane key="infor" tab="物资信息">
                <a-tabs type="card" onChange={item => this.changeTabPane(item)}>
                  <a-tab-pane key="material" tab="辅材">
                    {materialInformation}
                  </a-tab-pane>
                  <a-tab-pane key="appliance" tab="器具">
                    {materialInformation}
                  </a-tab-pane>
                  <a-tab-pane key="mechanical" tab="机械">
                    {materialInformation}
                  </a-tab-pane>
                  <a-tab-pane key="securityProtection" tab="安防用品">
                    {materialInformation}
                  </a-tab-pane>
                </a-tabs>
                {/* 展示区 */}
              </a-tab-pane>
            </a-tabs>
          </a-row>
          <div class="button-actions">
            <span>
              <a-button size="small" onClick={() => this.cancel()}>
                关闭
              </a-button>
            </span>
            <span>
              <a-button class="button-view" onClick={this.view} size="small">
                预览
              </a-button>
            </span>
            <span>
              <a-button class="button-submit" size="small" onClick={this.save}>
                提交
              </a-button>
            </span>
          </div>
        </a-form>
        <SmScheduleDiaryModal ref="SmScheduleDiaryModal" axios={this.axios} />
      </div>
    );
  },
};
