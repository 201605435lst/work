import { pagination as paginationConfig, tips } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import ApiModelFileRltConnector from '../../sm-api/sm-std-basic/RevitConnector';
import { requestIsSuccess, CreateGuid } from '../../_utils/utils';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';

let apiModelFileRltConnector = new ApiModelFileRltConnector();
export default {
  name: 'SmStdBasicViewConnectorModal',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },

  data() {
    return {
      dataSource: [],
      editingKey: '', //当前编辑的key
      modelFileId: null, //模型文件Id

      form: {},
      record: null,
      status: ModalStatus.Hide,

      pageIndex: 1,
      totalCount: 0,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWord: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },

      editable: false,
    };
  },

  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: '20%',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '连接件名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          width: '40%',
        },
        {
          title: '连接件位置',
          dataIndex: 'position',
          width: '25%',
          scopedSlots: { customRender: 'position' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiModelFileRltConnector = new ApiModelFileRltConnector(this.axios);
    },

    async init(key) {
      this.totalCount = 0;
      this.status = ModalStatus.Add;
      this.modelFileId = key;
      this.refresh();
    },

    async addLine() {
      this.editingKey = '';
      this.dataSource.push({
        id: CreateGuid(),
        name: null,
        position: null,
        editable: true,
      });
      this.totalCount += 1;
    },

    async remove(key) {
      let _this = this;
      this.$confirm({
        title: tips.remove.title,
        content: h => <div style="color:red;">{tips.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiModelFileRltConnector.delete(key);
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else setTimeout(reject, 100);
          });
        },
      });
    },

    close() {
      this.status = ModalStatus.Hide;
      this.dataSource = null;
    },

    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiModelFileRltConnector.getListByModelFileId({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        modelFileId: this.modelFileId,
        isAll: false,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.dataSource.forEach((value, index) => {
          value['editable'] = false;
        });

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

    onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'revitConnector',
        modelFileId: this.modelFileId,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },

    //文件导出
    async downloadFile(para) {
      let queryParams = {};
      queryParams = { ModelFileId: this.modelFileId };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },

    //编辑按钮
    async edit(record) {

      this.editingKey =record.id;
      if (record.name == null) this.$message.error('连接件名称不能为空');
      else{
        if (record.position == null) this.$message.error('连接件位置不能为空');
        else{
          const newData = [...this.dataSource];
          const target = newData.filter(item => record.id === item.id)[0];
          this.editingKey = record.id;
          if (target) {
            target.editable = true;
            this.dataSource = newData;
          }
        }
      }
    },

    async save(record) {
      if (record.name == null) this.$message.error('连接件名称不能为空');
      else{
        if (record.position == null) this.$message.error('连接件位置不能为空');
        else{
          const newData = [...this.dataSource];
          const target = newData.filter(item => record.id === item.id)[0];
          if (target) {
            let response = null;
            if (this.editingKey != '')
              response = await apiModelFileRltConnector.update({
                ...record,
                id: record.id,
                modelFileId: this.modelFileId,
              });
            else
              response = await apiModelFileRltConnector.create({
                ...record,
                ModelFileId: this.modelFileId,
              });
            if (requestIsSuccess(response)) {
              delete target.editable;
              this.dataSource = newData;
              this.refresh();
            }
          }
          this.editingKey = '';
        }
      }
    },
  },

  render() {
    return (
      <a-modal
        visible={this.visible}
        title={'查看连接件'}
        onCancel={this.close}
        onOk={this.close}
        width={800}
      >
        <div style={'display:flex'}>
          <a-button onClick={() => this.addLine()}>增加</a-button>
          <SmImport
            style="margin-left:10px"
            ref="smImport"
            url="api/app/stdBasicRevitConnector/upload"
            axios={this.axios}
            downloadErrorFile={true}
            importKey="revitConnector"
            onSelected={file => this.fileSelected(file)}
            onIsSuccess={() => {
              this.refresh();
            }}
          />
          <SmExport
            style="margin-left:10px"
            ref="smExport"
            url="api/app/stdBasicRevitConnector/export"
            axios={this.axios}
            templateName="revitConnector"
            downloadFileName="Revit连接件表"
            defaultTitle={'导出'}
            rowIndex={5}
            onDownload={para => this.downloadFile(para)}
          />
          ,
        </div>

        <a-table
          style="padding-top:10px;"
          columns={this.columns}
          dataSource={this.dataSource}
          pagination={false}
          rowKey={record => record.id}
          bordered={this.bordered}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let res = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return res;
              },
              name: (text, record, index) => {
                return record.editable ? (
                  <a-input
                    style="margin: -10px 0"
                    value={record.name}
                    onChange={event => (record.name = event.target.value)}
                  />
                ) : (
                  record.name
                );
              },
              position: (text, record, index) => {
                return record.editable ? (
                  <a-input
                    style="margin: -10px 0"
                    value={record.position}
                    onChange={event => (record.position = event.target.value)}
                  />
                ) : (
                  record.position
                );
              },
              operations: (text, record, index) => {
                let result;
                if (!record.editable)
                  result = (
                    <span disabled={this.editingKey !== ''}>
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >
                        编辑
                      </a>
                      <a-divider type="vertical" />
                      <a
                        onClick={() => {
                          this.remove(record.id);
                        }}
                      >
                        删除
                      </a>
                    </span>
                  );
                else
                  result = (
                    <span>
                      <a
                        onClick={() => {
                          this.save(record);
                        }}
                      >
                        保存
                      </a>
                      <a-divider type="vertical" />
                      <a
                        onClick={() => {
                          this.remove(record.id);
                        }}
                      >
                        删除
                      </a>
                    </span>
                  );
                return result;
              },
            },
          }}
        ></a-table>
        <a-pagination
          style="margin-top: 10px;display: flex;justify-content:flex-end"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
      </a-modal>
    );
  },
};
