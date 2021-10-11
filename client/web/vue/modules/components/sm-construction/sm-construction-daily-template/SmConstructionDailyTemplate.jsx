
import './style';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiDailyTemplate from '../../sm-api/sm-construction/DailyTemplate';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionDailyTemplateModal from './SmConstructionDailyTemplateModal';
import * as  permissionsConstruction from '../../_permissions/sm-construction';
let apiDailyTemplate = new ApiDailyTemplate();

export default {
  name: 'SmConstructionDailyTemplate',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      queryParams: {
        keyWords: null,  
        maxResultCount: paginationConfig.defaultPageSize, 
        pageIndex: 1, 
        totalCount: 0, 
      },
      dataSource: [], 
      loading: false,   
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '模板名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '模板描述',
          ellipsis: true,
          dataIndex: 'description',
          scopedSlots: { customRender: 'description' },
        },
        {
          title: '状态',
          ellipsis: true,
          dataIndex: 'isDefault',
          scopedSlots: { customRender: 'isDefault' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 200,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiDailyTemplate = new ApiDailyTemplate(this.axios);
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
      let response = await apiDailyTemplate.getList({
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
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionDailyTemplateModal.add();
    },
    // 添加(打开添加模态框)
    edit(record) {
      this.$refs.SmConstructionDailyTemplateModal.edit(record);
    },
    // 编辑(打开编辑模态框)
    view(record) {
      this.$refs.SmConstructionDailyTemplateModal.view(record);
    },
    // 设为模板
    async setDefault(record) {
      let response = await apiDailyTemplate.setDefault(record.id);
      if (utils.requestIsSuccess(response)) {
        this.$message.success('操作成功');
      }
      this.refresh();
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiDailyTemplate.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    return (
      <div class="sm-construction-dispatch-template">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear={true}
              axios={this.axios}
              placeholder={'请输入模板名称或者模板描述'}
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">

            {vIf(<a-button type="primary" onClick={() => this.add()}>
              新增
            </a-button>,
            vP(this.permissions, permissionsConstruction.DailyTemplates.Create),
            )}
          </template>
        </sc-table-operator>
        {/*展示区*/}
        <a-table
          dataSource={this.dataSource}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              name: (text, record) => {
                let result = record ? record.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    {result}
                  </a-tooltip>
                );
              },
              description: (text, record) => {
                let result = record ? record.description : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              isDefault: (text, record) => {
                let result = record && record.isDefault ?
                  <a-tag color="blue">
                    默认模板
                  </a-tag> : '';
                return result;
              },
              operations: (text, record) => {
                return  (
                  <span>
                    {vIf(
                      <a onClick={() => this.edit(record)}>编辑</a>,
                      vP(this.permissions, permissionsConstruction.DailyTemplates.Update),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      (vP(this.permissions, permissionsConstruction.DailyTemplates.Update) &&
                          vP(this.permissions, permissionsConstruction.DailyTemplates.Delete)) ||
                        (vP(this.permissions, permissionsConstruction.DailyTemplates.Update) && !record.isDefault &&
                          vP(this.permissions, permissionsConstruction.DailyTemplates.UpdateDefault)),
                    )}
                    {vIf(
                      <a
                        onClick={() => {
                          this.remove(record);
                        }}
                      >
                          删除
                      </a>,
                      vP(this.permissions, permissionsConstruction.DailyTemplates.Delete),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      (vP(this.permissions, permissionsConstruction.DailyTemplates.Delete)  && !record.isDefault &&
                      vP(this.permissions, permissionsConstruction.DailyTemplates.UpdateDefault)),
                    )}
                    {vIf(
                      <a
                        onClick={() => {
                          this.setDefault(record);
                        }}
                      >
                          设为默认
                      </a>,
                      !record.isDefault &&
                      vP(this.permissions, permissionsConstruction.DailyTemplates.UpdateDefault),
                    )}
                  </span>
                );},
            },
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

        {/*添加/编辑模板*/}
        <SmConstructionDailyTemplateModal
          ref='SmConstructionDailyTemplateModal'
          axios={this.axios}
          onSuccess={async () => {
            this.dataSource = []; // 有树状table的话,给数组清空,不然table 默认不展开 ……
            await this.refresh();
          }}
        />
      </div>
    );
  },



};
