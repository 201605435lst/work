import './style';
import { requestIsSuccess, getScheduleState, vP, vIf } from '../../_utils/utils';
import moment from 'moment';
import ApiSchedule from '../../sm-api/sm-schedule/Schedule';
import ApiScheduleFlowInfo from '../../sm-api/sm-schedule/ScheduleFlowTemplate';
import SmImport from '../../sm-import/sm-import-basic';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import permissionsSmSchedule from '../../_permissions/sm-schedule';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import { ScheduleState } from '../../_utils/enum';
import FrontSchedulesModal from '../sm-schedule-schedules-select/SmScheduleSchedulesSelectModal';
import SmBpmWorkflowModal from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmGantt from '../../sm-common/sm-gantt';
import FileSaver from 'file-saver';

let apiSchedule = new ApiSchedule();
let apiScheduleFlowInfo = new ApiScheduleFlowInfo();

export default {
  name: 'SmScheduleSchedules',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    multiple: { type: Boolean, default: false }, // 是否多选
    selected: { type: Array, default: () => [] }, //待选计划集合
    isSimple: { type: Boolean, default: false }, //是否为选择模式
    noId: { type: String, default: '' }, //选择前置任务时需要排出的计划
  },
  data() {
    return {
      schedules: [], //列表数据源
      datas:[], //甘特图数据源
      scheduleIds: [], //导出
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keywords: null,
        professionId: null,
        startTime: null,
        endTime: null,
        state: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      isGantt: false, //是否为甘特图模式
      loading: false,

      iVisible: false,
      record: null,
      exceptId: '', //选择除此计划之外的前置任务
      selectedRowKeys: [], //表格已选Ids
      iSelected: [], //已选计划

      dataArr:[], //
    };
  },
  computed: {
    columns() {
      return !this.isSimple
        ? [
          // {
          //   title: '#',
          //   dataIndex: 'index',
          //   scopedSlots: { customRender: 'index' },
          // },
          {
            title: '专业',
            dataIndex: 'profession',
            scopedSlots: { customRender: 'profession' },
          },
          {
            title: '施工部位',
            dataIndex: 'location',
          },
          {
            title: '任务名称',
            dataIndex: 'name',
            scopedSlots: { customRender: 'name' },
          },
          {
            title: '开始时间',
            dataIndex: 'startTime',
            scopedSlots: { customRender: 'startTime' },
          },
          {
            title: '完成时间',
            dataIndex: 'endTime',
            scopedSlots: { customRender: 'endTime' },
          },
          {
            title: '工期',
            dataIndex: 'timeLimit',
            scopedSlots: { customRender: 'timeLimit' },
          },
          {
            title: '进度(%)',
            dataIndex: 'progress',
          },
          {
            title: '审核方式',
            dataIndex: 'checkWay',
            scopedSlots: { customRender: 'checkWay' },
          },
          {
            title: '状态',
            dataIndex: 'state',
            scopedSlots: { customRender: 'state' },
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
          // {
          //   title: '#',
          //   dataIndex: 'index',
          //   scopedSlots: { customRender: 'index' },
          // },
          {
            title: '专业',
            dataIndex: 'profession',
            scopedSlots: { customRender: 'profession' },
          },
          {
            title: '施工部位',
            dataIndex: 'location',
          },
          {
            title: '任务名称',
            dataIndex: 'name',
            scopedSlots: { customRender: 'name' },
          },
          {
            title: '开始时间',
            dataIndex: 'startTime',
            scopedSlots: { customRender: 'startTime' },
          },
          {
            title: '完成时间',
            dataIndex: 'endTime',
            scopedSlots: { customRender: 'endTime' },
          },
          {
            title: '工期',
            dataIndex: 'timeLimit',
            scopedSlots: { customRender: 'timeLimit' },
          },
          {
            title: '状态',
            dataIndex: 'state',
            scopedSlots: { customRender: 'state' },
          },
        ];
    },
  },

  watch: {
    selected: {
      handler: function(value, oldVal) {
        this.iSelected = value;
        this.selectedRowKeys = value.map(item => item.id);
      },
      immediate: true,
    },
    noId: {
      handler: function(value, oldVal) {
        if (value) {
          this.refresh();
        }
      },
      immediate: true,
    },
  },

  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiSchedule = new ApiSchedule(this.axios);
      apiScheduleFlowInfo = new ApiScheduleFlowInfo(this.axios);
    },
    async refresh(resetPage = true, parent = null, page) {
      this.loading = true;
      this.dataArr = [];
      if (parent === null) {
        this.schedules = [];
      }
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let queryParams = {};
      if (parent) {
        queryParams.parentId = parent.id;
      } else {
        queryParams = { ...this.queryParams };
      }
      let response = await apiSchedule.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        noId: this.noId,
        ...queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        if (parent) {
          parent.children = response.data.items;
        } else {
          this.schedules = response.data.items;
          this.totalCount = response.data.totalCount;
        }
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, null, this.pageIndex);
          }
        }
      }
      this.loading = false;

      //给甘特图赋值（指定条数）
      //console.log(this.schedules);
      this.getData(this.schedules);

      this.datas = [{
        name: '施工项目',
        data: [
          this.dataArr.map((item,index) => {
            return {
              index: index + 1,
              major: item.profession ? item.profession.name : null,
              name: item.name,
              start: new Date(Date.parse(moment(item.startTime).format('YYYY-MM-DD HH:mm:ss').replace(/-/g,   "/"))).getTime(),
              end: new Date(Date.parse(moment(item.endTime).format('YYYY-MM-DD HH:mm:ss').replace(/-/g,   "/"))).getTime(),
              completed: item.progress,
              state: getScheduleState(item.state),
              id: item.id,
              y: index,
            };
          }),
        ],
      }];
      this.datas[0].data = this.datas[0].data ? this.datas[0].data[0] : [];
    },

    getData(schedules){
      schedules.map(item => {
        this.dataArr.push(item);
        if(item.children!=null && item.children.length > 0){
          this.getData(item.children);
        }
      });
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    addAutoFlow(record) {
      let _this = this;
      if (record) {
        this.$confirm({
          title: '设置审核流程',
          content: h => <div style="color:red;">{'确认设置审核流程？'}</div>,
          okType: 'warning',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiScheduleFlowInfo.create({ workflowTemplateId: record.id });
              if (requestIsSuccess(response)) {
                _this.$message.success('操作成功');
              }
              setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            });
          },
        });
      }
    },

    add() {
      this.$emit('add','single');
    },
    batchAdd(){
      this.$emit('add','batch');
    },

    addChild(record) {
      this.$emit('addChild', record);
    },

    edit(record) {
      this.$emit('edit', record);
    },

    async editFrontTasks(record, edit) {
      if (!edit) {
        this.iVisible = true;
        this.record = record;
        this.exceptId = record.id;
        record.scheduleRltSchedules.map(item => {
          this.selectedRowKeys.push({
            id: item.frontScheduleId,
            name: item.frontSchedule.name,
          });
        });
      } else {
        let _this = this;
        this.$confirm({
          title: '更新前置任务',
          content: h => <div style="color:red;">确定要更改此计划的前置任务吗？</div>,
          okType: 'warning',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let data = {
                ..._this.record,
                scheduleRltSchedules: record,
              };
              let response = await apiSchedule.update({ ...data, IsFrontSchedules: true });
              if (requestIsSuccess(response)) {
                _this.$message.success('操作成功');
                _this.refresh(false, null, _this.pageIndex);
              }
              setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            });
          },
        });
      }
    },

    milestone(record) {
      let _this = this;
      this.$confirm({
        title: '设为里程碑',
        content: h => <div style="color:red;">确定将此计划设为里程碑吗？</div>,
        okType: 'warning',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSchedule.update({ ...record, isMilestone: true });
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, null, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    view(record) {
      this.$emit('view', record);
    },

    //导出
    export() {
      let _this = this;
      this.loading = true;
      let data = { scheduleIds: this.scheduleIds };
      return new Promise(async (resolve, reject) => {
        let response = await apiSchedule.export(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `任务计划清单.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },

    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSchedule.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, null, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    //导入
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'schedules',
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },

    getProgress() {},

    //更新所选数据
    updateSelected(selectedRows) {
      if (!this.multiple) {
        this.iSelected = selectedRows;
      } else {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.schedules.find(sealItem => sealItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedRowKeys) {
          let schedules = this.schedules.find(item => item.id === id);
          if (!!schedules) {
            _selected.push(JSON.parse(JSON.stringify(schedules)));
          }
        }

        this.iSelected = _selected;
      }
      this.$emit('change', this.iSelected);
    },
  },
  render() {
    let _scheduleState = [];
    for (let item in ScheduleState) {
      _scheduleState.push(
        <a-select-option key={ScheduleState[item]}>
          {getScheduleState(ScheduleState[item])}
        </a-select-option>,
      );
    }
    let tableContent = (
      <div>
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.schedules}
          pagination={false}
          loading={this.loading}
          onExpand={(expanded, record) => {
            if (expanded && record.children.length == 0) {
              this.refresh(false, record);
            }
          }}
          rowSelection={
            this.isSimple
              ? {
                columnWidth: 30,
                type: this.multiple ? 'checkbox' : 'radio',
                selectedRowKeys: this.selectedRowKeys,
                onChange: (key, items) => {
                  this.selectedRowKeys = key;
                  this.updateSelected(items);
                },
              }
              : {
                columnWidth: 30,
                onChange: selectedRowKeys => {
                  this.scheduleIds = selectedRowKeys;
                },
              }
          }
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              profession: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.profession.name}>
                    <span>{index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1)}.{record.profession.name}</span>
                  </a-tooltip>
                );
              },
              name: (text, record) => {
                return (
                  <a
                    onClick={() => {
                      this.view(record);
                    }}
                  >
                    {record.name}
                  </a>
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
                let endTime =
                  moment(record.startTime).format('YYYY-MM-DD') != '0001-01-01'
                    ? moment(record.startTime).format('YYYY-MM-DD')
                    : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={endTime}>
                    <span>{endTime}</span>
                  </a-tooltip>
                );
              },
              timeLimit: (text, record) => {
                return record.timeLimit + ' days';
              },
              checkWay: (text, record) => {
                let checkWay = record.isAuto ? '自动审核' : '人工审核';
                return (
                  <a-tooltip placement="topLeft" title={checkWay}>
                    <span>{checkWay}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                return getScheduleState(record.state);
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >
                        编辑
                      </a>,
                      vP(this.permissions, permissionsSmSchedule.Schedules.Update),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmSchedule.Schedules.Update) &&
                        vP(this.permissions, [
                          permissionsSmSchedule.Schedules.Create,
                          permissionsSmSchedule.Schedules.Delete,
                        ]),
                    )}

                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多
                          <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.addChild(record);
                                }}
                              >
                                添加子项
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmSchedule.Schedules.Create),
                          )}
                          <a-menu-item>
                            <a
                              onClick={() => {
                                this.editFrontTasks(record, false); //打开前置任务选择框，并编辑当前计划的前置任务
                              }}
                            >
                              前置任务
                            </a>
                          </a-menu-item>
                          <a-menu-item>
                            <a
                              onClick={() => {
                                this.milestone(record); // 设置当前计划 的工作类型为里程碑
                              }}
                            >
                              里程碑
                            </a>
                          </a-menu-item>
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.remove(record);
                                }}
                              >
                                删除
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmSchedule.Schedules.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmSchedule.Schedules.Detail,
                        permissionsSmSchedule.Schedules.Update,
                        permissionsSmSchedule.Schedules.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 分页器 */}
        <a-pagination
          style="margin-top:10px; text-align: right;"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
      </div>
    );

    let ganttContent = (
      <div>
        <SmGantt
          axios={this.axios}
          datas={this.datas}
        >

        </SmGantt>
      </div>
    );

    return (
      <div class="sm-schedule-schedules">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            (this.queryParams.professionId = null),
            (this.queryParams.startTime = null),
            (this.queryParams.endTime = null),
            (this.queryParams.state = null),
            this.refresh();
          }}
        >
          <a-form-item label="施工专业">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'Profession'}
              placeholder="请选择施工专业"
              value={this.queryParams.professionId}
              onChange={value => {
                this.queryParams.professionId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          {/* <a-form-item label="施工部位">

          </a-form-item> */}
          {this.isSimple ? (
            undefined
          ) : (
            <a-form-item label="时间选择">
              <div style="display:flex">
                <a-date-picker
                  placeholder="起始时间"
                  value={this.queryParams.startTime ? this.queryParams.startTime : null}
                  onChange={value => {
                    this.queryParams.startTime = value
                      ? moment(value._d).format('YYYY-MM-DD')
                      : null;
                    if (
                      (value != null && this.queryParams.endTime != null) ||
                      (value == null && this.queryParams.endTime == null)
                    ) {
                      this.refresh();
                    }
                  }}
                />
                <p style="margin: 0 3px;">—</p>
                <a-date-picker
                  placeholder="结束时间"
                  value={this.queryParams.endTime ? this.queryParams.endTime : null}
                  onChange={value => {
                    this.queryParams.endTime = value ? moment(value._d).format('YYYY-MM-DD') : null;
                    if (
                      (value != null && this.queryParams.startTime != null) ||
                      (value == null && this.queryParams.startTime == null)
                    ) {
                      this.refresh();
                    }
                  }}
                />
              </div>
            </a-form-item>
          )}
          {this.isSimple ? (
            undefined
          ) : (
            <a-form-item label="计划状态">
              <a-select
                allowClear
                placeholder="请选择计划状态"
                value={this.queryParams.state}
                onChange={value => {
                  this.queryParams.state = value;
                  this.refresh();
                }}
              >
                {_scheduleState}
              </a-select>
            </a-form-item>
          )}
          <a-form-item label="关键字">
            <a-input
              axios={this.axios}
              placeholder={'请输入专业/任务名称/工期'}
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          {this.isSimple ? (
            undefined
          ) : (
            <template slot="buttons">
              <div style={'display:flex'}>
                {vIf(
                  <a-button type="primary" onClick={() => this.add()}>
                    <a-icon type="plus" /> 添加
                  </a-button>,
                  vP(this.permissions, permissionsSmSchedule.Schedules.Create),
                )}
                {vIf(
                  <a-button type="primary" onClick={() => this.batchAdd()}>
                    <a-icon type="plus" /> 批量添加
                  </a-button>,
                  vP(this.permissions, permissionsSmSchedule.Schedules.Create),
                )}
                {vIf(
                  <SmImport
                    ref="smImport"
                    url="api/app/resourceEquipment/upload"
                    axios={this.axios}
                    downloadErrorFile={true}
                    importKey="schedules"
                    onSelected={file => this.fileSelected(file)}
                    onIsSuccess={() => this.refresh()}
                  />,
                  vP(this.permissions, permissionsSmSchedule.Schedules.Import),
                )}
                {vIf(
                  <a-button
                    type="primary"
                    onClick={() => this.export()}
                    disabled={this.scheduleIds.length === 0}
                    loading={this.loading}
                  >
                    {' '}
                    <a-icon type="export" /> 导出
                  </a-button>,
                  vP(this.permissions, permissionsSmSchedule.Schedules.Export),
                )}
                <a-button type="primary" onClick={() => this.getProgress()}>
                  <a-icon type="sync" />
                  进度同步
                </a-button>
                {this.isGantt ? (
                  <a-button
                    type="primary"
                    onClick={() => {
                      this.isGantt = false;
                    }}
                  >
                    列表模式
                  </a-button>
                ) : (
                  <a-button
                    type="primary"
                    onClick={() => {
                      this.isGantt = true;
                    }}
                  >
                    甘特图模式
                  </a-button>
                )}
                {vIf(
                  <a-button type="primary" onClick={() => this.$refs.flowSelect.show()}>
                    <a-icon />
                    设置审核流程
                  </a-button>,
                  vP(this.permissions, permissionsSmSchedule.Schedules.SetFlow),
                )}
              </div>
            </template>
          )}
        </sc-table-operator>

        {/* 展示区 */}
        {this.isGantt ? ganttContent : tableContent}


        <FrontSchedulesModal
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedRowKeys}
          multiple={true}
          noId={this.exceptId}
          onOk={selected => {
            this.editFrontTasks(selected, true);
          }}
          onChange={v => ((this.iVisible = v), (this.selectedRowKeys = []))}
        ></FrontSchedulesModal>
        {/* 流程选择框 */}
        <SmBpmWorkflowModal
          ref="flowSelect"
          axios={this.axios}
          onSelected={value => {
            this.addAutoFlow(value);
          }}
        ></SmBpmWorkflowModal>
      </div>
    );
  },
};
