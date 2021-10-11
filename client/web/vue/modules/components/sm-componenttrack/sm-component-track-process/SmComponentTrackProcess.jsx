import './style';
import permissionsSafe from '../../_permissions/sm-safe';
import { requestIsSuccess, vP, vIf, getNodeTypeTitle } from '../../_utils/utils';
import ApiTrackProcess from '../../sm-api/sm-componenttrack/trackProcess';
import SmComponentTrackProcessModal from './SmComponentTrackProcessModal';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
let apiTrackProcess = new ApiTrackProcess();

export default {
  name: 'SmComponentTrackProcess',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      recordList: [],
      selectedRowKeys: [],
      pageIndex: 1,
      queryParams: {
        name: null,
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
          width: 130,
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          width: 130,
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '计划节点',
          ellipsis: true,
          dataIndex: 'nodes',
          scopedSlots: { customRender: 'nodes' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
          scopedSlots: { customRender: 'remark' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 130,
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
      apiTrackProcess = new ApiTrackProcess(this.axios);
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
      let response = await apiTrackProcess.getList({
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
      this.loading = false;
    },
    handleNode(node) {


    },
    // 编辑
    edit(record) {
      this.$refs.SmComponentTrackProcessModal.edit(record);
    },

    // 编辑
    add(record) {
      this.$refs.SmComponentTrackProcessModal.add(record);
    },
    // 删除
    remove(id) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => (
          <div style="color:red;">
            {tipsConfig.remove.content}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiTrackProcess.delete(id);
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
      <div class="sm-component-track-process">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.name = null;
            this.refresh();
          }}
        >

          <a-form-item label="计划名称">
            <a-input
              placeholder='请输入计划名称'
              value={this.queryParams.keyword}
              onPressEnter={this.refresh}
              allowClear={true}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" onClick={() => this.add()}>
                  添加
                </a-button>,
                vP(this.permissions, permissionsSafe.SafeProblems.Detail),
              )}
            </div>
          </template>
        </sc-table-operator>

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
              name: (text, record) => {
                let name = record && record.name ? record.name : '';
                return (
                  <a-tooltip placement="topLeft" title={name}>
                    <span>{name}</span>
                  </a-tooltip>
                );
              },
              nodes: (text, record) => {
                let result = '';
                if (record && record.nodes && record.nodes.length > 1) {
                  result = record.nodes.reduce((pre, current) => {
                    if(typeof pre=="object"){
                      return getNodeTypeTitle(pre.nodeType) + '>' + getNodeTypeTitle(current.nodeType);
                    }else{
                      return pre + '>' + getNodeTypeTitle(current.nodeType);
                    }
                   
                  });
                }
                if (record && record.nodes && record.nodes.length == 1) {
                  result = getNodeTypeTitle(record.nodes[0].nodeType);
                }
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              remark: (text, record) => {
                let result = record && record.remark ? record.remark : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >编辑
                      </a>,
                      vP(this.permissions, permissionsSafe.SafeProblems.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      (vP(this.permissions, permissionsSafe.SafeProblems.Detail)
                        && vP(this.permissions, permissionsSafe.SafeProblems.Detail)
                      ),
                    )}
                    {
                      vIf(
                        <a
                          onClick={() => {
                            this.remove(record.id);
                          }}
                        >
                          删除
                        </a>,
                        vP(this.permissions, permissionsSafe.SafeProblems.Detail),
                      )
                    }
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
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
        {/* 添加/编辑模板 */}
        <SmComponentTrackProcessModal
          ref="SmComponentTrackProcessModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
