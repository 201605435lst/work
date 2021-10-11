import './style';
import permissionsSafe from '../../_permissions/sm-safe';
import { requestIsSuccess, vP, vIf, getComponentTrackTypeTypeTitle } from '../../_utils/utils';
import ApiComponentQrCode from '../../sm-api/sm-componenttrack/componentQrCode';
import CrmCategoryTreeSelect from '../../sm-std-basic/sm-std-basic-component-category-tree-select';
import SmComponentQrCodeTimeLineModal from './SmComponentQrCodeTimeLineModal';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ComponentTrackTypeType } from '../../_utils/enum';
let apiComponentQrCode = new ApiComponentQrCode();

export default {
  name: 'SmComponentQrCode',
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
        componentCategoryId: null, //设备分类
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
          title: '所属设备分类',
          dataIndex: 'componentCategory',
          ellipsis: true,
          scopedSlots: { customRender: 'componentCategory' },
        },

        {
          title: '安装设备',
          ellipsis: true,
          dataIndex: 'installationEquipment',
          scopedSlots: { customRender: 'installationEquipment' },
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
      apiComponentQrCode = new ApiComponentQrCode(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.recordList = [];
      this.selectedRowKeys = [];

      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
      };
      let response = await apiComponentQrCode.getList({
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
      console.log(this.selectedRowKeys);
      this.loading = false;
    },
    // batchDefault(record) {
    //   console.log(record);
    //   if (!record && this.recordList.length == 0) {
    //     this.$message.warning('请选择你要预设的构件设备！！！');
    //     return;
    //   }
    //   let _datasource = [];
    //   _datasource = record ? [record] : this.recordList;
    //   console.log(_datasource);
    //   this.$refs.SmComponentQrCodeModal.batchDefault(_datasource);
    // },
    view(record) {
      if(record){
        if(record.installationEquipment){
          this.$refs.SmComponentQrCodeTimeLineModal.view(record);
        }
        else{
          this.$message.warning('当前二维码未绑定设备');
        }
      }else{
        this.$message.warning('请刷新页面重新尝试！！！');
      }
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    getColor(type) {
      let color;
      switch (type) {
      case ComponentTrackTypeType.Reserved:
        color = '#4ed830';
        break;
      case ComponentTrackTypeType.NoReserved:
        color = 'red';
        break;
      }
      return color;
    },
  },
  render() {
    return (
      <div class="sm-component-qr-code">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.componentCategoryId = null;
            this.refresh();
          }}
        >
          <a-form-item label="二维码分类">
            <CrmCategoryTreeSelect
              axios={this.axios}
              allowClear={true}
              value={this.queryParams.componentCategoryId}
              onInput={value => {
                this.queryParams.componentCategoryId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          {/* <a-form-item label="关键字">
            <a-input
              placeholder='请输入名称、编码'
              value={this.queryParams.keyword}
              onPressEnter={this.refresh}
              allowClear={true}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item> */}
          {/* <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" onClick={() => this.batchDefault()}>
                  批量预设
                </a-button>,
                vP(this.permissions, permissionsSafe.SafeProblems.Detail),
              )}
            </div>
          </template> */}
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
              componentCategory: (text, record) => {
                let result = record && record.generatingEquipment.componentCategory
                  ? record.generatingEquipment.componentCategory.parent
                    ? record.generatingEquipment.componentCategory.parent.name + '-' + record.generatingEquipment.componentCategory.name
                    : ''
                  : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              installationEquipment: (text, record) => {
                let name = record && record.installationEquipment && record.installationEquipment.name ? record.installationEquipment.name : '';
                return (
                  <a-tooltip placement="topLeft" title={name}>
                    <span>{name}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {record.state == ComponentTrackTypeType.Reserved ? (
                      vIf(
                        <a
                          onClick={() => {
                            this.view(record);
                          }}
                        >查看
                        </a>,
                        vP(this.permissions, permissionsSafe.SafeProblems.Detail),
                      )
                    ) : (
                      vIf(
                        <a
                          onClick={() => {
                            this.batchDefault(record);
                          }}
                        >
                          预设
                        </a>,
                        vP(this.permissions, permissionsSafe.SafeProblems.Detail),
                      ))
                    }
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板
        <SmComponentQrCodeModal
          ref="SmComponentQrCodeModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        /> */}
        {/* 详情模板 */}
        <SmComponentQrCodeTimeLineModal
          ref="SmComponentQrCodeTimeLineModal"
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
