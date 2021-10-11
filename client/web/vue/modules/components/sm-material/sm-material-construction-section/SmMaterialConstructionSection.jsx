import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiConstructionSection from '../../sm-api/sm-material/ConstructionSection';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import permissionsConstructionSection from '../../_permissions/sm-material';
import SmMaterialConstructionSectionModal from './SmMaterialConstructionSectionModal';

import moment from 'moment';
let apiConstructionSection = new ApiConstructionSection();

export default {
  name: 'SmMaterialConstructionSection',
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
      pageIndex: 1,
      queryParams: {
        name: null, //施工区段名称
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
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '区段名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
       
        {
          title: '起始锚段',
          ellipsis: true,
          dataIndex: 'startSegment',
          scopedSlots: { customRender: 'startSegment' },
        },
        {
          title: '终止锚段',
          ellipsis: true,
          dataIndex: 'endSegment',
          scopedSlots: { customRender: 'endSegment' },
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
      apiConstructionSection = new ApiConstructionSection(this.axios);
    },
    
    // 添加
    add() {
      this.$refs.SmMaterialConstructionSectionModal.add();
    },
    // 详情
    view(record) {
      this.$refs.SmMaterialConstructionSectionModal.view(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmMaterialConstructionSectionModal.edit(record);
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
            let response = await apiConstructionSection.delete(record.id);
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
      let data = {
        ...this.queryParams,
      };
      let response = await apiConstructionSection.getList({
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

  },
  render() {
    return (
      <div class="sm-material-constructionSection">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.name = undefined;
            this.refresh();
          }}
        >
          <a-form-item label="名称">
            <a-input
              placeholder="请输入施工区段名称"
              value={this.queryParams.name}
              allowClear
              onChange={event => {
                this.queryParams.name = event.target.value;
                console.log(this.queryParams.name);
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary"  onClick={() => this.add()}>
                添加
              </a-button>,
              vP(this.permissions, permissionsConstructionSection.ConstructionSections.Create),
            )}
          </template>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record) => {
                let result=record?record.name:'';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              startSegment: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.startSegment}>
                    <span>{record.startSegment}</span>
                  </a-tooltip>
                );
              },
              endSegment: (text, record) => {
                let result=record.endSegment?record.endSegment:'';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              remark: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.remark}>
                    <span>{record.remark}</span>
                  </a-tooltip>
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
                        详情
                      </a>,
                      vP(this.permissions, permissionsConstructionSection.ConstructionSections.Detail),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsConstructionSection.ConstructionSections.Detail) &&
                      vP(this.permissions, [
                        permissionsConstructionSection.ConstructionSections.Update,
                        permissionsConstructionSection.ConstructionSections.Delete,
                      ]),
                    )}

                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.edit(record);
                                }}
                              >
                                编辑
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsConstructionSection.ConstructionSections.Update),
                          )}
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
                            vP(this.permissions, permissionsConstructionSection.ConstructionSections.Delete),
                          )}

                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsConstructionSection.ConstructionSections.Create,
                        permissionsConstructionSection.ConstructionSections.Update,
                        permissionsConstructionSection.ConstructionSections.UpdateEnable,
                        permissionsConstructionSection.ConstructionSections.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmMaterialConstructionSectionModal
          ref="SmMaterialConstructionSectionModal"
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
