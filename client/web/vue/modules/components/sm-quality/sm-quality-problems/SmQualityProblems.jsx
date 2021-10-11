import './style';
import permissionsQuality from '../../_permissions/sm-quality';

import { requestIsSuccess, vP, vIf, getSafeProblemState, getQualityProblemType, getQualityProblemLevel } from '../../_utils/utils';
import { SafeProblemState, SafeFilterType, ModalStatus } from '../../_utils/enum';
import ApiProblem from '../../sm-api/sm-quality/QualityProblem';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmQualityProblemImproveModal from './SmQualityProblemImproveModal';
import SmQualityProblemVerifieModal from './SmQualityProblemVerifieModal';
import SmQualityProblemModal from './SmQualityProblemModal';
import SmQualityProblemReportModal from './SmQualityProblemReportModal';
import DataEnum from './src/SmSystemDataEnumTreeSelect';
import moment from 'moment';

let apiProblem = new ApiProblem();

export default {
  name: 'SmQualityProblems',
  props: {
    isD3: { type: Boolean, default: false },
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      pageIndex: 1,
      iIsD3: false, //是否三维里面的接口
      queryParams: {
        title: null,
        type: undefined,
        startTime: null,
        endTime: null,
        filterType: SafeFilterType.All,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dateRange: [],
    };
  },
  computed: {
    columns() {
      let SafeType = this.queryParams.filterType == SafeFilterType.MyWaitingVerify ? true : false;
      return this.iIsD3 ? [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 80,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '标题',
          dataIndex: 'title_',
          ellipsis: true,
          scopedSlots: { customRender: 'title_' },
        },
        {
          title: '问题等级',
          ellipsis: true,
          dataIndex: 'level',
          scopedSlots: { customRender: 'level' },
        },
        {
          title: '检查时间',
          ellipsis: true,
          dataIndex: 'checkTime',
          scopedSlots: { customRender: 'checkTime' },
        },
        {
          title: '检查人',
          dataIndex: 'checker',
          ellipsis: true,
          scopedSlots: { customRender: 'checker' },
        },
        {
          title: '责任人',
          dataIndex: 'responsibleUser',
          ellipsis: true,
          scopedSlots: { customRender: 'responsibleUser' },
        },
        {
          title: '状态',
          dataIndex: 'state',
          ellipsis: true,
          width: 80,
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '定位',
          dataIndex: 'operations',
          width: 110,
          scopedSlots: { customRender: 'operations' },
        },
      ] :
        [
          {
            title: '序号',
            dataIndex: 'index',
            ellipsis: true,
            width: 80,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '标题',
            dataIndex: 'title_',
            ellipsis: true,
            scopedSlots: { customRender: 'title_' },
          },
          {
            title: '所属专业',
            ellipsis: true,
            dataIndex: 'profession',
            scopedSlots: { customRender: 'profession' },
          },
          {
            title: '问题等级',
            ellipsis: true,
            dataIndex: 'level',
            scopedSlots: { customRender: 'level' },
          },
          {
            title: '问题类型',
            ellipsis: true,
            dataIndex: 'type',
            scopedSlots: { customRender: 'type' },
          },
          {
            title: '检查时间',
            ellipsis: true,
            dataIndex: 'checkTime',
            scopedSlots: { customRender: 'checkTime' },
          },
          {
            title: SafeType ? '整改时间' : '限制整改时间',
            dataIndex: 'limitTime',
            ellipsis: true,
            scopedSlots: { customRender: 'limitTime' },
          },
          {
            title: '检查人',
            dataIndex: 'checker',
            ellipsis: true,
            scopedSlots: { customRender: 'checker' },
          },
          {
            title: '责任人',
            dataIndex: 'responsibleUser',
            ellipsis: true,
            scopedSlots: { customRender: 'responsibleUser' },
          },
          {
            title: '检查单位',
            ellipsis: true,
            dataIndex: 'checkUnitName',
            scopedSlots: { customRender: 'checkUnitName' },
          },
          {
            title: '责任单位',
            dataIndex: 'responsibleUnit',
            ellipsis: true,
            scopedSlots: { customRender: 'responsibleUnit' },
          },
          {
            title: '状态',
            dataIndex: 'state',
            ellipsis: true,
            width: 80,
            scopedSlots: { customRender: 'state' },
          },
          {
            title: '操作',
            dataIndex: 'operations',
            width: 110,
            scopedSlots: { customRender: 'operations' },
          },
        ];
    },
  },
  watch: {
    isD3: {
      handler: function (val, oldVal) {
        this.iIsD3 = val;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmQualityProblemModal.add();
    },
    //整改improve
    improve(record) {
      this.$refs.SmQualityProblemImproveModal.improve(record);
    },
    //验证
    verifie(record) {
      this.$refs.SmQualityProblemVerifieModal.verifie(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmQualityProblemModal.edit(record);
    },
    //查看
    view(record) {
      this.$refs.SmQualityProblemReportModal.view(record);

    },
    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiProblem.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.record = record;
              _this.$message.success('操作成功');
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiProblem.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
        startTime: this.dateRange.length > 0 ? moment(this.dateRange[0]).hours(0).minutes(0).seconds(0).format('YYYY-MM-DD HH:mm:ss') : '',
        endTime: this.dateRange.length > 0 ? moment(this.dateRange[1]).hours(23).minutes(59).seconds(59).format('YYYY-MM-DD HH:mm:ss') : '',
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
      this.loading = false;
    },
    getColor(type) {
      let color;
      switch (type) {
      case SafeProblemState.WaitingImprove: color = 'red';
        break;
      case SafeProblemState.WaitingVerifie: color = '#2b2de4de';
        break;
      case SafeProblemState.Improved: color = '#30d430';
        break;
      }
      return color;
    },
    /* 页面呈现内容分类 */
    changeItem(e) {
      this.queryParams.filterType = e.target.value;
      this.refresh();
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
      <div class="sm-quality-problem">
        {!this.iIsD3 ?
          <div class="sm-quality-problem-action">
            {/* 筛选 */}
            <sc-table-operator
              size={this.iIsD3 ? 'small' : 'default'}
              onSearch={() => {
                this.refresh();
              }}
              onReset={() => {
                this.queryParams.title = ''; // 关键字
                this.queryParams.startTime = null;
                this.queryParams.endTime = null;
                this.queryParams.type = undefined; // 检测类型
                this.dateRange = [];
                this.refresh();
              }}
            >
              <a-form-item label="关键字">
                <a-input
                  size={this.iIsD3 ? 'small' : 'default'}
                  placeholder="请输入标题"
                  value={this.queryParams.title}
                  onInput={event => {
                    this.queryParams.title = event.target.value;
                    this.refresh();
                  }}
                />
              </a-form-item>
              <a-form-item label="检查日期">
                <a-range-picker
                  style="width:100%"
                  value={this.dateRange}
                  onChange={value => {
                    this.dateRange = value;
                    this.refresh();
                  }}
                />
              </a-form-item>
              <a-form-item label="问题类型">
                <DataEnum
                  disabled={this.status == ModalStatus.View}
                  placeholder="请选择问题类型"
                  enum="QualityProblemType"
                  utils="getQualityProblemType"
                  value={this.queryParams.type}
                  onChange={value => {
                    this.queryParams.type = value;
                    this.refresh();
                  }}
                />
              </a-form-item>
              <template slot="buttons">
                {vIf(
                  <a-button type="primary" onClick={() => this.add()}>
                    标记
                  </a-button>,
                  vP(this.permissions, permissionsQuality.QualityProblems.Create),
                )}
              </template>
            </sc-table-operator>
            <div class="sm-quality-problem-item" style="margin-bottom:10px;">
              <a-radio-group defaultValue={SafeFilterType.All} onChange={this.changeItem}>
                <a-radio value={SafeFilterType.All}>
                  全部
                </a-radio>
                <a-radio value={SafeFilterType.MyChecked}>
                  我检查的
                </a-radio>
                <a-radio value={SafeFilterType.MyWaitingImprove}>
                  待我整改
                </a-radio>
                <a-radio value={SafeFilterType.MyWaitingVerify}>
                  待我验证
                </a-radio>
                <a-radio value={SafeFilterType.CopyMine}>
                  抄送我的
                </a-radio>
              </a-radio-group>
            </div>
          </div> : ''}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          size={this.iIsD3 ? 'small' : 'default'}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              title_: (text, record) => {
                let title = record && record.title ? record.title : '';
                return (
                  <a-tooltip placement="topLeft" title={title}>
                    <span> {this.iIsD3 ? <a
                      onClick={() => {
                        this.view(record);
                      }}
                    >{title}
                    </a> : <span>{title}</span>}</span>
                  </a-tooltip>
                );
              },
              profession: (text, record, index) => {
                let profession = record.profession ? record.profession.name : '';
                return (
                  <a-tooltip placement="topLeft" title={profession}>
                    <span>{profession}</span>
                  </a-tooltip>
                );
              },
              level: (text, record, index) => {
                let level = record.level ? getQualityProblemLevel(record.level) : '';
                return (
                  <a-tooltip placement="topLeft" title={level}>
                    <span>{level}</span>
                  </a-tooltip>
                );
              },
              type: (text, record, index) => {
                let type = record.type ? getQualityProblemType(record.type) : '';
                return (
                  <a-tooltip placement="topLeft" title={type}>
                    <span>{type}</span>
                  </a-tooltip>
                );
              },

              checkTime: (text, record) => {
                let result = record && record.checkTime ? moment(record.checkTime).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              limitTime: (text, record) => {
                let SafeType = this.queryParams.filterType == SafeFilterType.MyWaitingVerify ? true : false;
                let result = record && record.limitTime ? moment(record.limitTime).format('YYYY-MM-DD') : '';
                let result_ = record && record.changeTime ? moment(record.changeTime).format('YYYY-MM-DD') : '';
                SafeType ? result = result_ : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              checker: (text, record) => {
                let result = record && record.checker ? record.checker.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              responsibleUser: (text, record) => {
                let result = record && record.responsibleUser ? record.responsibleUser.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              checkUnitName: (text, record) => {
                let result = record && record.checkUnitName ? record.checkUnitName : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              responsibleUnit: (text, record) => {
                let result = record && record.responsibleOrganization ? record.responsibleOrganization.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                let result = record && record.state ? getSafeProblemState(record.state) : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span style={{ color: this.getColor(record.state) }}>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return !this.iIsD3 ? [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >查看
                      </a>,
                      vP(this.permissions, permissionsQuality.QualityProblems.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      (vP(this.permissions, permissionsQuality.QualityProblems.Detail) &&
                        ((this.queryParams.filterType == SafeFilterType.MyChecked && record.state == SafeProblemState.WaitingImprove) && vP(this.permissions, permissionsQuality.QualityProblems.Update)) ||
                        ((this.queryParams.filterType == SafeFilterType.MyWaitingImprove && record.state == SafeProblemState.WaitingImprove && vP(this.permissions, permissionsQuality.QualityProblems.Improve))
                          || (this.queryParams.filterType == SafeFilterType.MyWaitingVerify && record.state == SafeProblemState.WaitingVerifie && vP(this.permissions, permissionsQuality.QualityProblems.Verify)))
                      ),
                    )}
                    {this.queryParams.filterType == SafeFilterType.MyWaitingImprove &&
                      record.state == SafeProblemState.WaitingImprove ?
                      vIf(
                        <a
                          onClick={() => {
                            this.improve(record);
                          }}
                        >
                          整改
                        </a>,
                        vP(this.permissions, permissionsQuality.QualityProblems.Improve),
                      ) : ''

                    }
                    {
                      this.queryParams.filterType == SafeFilterType.MyWaitingVerify &&
                        record.state == SafeProblemState.WaitingVerifie ?
                        vIf(
                          <a
                            onClick={() => {
                              this.verifie(record);
                            }}
                          >
                            验证
                          </a>,
                          vP(this.permissions, permissionsQuality.QualityProblems.Verify),
                        ) : ''
                    }
                    {vIf(
                      <a-divider type="vertical" />,
                      ((this.queryParams.filterType == SafeFilterType.MyChecked && record.state == SafeProblemState.WaitingImprove) && vP(this.permissions, permissionsQuality.QualityProblems.Update) &&
                        ((this.queryParams.filterType == SafeFilterType.MyWaitingImprove && record.state == SafeProblemState.WaitingImprove && vP(this.permissions, permissionsQuality.QualityProblems.Improve))
                          || (this.queryParams.filterType == SafeFilterType.MyWaitingVerify && record.state == SafeProblemState.WaitingVerifie && vP(this.permissions, permissionsQuality.QualityProblems.Verify)))
                      ),
                    )}
                    {
                      this.queryParams.filterType == SafeFilterType.MyChecked && record.state == SafeProblemState.WaitingImprove ?
                        vIf(
                          <a
                            onClick={() => {
                              this.edit(record);
                            }}
                          >编辑
                          </a>,
                          vP(this.permissions, permissionsQuality.QualityProblems.Update),
                        ) : ''}
                  </span>,
                ] : [
                  <span class="position_">
                    {record.equipments && record.equipments.length > 0 ? record.equipments.map(item => {
                      return <a
                        class="position_a"
                        onClick={() => {
                          this.$emit('flyTo', {
                            groupName: item.equipment.group
                              ? item.equipment.group.name
                              : '',
                            name: item.equipment.name,
                            state: item.state,
                          });
                        }}
                      >
                        {item.equipment ? item.equipment.name : ''}
                      </a>;

                    }) : ''}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmQualityProblemModal
          ref="SmQualityProblemModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 问题整改模板 */}
        <SmQualityProblemImproveModal
          ref="SmQualityProblemImproveModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 问题报告模板 */}
        <SmQualityProblemReportModal
          ref="SmQualityProblemReportModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

        {/* 问题验证模板 */}
        <SmQualityProblemVerifieModal
          ref="SmQualityProblemVerifieModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 分页器 */}
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
