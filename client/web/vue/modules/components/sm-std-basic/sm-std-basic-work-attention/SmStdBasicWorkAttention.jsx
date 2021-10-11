import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiWorkAttention from '../../sm-api/sm-std-basic/WorkAttention';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
import { deleteEmptyProps } from '../../_utils/tree_array_tools';
import SmStdBasicWorkAttentionModal from './SmStdBasicWorkAttentionModal';
let apiWorkAttention = new ApiWorkAttention();
import SmStdBasicWorkAttentionTreeSelect from '../sm-std-basic-work-attention-tree-select/SmStdBasicWorkAttentionTreeSelect';
import SmStdBasicWorkAttentionTypeModal from './SmStdBasicWorkAttentionTypeModal';
import './style/index';
import moment from 'moment';
import { refresh } from 'less';

export default {
  name: 'SmStdBasicWorkAttention',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    isSimple: { type: Boolean, default: false },//是否精简模式
    multiple: { type: Boolean, default: false },//是否多选
    permissions: { type: Array, default: () => [] },
    selected: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      totalCount: 0,
      pageIndex: 1,
      isRefresh: null,//树选择是否刷新
      queryParams: {
        keyWords: null, //关键字查询
        typeId: null,//类别
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dataSource: [], // 列表数据源
      loading: false,
      iSelected: [], //已选项
      selectedIds: [],//已选项Ids
    };
  },
  computed: {
    columns() {
      return this.isSimple ? [
        {
          title: '序号',
          dataIndex: 'index',
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '类别',
          dataIndex: 'typeId',
          ellipsis: true,
          width: 180,
          scopedSlots: { customRender: 'typeId' },
        },
        {
          title: '作业注意事项',
          dataIndex: 'attention',
          ellipsis: true,
          scopedSlots: { customRender: 'attention' },
        },
      ] : [
        {
          title: '序号',
          dataIndex: 'index',
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '类别',
          dataIndex: 'typeId',
          ellipsis: true,
          width: 180,
          scopedSlots: { customRender: 'typeId' },
        },
        {
          title: '作业注意事项',
          dataIndex: 'attention',
          ellipsis: true,
          scopedSlots: { customRender: 'attention' },
        },
        {
          title: '上次编辑时间',
          dataIndex: 'editData',
          width: 180,
          ellipsis: true,
          scopedSlots: { customRender: 'editData' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 180,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    selected: {
      handler: function (value) {
        this.iSelected = value;
        this.selectedIds = value.map(item => item.id);
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
      apiWorkAttention = new ApiWorkAttention(this.axios);
    },
    // 添加
    add(record) {
      this.record = record;
      this.$refs.SmStdBasicWorkAttentionModal.add(record);
    },
    addType(record) {
      this.record = record;
      this.$refs.SmStdBasicWorkAttentionTypeModal.add(record);
    },
    // 编辑
    edit(record) {
      this.record = record;
      this.$refs.SmStdBasicWorkAttentionModal.edit(record);
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
            let response = await apiWorkAttention.delete(record.id);
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
    // 刷新列表
    async refresh(resetPage = true, page) {
      this.isRefresh = moment().format("YYYY_MM_DD_hh_mm_ss");
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        repairTagKey: this.repairTagKey,
      };
      let response = await apiWorkAttention.getList({
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
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.dataSource.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedIds) {
          let temp = this.dataSource.find(item => item.id === id);
          if (!!temp) {
            _selected.push(JSON.parse(JSON.stringify(temp)));
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
      <div class="sm-std-basic-work-attention">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.queryParams.typeId = null;
            this.refresh();
          }}
        >
          <a-form-item label="类别">
            <SmStdBasicWorkAttentionTreeSelect
              repairTagKey={this.repairTagKey}
              axios={this.axios}
              isRefresh={this.isRefresh}
              value={this.queryParams.typeId}
              placeholder="类别选择"
              onChange={value => {
                this.queryParams.typeId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="关键字">
            <a-input
              allowClear
              placeholder="请输入注意事项关键字"
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          {!this.isSimple ? <template slot="buttons">
            {vIf(
              <a-button type="primary" onClick={() => this.add()} >
                添加
              </a-button>,
              vP(this.permissions, permissionsSmStdBasic.WorkAttention.Create),
            )}
            {vIf(
              <a-button type="primary" onClick={() => this.addType()} >
                类别维护
              </a-button>,
              vP(this.permissions, permissionsSmStdBasic.WorkAttention.CreateType),
            )}
          </template> : undefined}

        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          pagination={false}
          loading={this.loading}
          rowSelection={this.isSimple ? {
            type: this.multiple ? 'checkbox' : 'radio',
            columnWidth: 30,
            selectedRowKeys: this.selectedIds,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectedIds = selectedRowKeys;
              this.updateSelected(selectedRows);
            },
          } : undefined}
          scroll={this.isSimple ? { y: 300 } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return <a-tooltip placement="topLeft" title={result}>{result}</a-tooltip>;
              },
              typeId: (text, record, index) => {
                let result = record && record.type ? record.type.content : '';
                return <a-tooltip placement="topLeft" title={result}>{result}</a-tooltip>;
              },
              attention: (text, record, index) => {
                let result = record ? record.content : '';
                return <a-tooltip placement="topLeft" title={result}>{result}</a-tooltip>;
              },
              editData: (text, record, index) => {
                let result = (record && record.lastModificationTime) ?
                  moment(record.lastModificationTime).format("YYYY-MM-DD HH:mm:ss") :
                  moment(record.creationTime).format("YYYY-MM-DD HH:mm:ss");
                return <a-tooltip placement="topLeft" title={result}>{result}</a-tooltip>;
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
                      vP(this.permissions, permissionsSmStdBasic.WorkAttention.Update),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.WorkAttention.Update) &&
                      vP(this.permissions, permissionsSmStdBasic.WorkAttention.Delete),
                    )}
                    {vIf(
                      <a
                        onClick={() => {
                          this.remove(record);
                        }}
                      > 删除
                      </a>,
                      vP(this.permissions, permissionsSmStdBasic.WorkAttention.Delete),
                    )}
                  </span>,
                ];
              },
            },
          }}
        >
          <span slot="customTitle"><a-icon type="smile-o" /> name</span>
        </a-table>
        {/* 分页器 */}
        <a-pagination
          style="margin-top:10px; text-align: right;"
          size={this.isSimple ? 'small' : ''}
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
        <SmStdBasicWorkAttentionModal
          ref="SmStdBasicWorkAttentionModal"
          axios={this.axios}
          repairTagKey={this.repairTagKey}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

        <SmStdBasicWorkAttentionTypeModal
          ref="SmStdBasicWorkAttentionTypeModal"
          repairTagKey={this.repairTagKey}
          axios={this.axios}
          onSuccess={() => {
            this.isRefresh = moment().format("YYYY_MM_DD_hh_mm_ss");
            this.queryParams.typeId = null;
            this.queryParams.keyWords = null;
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
