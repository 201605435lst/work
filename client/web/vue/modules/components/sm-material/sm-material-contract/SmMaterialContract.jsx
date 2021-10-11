/**
 * 说明：物资合同管理
 * 作者：easten
 */
import { requestIsSuccess, getSupplierType, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiContract from '../../sm-api/sm-material/Contract';
import moment from 'moment';
import SmMaterialContractModal from './SmMaterialContractModal';
import FileSaver from 'file-saver';
import './style';
let apiContract =new ApiContract();
export default {
  name: 'SmMaterialContract',
  components: {},
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      queryParams: {
        keywords: '',
        maxResultCount: 0,
      },
      totalCount: 0,
      pageIndex: 1,
      dataSource: null,
      record: null,
      selectedRowKeys: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'xh',
          width: 60,
          ellipsis: true,
          align: 'center',
          scopedSlots: { customRender: 'xh' },
        },
        {
          title: '合同编号',
          dataIndex: 'code',
          ellipsis: true,
          width: 200,
          align: 'center',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '合同名称',
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '合同日期',
          dataIndex: 'date',
          scopedSlots: { customRender: 'date' },
          ellipsis: true,
        },
        {
          title: '合同金额（万）',
          dataIndex: 'amount',
          ellipsis: true,
        },
        {
          title: '上传人',
          dataIndex: 'creator.userName',
          ellipsis: true,
        },
        {
          title: '上传时间',
          dataIndex: 'creationTime',
          width: 200,
          ellipsis: true,
          scopedSlots: { customRender: 'creationTime' },
        },       
        {
          title: '操作',
          dataIndex: 'operations',
          width: '140px',
          align: 'center',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},
  created() {
    apiContract=new ApiContract(this.axios);
    this.refresh();
  },
  methods: {
    // 刷新
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      // 获取数据
      let response = await apiContract.getList({
        ...this.queryParams,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
      });
      if (requestIsSuccess(response)) {
        console.log(response);
        this.loading = false;
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
    },
    // 新增
    add(){
      this.$refs.materialContractModal.add();
    },
    // 删除
    delete(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          if (record) {
            return new Promise(async (resolve, reject) => {
              let response = await apiContract.delete(record.id);
              if (requestIsSuccess(response)) {
                _this.refresh(false, _this.pageIndex);
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          } else {
            return new Promise(async (resolve, reject) => {
              console.log(_this.selectedRowKeys);
              let response = await apiContract.deleteRange(_this.selectedRowKeys);
              if (requestIsSuccess(response)) {
                _this.refresh(false, _this.pageIndex);
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          }
        },
        onCancel() {},
      });
    },
    // 导出
    async export() {
      if (this.selectedRowKeys.length === 0) {
        this.$message.error('请选择需要导出的数据');
        return false;
      }
      let response = await apiContract.export(this.selectedRowKeys);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0)
        {
          this.$message.info('物资合同信息导出成功');
          this.selectedRowKeys=[];
          FileSaver.saveAs(new Blob([response.data], { type: 'application/vnd.ms-excel' }), `物资合同信息.xlsx`);
        }
      }
    },
    // 分页事件
    onPageChange() {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      this.queryParams.SkipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;
      this.refresh(false);
    },
    // 记录编辑
    edit(record) {
      this.$refs.materialContractModal.edit(record);
    },
    // 查看
    view(record) {
      this.$refs.materialContractModal.view(record);
    },
  },
  render() {
    return (
      <div class="sm-material-contract">
        <div class="head">
          <a-row>
            <a-col span={8} style='display:flex;width:100%;justify-content:space-between;'>
              <div class="m-button-container">
                <a-button
                  style="width:80px;margin-left:5px"
                  type="primary"
                  size="default"
                  onClick={() => this.add()}
                >
                  新增
                </a-button>
                <a-button
                  style="width:80px;margin-left:5px"
                  type="danger"
                  size="default"
                  onClick={() => this.delete()}
                >
                  删除
                </a-button>
                <a-button
                  style="width:80px;margin-left:5px"
                  color="red"
                  size="default"
                  onClick={() => this.export()}
                >
                  导出
                </a-button>
              </div>
              <div class='m-query-content'>
                <span class='key-text'>合同名称：</span>
                <a-input
                  placeholder="请输入查询关键字"
                  class="m-form"
                  value={this.queryParams.keywords}
                  onInput={event => {
                    this.queryParams.keywords = event.target.value;
                    this.refresh();
                  }}
                />
                <a-button
                  style="width:80px;margin-left:5px"
                  type="primary"
                  size="default"
                  onClick={() => this.refresh()}
                >
                  查询
                </a-button>
              </div>
            </a-col>
          </a-row>
        </div>
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          row-selection={{
            selectedRowKeys:this.selectedRowKeys,
            onChange:(keys,rows)=>{
              console.log(keys);
              console.log(rows);
              this.selectedRowKeys=keys;
            },
          }}
          pagination={false}
          {...{
            scopedSlots: {
              xh: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record, index) => {
                return <a onClick={()=>this.view(record)}>{text}</a>;
              },
              date:(text, record, index) => {
                return moment(record.date).format('YYYY-MM-DD');
              },
              operations: (text, record) => {
                return [
                  <span>
                    <a-button style="padding:2px" onClick={() => this.view(record)} type="link">
                      {' '}
                      查看
                    </a-button>
                  </span>,
                  <a-divider type="vertical" />,
                  <span>
                    <a-dropdown>
                      <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                        更多 <a-icon type="down" />
                      </a>
                      <a-menu slot="overlay">
                        <a-menu-item>
                          <a href="javascript:;" onClick={() => this.edit(record)}>
                            编辑
                          </a>
                        </a-menu-item>
                        <a-menu-item>
                          <a
                            href="javascript:;"
                            onClick={() => this.delete(record)}
                            style="color:red"
                          >
                            删除
                          </a>
                        </a-menu-item>
                      </a-menu>
                    </a-dropdown>
                  </span>,
                ];
              },
              entryTime: (text, record) => {
                return moment(record.entryTime).format('YYYY-MM-DD');
              },
              creationTime: (text, record) => {
                return moment(record.creationTime).format('YYYY-MM-DD HH:mm:ss');
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

        <SmMaterialContractModal
          ref="materialContractModal"
          axios={this.axios}
          onSuccess={() => this.refresh()}
        />
      </div>
    );
  },
};