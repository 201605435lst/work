import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import moment from 'moment';
import FileSaver from 'file-saver';
import permissionsSmschedule from '../../_permissions/sm-schedule';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiDiary from '../../sm-api/sm-schedule/Diary';
import { DiaryState } from '../../_utils/enum';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmScheduleDiaryModal from './SmScheduleDiaryModal';
import SmScheduleDiaryAuthorizationModal from '../sm-schedule-diarys/SmScheduleDiaryAuthorizationModal';
let apiDiary = new ApiDiary();

export default {
  name: 'SmScheduleDiarys',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: null, //数据--日志
      loading: false,
      totalCount: 0,
      record: null,
      recordList: [],
      selectedRowKeys: [],
      pageIndex: 1,
      approvalId: null,
      queryParams: {
        professionId: null, //施工作业id
        keywords: null, //关键字
        startTime: null,
        endTime: null,
        isCreator: false,
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '日志编号',
          dataIndex: 'code',
          width: 160,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '专业',
          ellipsis: true,
          dataIndex: 'profession',
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '施工部位',
          ellipsis: true,
          dataIndex: 'location',
          scopedSlots: { customRender: 'location' },
        },
        {
          title: '施工内容',
          dataIndex: 'content',
          ellipsis: true,
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '申报人',
          ellipsis: true,
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
        },
        {
          title: '施工日期',
          ellipsis: true,
          dataIndex: 'startTime',
          scopedSlots: { customRender: 'startTime' },
        },
        {
          title: '填报时间',
          dataIndex: 'creationTime',
          ellipsis: true,
          scopedSlots: { customRender: 'creationTime' },
        },
        {
          title: '状态',
          dataIndex: 'state',
          ellipsis: true,
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiDiary = new ApiDiary(this.axios);
    },
    fill(record) {
      this.$emit('fill', record.id);
    },
    async view(record) {
      this.$refs.SmScheduleDiaryModal.view(record);
    },
    edit(record) {
      this.$emit('edit', record.id);
    },
    //获取审批单
    authorization() {
      if (this.recordList.length > 0) {
        this.$refs.SmScheduleDiaryAuthorizationModal.infor(this.recordList);
      } else {
        this.$message.warning('请选择你要查看的审批单');
      }
    },
    logStatistics() {
      this.$emit('logStatistics');
    },
    remove() {},
    async export() {
      let _this = this;
      //导出按钮
      _this.isLoading = true;
      _this.$confirm({
        title: '确认导出',
        content: h => (
          <div style="color:red;">
            {this.selectedRowKeys.length == 0
              ? '确定要导出全部数据吗？'
              : `确定要导出这 ${this.selectedRowKeys.length} 条数据吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          let data = {
            ..._this.queryParams,
            ids: _this.selectedRowKeys,
          };
        
          return new Promise(async (resolve, reject) => {
            let response = await apiDiary.export(  
              data,
            );
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'application/vnd.ms-excel' }),
                `日志填报记录表.xlsx`,
              );
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
      };
      let response = await apiDiary.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
      if (this.dataSource.length > 0) {
        this.isCanExport = false;
      } else {
        this.isCanExport = true;
      }
      this.loading = false;
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
  },
  render() {
    return (
      <div class="sm-schedule-diarys">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            this.queryParams.professionId = null;
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.queryParams.isCreator = false;
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
          <a-form-item label="关键字">
            <a-input
              allowClear={true}
              axios={this.axios}
              placeholder={'请输入施工部位/施工内容'}
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="时间选择">
            <div style="display:flex">
              <a-date-picker
                placeholder="起始时间"
                value={this.queryParams.startTime ? this.queryParams.startTime : null}
                onChange={value => {
                  this.queryParams.startTime = value ? moment(value).format('YYYY-MM-DD') : null;
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
                  this.queryParams.endTime = value ? moment(value).format('YYYY-MM-DD') : null;
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
          <template slot="buttons">
            <a-button
              class="all-button"
              size="small"
              onClick={() => {
                this.queryParams.keywords = null;
                this.queryParams.professionId = null;
                this.queryParams.startTime = null;
                this.queryParams.endTime = null;
                this.queryParams.isCreator = false;
                this.refresh();
              }}
            >
              全部
            </a-button>
            <a-button
              class="submit-button"
              size="small"
              onClick={() => {
                this.queryParams.isCreator = true;
                this.refresh();
              }}
            >
              我填报的
            </a-button>
            {vIf(
              <a-button type="primary" size="small" onClick={() => this.add()}>
              pdf导出
              </a-button>,
              vP(this.permissions, permissionsSmschedule.Diarys.PdfExport),
            )}
            {vIf(
              <a-button
                type="primary"
                size="small"
                disabled={this.totalCount.length === 0}
                onClick={() => this.export()}
              >
              execl导出
              </a-button>,
              vP(this.permissions, permissionsSmschedule.Diarys.ExcelExport),
            )}
            {vIf(
              <a-button
                type="primary"
                size="small"
                disabled={this.totalCount.length === 0}
                onClick={() => this.authorization()}
              >
              审批单
              </a-button>,
              vP(this.permissions, permissionsSmschedule.Diarys.Examination),
            )}
            {vIf(
              <a-button type="primary" size="small" onClick={() => this.logStatistics()}>
              日志统计
              </a-button>,
              vP(this.permissions, permissionsSmschedule.Diarys.LogStatistics),
            )}
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          rowSelection={{
            onChange: (selectedRowKeys, recordList) => {
              this.recordList = recordList;
              this.selectedRowKeys = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.diaryCode}>
                    {record.diaryCode ? (
                      <span
                        class="table-location"
                        onClick={() => {
                          // vIf(
                          this.view(record);
                          //   vP(this.permissions, permissionsSmschedule.Diarys.View),
                          // );
                        }}
                      >
                        {record.diaryCode}
                      </span>
                    ) : (
                      ''
                    )}
                  </a-tooltip>
                );
              },
              profession: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.profession.name}>
                    <span>{record.profession.name}</span>
                  </a-tooltip>
                );
              },
              location: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.location}>
                    <span>{record.location}</span>
                  </a-tooltip>
                );
              },
              content: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.name}>
                    <span>{record.name}</span>
                  </a-tooltip>
                );
              },

              creator: (text, record) => {
                return record.approvalRltMembers.length>0 && record.approvalRltMembers.filter(x => x.type === 1)[0].member 
                  ?record.approvalRltMembers.filter(x => x.type === 1)[0].member.name:'';
              },
              startTime: (text, record) => {
                let startTime =
                  moment(record.time).format('YYYY-MM-DD') != '0001-01-01'
                    ? moment(record.time).format('YYYY-MM-DD')
                    : '';
                return (
                  <a-tooltip placement="topLeft" title={startTime}>
                    <span>{startTime}</span>
                  </a-tooltip>
                );
              },

              creationTime: (text, record) => {
                let creationTime =
                  record && record.diaryCode ? moment(record.fillTime).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={creationTime}>
                    <span>{creationTime}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                return !record.diaryCode ? (
                  <a-tag color="red">未填报</a-tag>
                ) : (
                  <a-tag color="green">已填报</a-tag>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                       
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                      查看
                      </a>,
                      vP(this.permissions, permissionsSmschedule.Diarys.View),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      ( vP(this.permissions, permissionsSmschedule.Diarys.View) &&
                      vP(this.permissions, permissionsSmschedule.Diarys.Fill)) || 
                      ( vP(this.permissions, permissionsSmschedule.Diarys.View) &&
                      vP(this.permissions, permissionsSmschedule.Diarys.Update)) ,
                    )}
                    {vIf(
                      <a
                        disabled={record.diaryCode ? true : false}
                        onClick={() => {
                          this.fill(record);
                        }}
                      >
                      填报
                      </a>,
                      vP(this.permissions, permissionsSmschedule.Diarys.Fill),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      (  vP(this.permissions, permissionsSmschedule.Diarys.Update) &&
                      vP(this.permissions, permissionsSmschedule.Diarys.Fill)) || 
                      ( vP(this.permissions, permissionsSmschedule.Diarys.View) &&
                      vP(this.permissions, permissionsSmschedule.Diarys.Update)) ,
                    )}
                    {vIf(
                      <a
                        disabled={record.diaryCode ? false : true}
                        onClick={() => {
                          this.edit(record);
                        }}
                      >
                      编辑
                      </a>,
                      vP(this.permissions, permissionsSmschedule.Diarys.Update),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 分页器 */}
        {/* 审批单模态框 */}
        <SmScheduleDiaryModal ref="SmScheduleDiaryModal" axios={this.axios} />
        <SmScheduleDiaryAuthorizationModal
          ref="SmScheduleDiaryAuthorizationModal"
          axios={this.axios}
        />
        <a-pagination
          style="float:right; margin-top:10px"
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
  },
};
