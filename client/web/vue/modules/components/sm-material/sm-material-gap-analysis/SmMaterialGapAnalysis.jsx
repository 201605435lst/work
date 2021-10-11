
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig } from '../../_utils/config';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import moment from 'moment';
let apiEntity = new ApiEntity();

export default {
  name: 'SmMaterialGapAnalysis',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      gaps: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        startTime: null,
        endTime: null,
        maxResultCount: paginationConfig.defaultPageSize,
        plus:false, //正差
        minus:false, //负差
      },
      loading: false,
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '物资类型',
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '物资名称',
          dataIndex: 'name',
        },
        {
          title: '物资型号',
          dataIndex: 'model',
          scopedSlots: { customRender: 'model' },
        },
        {
          title: '物资规格',
          dataIndex: 'spec',
          scopedSlots: { customRender: 'spec' },
        },
        {
          title: '单位',
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '采购月份',
          dataIndex: 'time',
          scopedSlots: { customRender: 'time' },
        },
        {
          title: '需求计划量',
          dataIndex: 'needNum',
          scopedSlots: { customRender: 'needNum' },
        },
        {
          title: '采购计划量',
          dataIndex: 'purchaseNum',
          scopedSlots: { customRender: 'purchaseNum' },
        },
        {
          title: '量差',
          dataIndex: 'gap',
          scopedSlots: { customRender: 'gap' },
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
      apiEntity = new ApiEntity(this.axios);
    },
    async refresh() { },
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
      <div class="sm-material-gap-analysis">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            (this.queryParams.startTime = null), (this.queryParams.endTime = null), this.refresh();
          }}
        >
          <a-form-item label="计划时间">
            <div style="display:flex">
              <a-date-picker
                placeholder="起始时间"
                value={this.queryParams.startTime ? this.queryParams.startTime : null}
                onChange={value => {
                  this.queryParams.startTime = value ? moment(value._d).format('YYYY-MM-DD') : null;
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
          <template slot="buttons">
            <div style={'display:flex'}>
              <a-button type="primary" onClick={() => {this.queryParams.plus = false;this.queryParams.minus = false;this.refresh();}}>
                  量差分析
              </a-button>
              <a-button type="primary" onClick={() => {this.queryParams.plus = true;this.queryParams.minus = false;this.refresh();}}>
                  正差
              </a-button>
              <a-button type="primary" onClick={() => {this.queryParams.plus = false;this.queryParams.minus = true;this.refresh();}}>
                  负差
              </a-button>
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.gaps}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record) => {
                return (
                  <a
                    onClick={() => {
                      this.view(record);
                    }}
                  >
                    {record.code}
                  </a>
                );
              },
              planTime: (text, record) => {
                let planTime =
                moment(record.planTime).format('YYYY-MM-DD') != '0001-01-01'
                  ? moment(record.planTime).format('YYYY-MM-DD')
                  : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={planTime}>
                    <span>{planTime}</span>
                  </a-tooltip>
                );
              },
              creator: (text, record) => {
                return record.userName;
              },
              creationTime: (text, record) => {
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
  },
};
    