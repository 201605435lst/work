
import './style';
import { requestIsSuccess, getDispatchState, vIf, vP } from '../../_utils/utils';
import { DispatchState } from '../../_utils/enum';
import ApiDispatch from '../../sm-api/sm-construction/ApiDispatch';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionDispatchModal from './SmConstructionDispatchModal';
import SmBpmWorkflowView from '../../sm-bpm/sm-bpm-workflow-view';
import * as permissionsSmConstruction from '../../_permissions/sm-construction';
import moment from 'moment';
import FileSaver from 'file-saver';


let apiDispatch = new ApiDispatch();

export default {
  name: 'SmConstructionDispatchs',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    approval: { type: Boolean, default: false },//是否审批
    passed: { type: Boolean, default: false },//是否查询
    isForDaily: { type: Boolean, default: false },//是否用于施工日志填报
    showSelectRow: { type: Boolean, default: true }, // 是否显示选择栏
    permissions: { type: Array, default: () => [] },
    isSimple: { type: Boolean, default: false },//是否精简模式
    iMultiple: { type: Boolean, default: false },//是否多选
    selected: { type: Array, default: () => [] },//所选派工
    advancedCount: { type: Number, default: 6 },
  },
  data() {
    return {
      dispatchList: [], // table 数据源
      loading: false, // table 是否处于加载状态
      multiple: false, // table Row 是否是 多选模式
      selectDispatchIds: [], // 选择的 派工 ids (选择框模式的时候用)
      selectedEntity: undefined, // 选择的实体(派工单)
      totalCount: 0,
      iSelected: [],//已选派工
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyword: null, // 模糊搜索
        startTime: null, //开始时间
        endTime: null, //结束时间
        approval: this.approval, //是否审批
        waiting: true,
        passed: this.passed,
        isForDaily: this.isForDaily,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dateRange: [],
    };
  },
  computed: {
    btnDisable() { // 按钮 是否 禁用
      if (this.selectedEntity === undefined) { // 没有选择任何东西  的话
        return true;
      }
      return this.selectedEntity.state === DispatchState.Pass || this.selectedEntity.state === DispatchState.OnReview;
    },

    editBtnVisible() { // 控制编辑按钮隐藏
      if (this.approval) return false;
      if (this.passed) return false;
      return true;
    },
    iShowSelection() { // 控制 选择小圆点隐藏
      if (this.approval) return false;
      if (this.passed && !this.showSelectRow) return false;
      return this.showSelectRow;
    },
    columns() {
      return this.isSimple ?
        [
          {
            title: '序号',
            dataIndex: 'index',
            ellipsis: true,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '派工单号',
            dataIndex: 'code',
            ellipsis: true,
            scopedSlots: { customRender: 'code' },
          },
          {
            title: '施工专业',
            ellipsis: true,
            dataIndex: 'profession',
          },
          {
            title: '承包商',
            ellipsis: true,
            dataIndex: 'contractor.name',
          },
          {
            title: '提交人',
            ellipsis: true,
            dataIndex: 'creator.name',
          },
          {
            title: '派工时间',
            ellipsis: true,
            dataIndex: 'time',
            scopedSlots: { customRender: 'time' },
          },
        ] : [
          {
            title: '序号',
            dataIndex: 'index',
            ellipsis: true,
            width: 100,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '派工单号',
            dataIndex: 'code',
            ellipsis: true,
            scopedSlots: { customRender: 'code' },
          },
          {
            title: '派工单名称',
            dataIndex: 'name',
            ellipsis: true,
          },
          {
            title: '施工专业',
            ellipsis: true,
            dataIndex: 'profession',
          },
          {
            title: '承包商',
            ellipsis: true,
            dataIndex: 'contractor.name',
          },
          {
            title: '提交人',
            ellipsis: true,
            dataIndex: 'creator.name',
          },
          {
            title: '派工时间',
            ellipsis: true,
            dataIndex: 'time',
            scopedSlots: { customRender: 'time' },
          },
          {
            title: '状态',
            dataIndex: 'state',
            scopedSlots: { customRender: 'state' },
          },
          {
            title: '操作',
            dataIndex: 'operations',
            width: 120,
            scopedSlots: { customRender: 'operations' },
          },
        ];
    },
  },
  watch: {
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.selectDispatchIds = value.map(item => item.id);
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiDispatch = new ApiDispatch(this.axios);
    },
    // 刷新获取list 
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let res = await apiDispatch.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
        startTime: this.dateRange.length > 0 ? moment(this.dateRange[0]).hours(0).minutes(0).seconds(0).format('YYYY-MM-DD HH:mm:ss') : '',
        endTime: this.dateRange.length > 0 ? moment(this.dateRange[1]).hours(23).minutes(59).seconds(59).format('YYYY-MM-DD HH:mm:ss') : '',
      });
      if (requestIsSuccess(res) && res.data) {
        this.dispatchList = res.data.items;
        this.totalCount = res.data.totalCount;
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

    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    // 添加(打开添加模态框)
    add() {
      this.$emit('add');
    },

    // 编辑(打开编辑模态框)
    edit(record) {
      this.$emit(
        'edit',
        record.id,
      );
    },

    // 详情(打开详情模态框)
    view(record) {
      this.$emit(
        'view',
        record.id,
      );
    },

    // 导出派工单
    async export(record) {
      let response = await apiDispatch.export(record.id);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          this.$message.info('导出成功');
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `派工单明细.docx`,
          );
        }
      }
    },

    //派工单审批
    process(record) {
      this.$emit(
        'process',
        record.id,
      );
    },

    // 流程信息查看
    workflowView(record) {
      this.$refs.workflowViewer.view(record.workflowId);
    },

    // 派工单审批
    submit(record) {
      this.$refs.SmConstructionDispatchModal.submit(record);
    },

    remove() {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiDispatch.delete(_this.selectDispatchIds);
            if (requestIsSuccess(response)) {
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

    //获取派工单审批状态颜色
    getDispatchStateColor(status) {
      let title = '';
      switch (status) {
      case DispatchState.UnSubmit:
        title = '#bfbfbf';//待提交
        break;
      case DispatchState.OnReview:
        title = '#faad14';//审核中
        break;
      case DispatchState.Pass:
        title = '#61d023';//已通过
        break;
      case DispatchState.UnPass:
        title = '#ff5500';//已驳回
        break;
      default:
        break;
      }
      return title;
    },

    // 切换 选择模式
    switchSelectType() {
      this.multiple = !this.multiple;
      this.selectDispatchIds = []; // 同时清空选择的东西
      this.selectedEntity = undefined;
      this.refresh();
    },

    // 审批记录切换
    tabChange(key) {
      this.queryParams.waiting = key === 1;
      this.refresh();
    },
    //更新所选数据
    updateSelected(selectedRows) {
      if (this.iMultiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.dispatchList.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectDispatchIds) {
          let _dispatch = this.dispatchList.find(item => item.id === id);
          if (!!_dispatch) {
            _selected.push(JSON.parse(JSON.stringify(_dispatch)));
          }
        }

        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }

      this.$emit('change', this.iSelected);
    },
  },
  render() {
    return (
      <div class="sm-construction-dispatch">
        {/* 操作区 */}
        <sc-table-operator
          size={this.isSimple ? 'small' : 'default'}
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {
              keyword: '',
              approval: this.approval, //是否审批
              waiting: this.queryParams.waiting,
              passed: this.passed,
              isForDaily: this.isForDaily,
            };
            this.dateRange = [];
            this.refresh();
          }}
        >

          <a-form-item label="模糊搜索">
            <a-input
              size={this.isSimple ? 'small' : 'default'}
              placeholder="请输入编号、专业"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="派工时间">
            <a-range-picker
              size={this.isSimple ? 'small' : 'default'}
              value={this.dateRange}
              onChange={value => {
                this.dateRange = value;
                this.refresh();
              }}
            />
          </a-form-item>
          {this.isSimple ? "" :
            <template slot='buttons'>
              {this.editBtnVisible && // 只有 不审批 或者不是 查看 的时候 这些按钮 才有,审批的时候这些按钮 消失
                [
                  vIf(
                    <a-button size='small' type='primary' icon='plus' onClick={this.add}>
                      添加
                    </a-button>,
                    vP(this.permissions, permissionsSmConstruction.Dispatch.Create),
                  ),
                  <a-button size="small" type='primary' icon={this.multiple ? 'check-circle' : 'check-square'} onClick={() => this.switchSelectType()}>
                    {this.multiple ? '切到单选' : '切到多选'}
                  </a-button>,
                  vIf(
                    <a-button size="small" type='primary' onClick={() => this.edit(this.selectedEntity)} disabled={this.btnDisable}>
                      编辑
                    </a-button>,
                    vP(this.permissions, permissionsSmConstruction.Dispatch.Update),
                  ),
                  vIf(
                    <a-button size="small" type='primary' onClick={() => this.submit(this.selectedEntity)} disabled={this.btnDisable}>
                      提交审批
                    </a-button>,
                    vP(this.permissions, permissionsSmConstruction.Dispatch.Submit),
                  ),

                  vIf(
                    <a-button size="small" type='primary' onClick={() => this.export(this.selectedEntity)} disabled={this.selectedEntity == undefined}>
                      导出
                    </a-button>,
                    vP(this.permissions, permissionsSmConstruction.Dispatch.Export),
                  ),
                  vIf(
                    <a-button
                      size="small"
                      type='danger'
                      onClick={() => this.remove()}
                      disabled={this.selectDispatchIds.length === 0 ||
                        this.selectedEntity && this.selectedEntity.state == DispatchState.Pass ||
                        this.selectedEntity && this.selectedEntity.state == DispatchState.OnReview}
                    >
                      删除
                    </a-button>,
                    vP(this.permissions, permissionsSmConstruction.Dispatch.Delete),
                  ),

                ]
              }
            </template>}
        </sc-table-operator>

        {/* 审批tab切换按钮 */}
        {this.approval &&
          <a-tabs default-active-key='1' onChange={this.tabChange} size='small'>
            <a-tab-pane key={1} tab='待我审批'>
            </a-tab-pane>
            <a-tab-pane key={2} tab='我审批的' force-render>
            </a-tab-pane>
          </a-tabs>
        }

        {/*展示区*/}
        <a-table
          dataSource={this.dispatchList}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          pagination={false}
          class="dispatch-table"
          rowSelection={this.iShowSelection || this.isSimple ? {
            selectedRowKeys: this.selectDispatchIds,
            columnWidth: 30,
            type: this.multiple ? 'checkbox' : 'radio',
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectDispatchIds = selectedRowKeys;
              if (!this.multiple) { // 单选的时候
                this.selectedEntity = this.dispatchList.find(x => x.id === selectedRowKeys[0]);
              }
              if (this.isSimple) {
                this.updateSelected(selectedRows);
              }
              this.$emit('selectedChange', selectedRowKeys); //往外冒泡 选择的ids
            },
            getCheckboxProps: record => ({
              props: {
                disabled: this.isSimple ? false : this.multiple ? record.state === DispatchState.Pass || record.state === DispatchState.OnReview : false, // Column configuration not to be checked
                name: record.name,
              },
            }),
          } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = `${index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              code: (text, record) => {
                return <a-tooltip title={text} placement='topLeft'>
                  {this.isSimple ? { text } : <a-button size='small' style='padding:0px;' type='link' onClick={() => {
                    this.view(record);
                  }}>{text}</a-button>}
                </a-tooltip>;
              },
              time: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={text ? moment(text).format('YYYY-MM-DD HH:mm:ss') : ''}>
                    <span> {text ? moment(text).format('YYYY-MM-DD HH:mm:ss') : ''}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                let result = record ? getDispatchState(record.state) : '';
                return (
                  <a-tag color={this.getDispatchStateColor(record.state)}>
                    {result}
                  </a-tag>
                );
              },
              operations: (text, record) => {
                return (
                  <div>
                    {vIf(
                      <div
                        style='display:inline'
                        onClick={() => { if (record.state !== DispatchState.UnSubmit) { this.workflowView(record); } }}
                      >
                        <a style={{ 'cursor': record.state === DispatchState.UnSubmit ? 'not-allowed' : '' }}>查看</a>
                      </div>,
                      vP(this.permissions, permissionsSmConstruction.Dispatch.Detail),
                    )}
                    {vIf(
                      record.state == DispatchState.OnReview && this.approval ?
                        [
                          <a-divider type="vertical" />,
                          <div style='display:inline' onClick={() => this.process(record)}><a>审批</a></div>,
                        ] : undefined,
                      vP(this.permissions, permissionsSmConstruction.Dispatch.Approval),
                    )}
                  </div>
                );
              },
            },
          }}
        />

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
          size={this.isSimple ? 'small' : 'default'}
          showTotal={paginationConfig.showTotal}
        />
        {/*添加/编辑模板*/}
        <SmConstructionDispatchModal
          ref='SmConstructionDispatchModal'
          axios={this.axios}
          onSuccess={() => {
            this.selectedEntity = undefined;
            this.refresh();
          }}
        />
        {/* 流程查看器 */}
        <SmBpmWorkflowView axios={this.axios} ref="workflowViewer" />
      </div>
    );
  },



};
