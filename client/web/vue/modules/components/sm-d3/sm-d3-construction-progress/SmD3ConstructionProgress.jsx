
/**
 * 说明：进度模拟
 * 作者：easten
 */
import './style/index';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ConstructType } from '../../_utils/enum';
import ApiPlan from '../../sm-api/sm-construction/ApiPlan';
import ApiProject from '../../sm-api/sm-project/Project';
import SmConstructionPlanWithGantt from '../../sm-construction/sm-construction-plan-with-gantt/SmConstructionPlanWithGantt';
let apiProject = new ApiProject();// 项目api 查类型用
let apiPlan = new ApiPlan();
export default {
  name: 'SmD3ConstructionProgress',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object,
      default: () => {
        return { left: '280px', bottom: '20px', right: "20px" };
      },
    },
    height: { type: String, default: '40%' },
    // width: { type: String, default: '840px' },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iVisible: false,
      shwoInfo: false,
      dataSource: [],
      loading: false,
      totalCount: 0,
      pageIndex: 1,
      record: null,
      constructType: ConstructType.Civil,
      maxResultCount: paginationConfig.defaultPageSize,
      tabKey: '进度模拟',
      projects: [],// 项目
      queryParams: {
        searchKey: '', // 模糊搜索
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        waiting: true,  // 查询待我审批
        approval: this.approval, // 审批相关查询
      },
      list: [], // table 数据源
      selectedPlanId: null,
    };
  },

  computed: {

    columns() {
      let baseColumns = [
        // {
        //   title: '序号', dataIndex: 'id', width: 50, customRender: (text, record, index) => {
        //     let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
        //     return (<span>{result}</span>);
        //   },
        // },
        {
          title: '计划名称', dataIndex: 'name', width: 180, ellipsis: true,
          customRender: (text, record, index) => {
            if (this.approval || this.onlyQuery) {
              return <a-tooltip title={text} placement='topLeft'>
                <a-button size='small' style='padding:0px;' type='link' onClick={() => {
                  this.$refs.PlanApprovalModal.view(record.id);
                }}>{text}</a-button>
              </a-tooltip>;
            }
            return <span>{text}</span>;
          },
        },
      ];
      return baseColumns;
    },
  },
  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.getProject();
  },
  methods: {
    initAxios() {
      apiPlan = new ApiPlan(this.axios);
      apiProject = new ApiProject(this.axios);
    },
    async refresh() {
      this.list = [];
      this.selectedEntity = undefined;
      this.loading = true;
      let res = await apiPlan.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        onlyPass: this.onlyQuery, // 只查询的话就只能查 已审核的数据
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // 考虑 第二页只有一条数据的情况,删除这个数据 会导致 当前第二页是空的,实际上应该跳转到第一页才行
        if (this.list.length === 0 && this.queryParams.pageIndex !== 1) {
          this.queryParams.pageIndex = 1;
          await this.refresh();
        }
        this.queryParams.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },
    // 获取项目
    async getProject() {
      let res = await apiProject.getList({ maxResultCount: 999 });
      if (requestIsSuccess(res) && res.data) {
        this.projects = res.data.items;
      }
    },
    // 分页事件
    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
    customRow() { // return 一个 lambda
      return (record, index) => ({
        on: { // 事件
          click: (event) => {
            console.log(record);
          },
        },
      });
    },
  },
  render() {
    return (
      <sc-panel
        class="d3-progress"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        height={this.height}
        // width={this.width}
        animate="bottomEnter"
        forceRender
        icon="alert"
        resizable
        title="进度模拟"
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <div class="sm-d3-construction-progress">
          <div class="progress-left">
            {/* 展示项目及计划 */}
            <div>
              {/* 计划列表 */}
              <a-table
                scroll={{ y: 270 }}
                size='small'
                dataSource={this.list}
                rowKey={record => record.id}
                columns={this.columns}
                customRow={this.customRow}
                loading={this.loading}
                bordered={true}
                pagination={false}
                rowSelection={{
                  type: 'radio',
                  //selectedRowKeys: this.iSelectPlanIds,
                  onChange: (selectedRowKeys, selectedRows) => {
                    this.iSelectPlanIds = selectedRowKeys;
                    this.selectedPlanId = selectedRowKeys[0];
                    this.selectedEntity = this.list.find(x => x.id === selectedRowKeys[0]);
                  },
                }}
              />
              <a-pagination
                size='small'
                style='margin-top:10px; text-align: right;'
                total={this.queryParams.totalCount}
                pageSize={this.queryParams.maxResultCount}
                current={this.queryParams.pageIndex}
                onChange={this.onPageChange}
                onShowSizeChange={this.onPageChange}
                showSizeChanger={false}
                showQuickJumper={false}
                showTotal={paginationConfig.showTotal}
              />
            </div>
          </div>
          <div class="progress-right">
            {/* 展示甘特图 */}
            <SmConstructionPlanWithGantt
              single
              isApproval
              axios={this.axios}
              planId={this.selectedPlanId}
              onChange={(data)=>this.$emit("change",data)}
              onInit={(data)=>this.$emit("init",data)}
            />
          </div>
        </div >
      </sc-panel >
    );
  },
};
