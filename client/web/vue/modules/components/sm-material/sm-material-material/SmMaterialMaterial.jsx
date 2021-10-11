
import './style';
import { requestIsSuccess, vP, vIf, initBackgroundTask } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiMaterial from '../../sm-api/sm-technology/Material';
import MaterialTypeSelect from '../sm-material-material-type-select';
import { base64toFile, SaveMultipleFile, SaveSingleFile } from '../../sm-file/sm-file-manage/src/common';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import MaterialModal from './SmMaterialMaterialModal';
import permissionsSmMaterial from '../../_permissions/sm-material';
import { PageState } from '../../_utils/enum';
import FileSaver from 'file-saver';
import ApiQRcode from '../../sm-api/sm-common/QRcode';

let apiQRcode = new ApiQRcode();
let apiMaterial = new ApiMaterial();

export default {
  name: 'SmMaterialMaterial',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      materials: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keywords: null,
        professionId: null,
        typeId: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      materialIds: [],
      visible: false,
      id: null,
      pageState: PageState.Add,
      taskKey: "material",
      progress: null,
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
          title: '材料名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '规格/型号',
          dataIndex: 'spec',
        },
        {
          title: '类别',
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '价格(元)',
          dataIndex: 'price',
          scopedSlots: { customRender: 'price' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
    this.refreshTask();
  },
  methods: {
    initAxios() {
      apiQRcode=new ApiQRcode(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
        this.materialIds = [];
      }
      let response = await apiMaterial.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.materials = response.data.items;
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
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    add() {
      this.$refs.materialModal.add();
    },

    view(record) {
      this.$refs.materialModal.view(record);
    },

    edit(record) {
      this.$refs.materialModal.edit(record);
    },

    handleMenuClick(e) {
      if (e.key == "1") {
        // 导出 所选
        this.export(this.materialIds);
      }
      if (e.key == "2") {
        this.export([]);
      }
    },
    //导出
    export(ids) {
      let _this = this;
      this.loading = true;
      //let data = { ids:this.materialIds}; *
      return new Promise(async (resolve, reject) => {
        let response = await apiMaterial.export(ids);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `材料详情表.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },

    //删除
    remove(multiple, selectedRowKeys) {
      if (selectedRowKeys && selectedRowKeys.length > 0) {
        let _this = this;
        this.$confirm({
          title: tipsConfig.remove.title,
          content: h => (
            <div style="color:red;">
              {multiple ? '确定要删除这几条数据？' : tipsConfig.remove.content}
            </div>
          ),
          okType: 'danger',
          onOk() {
            if(multiple){
              return new Promise(async (resolve, reject) => {
                let response = await apiMaterial.deleteRange(selectedRowKeys);
                if (requestIsSuccess(response)) {
                  _this.$message.success('数据已删除');
                  _this.refresh();
                  setTimeout(resolve, 100);
                } else {
                  setTimeout(reject, 100);
                }
              });
            }else{
              return new Promise(async (resolve, reject) => {
                let response = await apiMaterial.delete(selectedRowKeys);
                if (requestIsSuccess(response)) {
                  _this.$message.success('数据已删除');
                  _this.refresh();
                  setTimeout(resolve, 100);
                } else {
                  setTimeout(reject, 100);
                }
              });
            }
          },
          onCancel() { },
        });
      } else {
        this.$message.error('请选择要删除的材料！');
      }
    },
    // 同步工程数据
    async synchronize() {
      // 进度

      let _this = this;
      apiMaterial.synchronize(this.taskKey);
      this.refreshTask();
    },
    refreshTask() {
      initBackgroundTask(
        this.taskKey,
        this.axios,
        {
          onProgress: (data) => {
            this.progress = data.progress;
          },
          onSuccess: () => {
            console.log("已完成");
            this.progress = null;
            this.refresh();
            // this.$message.success("同步成功");
          },
          onFailure: (data) => {
            console.log("失败");
          },
        },
      );
    },
    /* 下载二维码 */
    async downloadQRcode(record) {
      if(record){
        let data=JSON.stringify({  key:'material',value:record.id });
        let response = await apiQRcode.getImage(data);
        if (requestIsSuccess(response)) {
          let content = response.data;
          let prefixBase64= "data:image/png;base64,"; // base64前缀
          // 这里是获取到的图片base64编码,这里只是个例子，要自行编码图片替换这里才能测试看到效果
          const imgUrl = prefixBase64 + content;
          console.log(imgUrl);
          let file=base64toFile(imgUrl,record.name);
          SaveSingleFile(`${file.name}`, file.size, file).then(a => {
            console.log('下载成功');
          });
          this.$message.success('操作成功');
        }
      }else{
        this.$message.warning("请刷新页面重新尝试");
      }
    },
  },
  render() {
    return (
      <div class="sm-material-material">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            this.queryParams.professionId = null;
            this.queryParams.typeId = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              axios={this.axios}
              placeholder={'请输入材料名称'}
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="类别">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'MaterialType'}
              value={this.queryParams.typeId}
              onChange={(value) => {
                this.queryParams.typeId = value;
                this.refresh();
              }}
            // placeholder={this.isShow ? '' : '请选择材料类别'}
            // disabled={this.isShow}
            />
          </a-form-item>

          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" size="default" onClick={() => this.add()}> 新增</a-button>,
                vP(this.permissions, permissionsSmMaterial.Materials.Create),
              )}
              {vIf(
                <a-button type="primary" size="default" onClick={() => this.remove(true, this.materialIds)}> 删除</a-button>,
                vP(this.permissions, permissionsSmMaterial.Materials.Delete),
              )}
              {vIf(
                <a-button loading={!!this.progress} type="success" size="default" onClick={() => this.synchronize()}>同步工程数据{this.progress ? `(${parseInt(this.progress * 100)}%)` : ""}</a-button>,
                vP(this.permissions, permissionsSmMaterial.Materials.Delete),
              )}
              {vIf(
                (<a-dropdown>
                  <a-menu slot="overlay" onClick={this.handleMenuClick}>
                    <a-menu-item key="1" disabled={this.materialIds.length === 0} loading={this.loading}> <a-icon type="export" />导出所选</a-menu-item>
                    <a-menu-item key="2"> <a-icon type="export" />导出全部</a-menu-item>
                  </a-menu>
                  <a-button size="default" style="margin-left: 8px"> 导出 <a-icon type="down" /> </a-button>
                </a-dropdown>),
                // <a-button type="primary" size="small" onClick={() => this.export()} disabled={this.materialIds.length === 0} loading={this.loading}> <a-icon type="export" /> 导出</a-button>,
                vP(this.permissions, permissionsSmMaterial.Materials.Export),
              )}
            </div>
          </template>
        </sc-table-operator>
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.materials}
          pagination={false}
          loading={this.loading}
          rowSelection={
            {
              columnWidth: 30,
              selectedRowKeys:this.materialIds,
              onChange: selectRowKeys => {
                this.materialIds = selectRowKeys;
              },
            }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record) => {
                return <a onClick={() => { this.view(record); }}>{record.name}</a>;
              },
              profession: (text, record) => {
                return <a-tooltip placement='topLeft' title={record.profession.name}><span>{record.profession.name}</span></a-tooltip>;
              },
              type: (text, record) => {
                return <a-tooltip placement='topLeft' title={record.type.name}><span>{record.type.name}</span></a-tooltip>;
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a onClick={() => { this.edit(record); }}>编辑</a>,
                      vP(this.permissions, permissionsSmMaterial.Materials.Update),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmMaterial.Materials.Update) &&
                      vP(this.permissions, [
                        permissionsSmMaterial.Materials.Delete , permissionsSmMaterial.Materials.ExportCode]),
                    )}
                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}> 更多 <a-icon type="down" /> </a>
                        <a-menu slot="overlay">
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => { this.remove(false, record.id); }}>删除</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmMaterial.Materials.Delete),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.downloadQRcode(record);
                                }}
                              >
                                下载二维码
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmMaterial.Materials.ExportCode),
                          ) }
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmMaterial.Materials.Delete , permissionsSmMaterial.Materials.ExportCode]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        >
        </a-table>

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

        <MaterialModal
          ref="materialModal"
          axios={this.axios}
          onChange={value => this.visible = value}
          onSuccess={() => { this.visible = false; this.refresh(); }}
        />
      </div>
    );
  },
};
