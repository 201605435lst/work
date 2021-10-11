import './style';
import permissionsComponenttrack from '../../_permissions/sm-componenttrack';
import ApiEquipments from '../../sm-api/sm-resource/Equipments';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiComponentQrCode from '../../sm-api/sm-componenttrack/componentQrCode';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import CrmCategoryTreeSelect from '../../sm-std-basic/sm-std-basic-component-category-tree-select';
import { base64toFile,SaveMultipleFile,SaveSingleFile } from '../../sm-file/sm-file-manage/src/common';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
let apiComponentQrCode = new ApiComponentQrCode();
let apiEquipments = new ApiEquipments();
export default {
  name: 'SmComponentQrCodeGenerate',
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
      isCanDownload:false,
      recordList: [],
      selectedRowKeys: [],
      pageIndex: 1,
      queryParams: {
        organizationIds: [], //维护单位
        componentCategoryId: null, //构件分类
        keyWord:null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    canSelect(){
      return this.totalCount==0 || this.dataSource.some(item=>!item.isGenerateCode);
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '设备分类',
          ellipsis: true,
          dataIndex: 'componentCategory',
          scopedSlots: { customRender: 'componentCategory' },
        },
        {
          title: '设备编码',
          ellipsis: true,
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '设备型号',
          dataIndex: 'productCategory',
          ellipsis: true,
          scopedSlots: { customRender: 'productCategory' },
        },
        {
          title: '生产厂家',
          dataIndex: 'manufacturer',
          ellipsis: true,
          scopedSlots: { customRender: 'manufacturer' },
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
      apiEquipments = new ApiEquipments(this.axios);
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
      let response = await apiEquipments.getList({
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
        if(this.dataSource.length>0){
          this.isCanDownload=false;
        }else{
          this.isCanDownload=true;
        }
      }
     
      this.loading = false;
    },
    async view(record){
      let imgSrc='';
      let response = await apiComponentQrCode.getView(record?record.id:'');
      if (requestIsSuccess(response)) {
        imgSrc='data:image/png;base64,'+response.data;
      }
      return imgSrc;
    },
    getFileStream(records) {
      return new Promise(resolve => {
        if (records.length > 0) {
          let data = [];
          records.forEach(async item => {
            let response = await apiComponentQrCode.exportCode([item.id]);
            if (requestIsSuccess(response)) {
              let qrBase64 = btoa(String.fromCharCode(...new Uint8Array(response.data)));
              let prefixBase64= "data:image/png;base64,"; // base64前缀
              // 这里是获取到的图片base64编码,这里只是个例子，要自行编码图片替换这里才能测试看到效果
              const imgUrl = prefixBase64 + qrBase64;
              let file=base64toFile(imgUrl);
              data.push({
                name: `${item.name}.png`,
                blob: file,
              });
              console.log(data);
              if(data.length==records.length){
                resolve(data);
              }
            }
          });
        }
      });
    },
    async fileDownload(files) {
      if (files instanceof Array) {
        // 多文件打包下载
        this.getFileStream(files).then(datas => {
          // 拼装压缩包格式
          if (datas.length > 0) {
            SaveMultipleFile(`附件.zip`, datas).then(() => {
              console.log('下载成功');
            });
          }
        });
      } else {
        let response = await apiComponentQrCode.exportCode([files?files.id:'']);
        if (requestIsSuccess(response)) {
          let qrBase64 = btoa(String.fromCharCode(...new Uint8Array(response.data)));
          let prefixBase64= "data:image/png;base64,"; // base64前缀
          // 这里是获取到的图片base64编码,这里只是个例子，要自行编码图片替换这里才能测试看到效果
          const imgUrl = prefixBase64 + qrBase64;
          let file=base64toFile(imgUrl);
          SaveSingleFile(`${files.name}.png`, file.size, file).then(a => {
            console.log('下载成功');
          });
        }
      }
      console.log("3333333");
      this.refresh();
    },
    handleExport(record){
      if(!record.isGenerateCode){
        this.exportCode(record);
      }else{
        let _this = this;
        this.$confirm({
          title: "确认生成",
          content: h => <div style="color:red;">{"确定生成新的二维码?旧的二维码将会失效"}</div>,
          okType: 'danger',
          onOk() {
            _this.exportCode(record);
          },
          onCancel() { },
        });
      }
    },
    exportCode(record){
      try {
        if(record || this.selectedRowKeys.length>0){
          let str=record ||this.recordList;
          this.fileDownload(str);
        }else{
          throw new Error("请选择你要导出的设备二维码");
        }
      } catch (error) {
        this.$message.error(error.message);
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
            this.queryParams.organizationIds = [];
            this.queryParams.componentCategoryId = null;
            this.queryParams.keyWord = null;
            this.refresh();
          }}
        >
          <a-form-item label="设备分类">
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
          <a-form-item label="关键字">
            <a-input
              placeholder='请输入名称、编码'
              value={this.queryParams.keyWord}
              onPressEnter={this.refresh}
              allowClear={true}
              onInput={event => {
                this.queryParams.keyWord = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="维护单位">
            <OrganizationTreeSelect
              axios={this.axios}
              multiple={true}
              value={this.queryParams.organizationIds}
              onInput={value => {
                this.queryParams.organizationIds = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary"  onClick={() => this.exportCode()} disabled={!this.canSelect}>
                    批量生成二维码
                </a-button>,
                vP(this.permissions, permissionsComponenttrack.ComponentRltQRCodes.Export),
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
            getCheckboxProps: record => ({
              props: {
                disabled: record.isGenerateCode, 
                name: record.name,
              },
            }),
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
                  // <a-popover placement="top" trigger="click">
                  //   <span slot="content">
                  //     <p> <img src={imgSrc} alt=""/></p>
                      
                //   </span>
                //   <span style={{color:record.isGenerateCode?"#3cd11b":''}}>{name}</span>
                // </a-popover>
                );
              },
              componentCategory: (text, record) => {
                let result = record.componentCategory
                  ? record.componentCategory.parent
                    ? record.componentCategory.parent.name + '-' + record.componentCategory.name
                    : ''
                  : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              code: (text, record) => {
                let result = record && record.code ? record.code : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              productCategory: (text, record) => {
                let result = record.productCategory ? record.productCategory.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              manufacturer: (text, record) => {
                let result = record.manufacturer ? record.manufacturer.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.handleExport(record);
                        }}
                      >生成二维码
                      </a>,
                      vP(this.permissions, permissionsComponenttrack.ComponentRltQRCodes.Export),
                    )}
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
      </div>
    );
  },
};
