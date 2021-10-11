import { ModalStatus } from '../../_utils/enum';
import SmStdBasicModelFileModal from './SmStdBasicModelFileModal';
import SmStdBasicViewConnectorModal from './SmStdBasicViewConnectorModal';
import SmFileManageModal from '../../sm-file/sm-file-manage-modal/SmFileManageModal';
import ApiModelFile from '../../sm-api/sm-std-basic/ModelFile';
import ThumbView from './src/ThumbView';
import { getModelDetailLevel, requestIsSuccess, getFileUrl } from '../../_utils/utils';
import { tips } from '../../_utils/config';

let apiModelFile = new ApiModelFile();
export default {
  name: 'SmStdBasicConnectionModal',

  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },

  data() {
    return {
      form: {},
      status: ModalStatus.Hide,
      modelId: null,

      dataSource: null,
      totalCount: 0,
      viewState: 'disable',

      selectedModelFiles: [], //选中的需要下载的模型文件
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
          width: '10%',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '模型精细等级',
          dataIndex: 'detailLevel',
          width: '25%',
          scopedSlots: { customRender: 'detailLevel' },
        },
        {
          title: '缩略图',
          dataIndex: 'thumb',
          width: '35%',
          scopedSlots: { customRender: 'thumb' },
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
      apiModelFile = new ApiModelFile(this.axios);
    },

    add(key) {
      this.status = ModalStatus.Add;
      this.modelId = key;
      this.refresh();
    },

    close() {
      this.status = ModalStatus.Hide;
    },

    ok() {
      this.status = ModalStatus.Hide;
    },

    addModel(key) {
      this.$refs.SmStdBasicModelFileModal.add(key);
    },

    editModel(record) {
      this.$refs.SmStdBasicModelFileModal.edit(record);
    },

    async refresh(resetPage = true) {
      let response = await apiModelFile.getList(this.modelId);
      if (requestIsSuccess(response)) this.dataSource = response.data.items;
    },

    // 缩略图浏览
    viewFile(file) {
      this.$refs.ThumbView.view(file);
    },

    delete(key) {
      let _this = this;
      this.$confirm({
        title: tips.remove.title,
        content: h => <div style="color:red;">{tips.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiModelFile.delete(key);
            if (requestIsSuccess(response)) {
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },

    view(key) {
      this.$refs.SmStdBasicViewConnectorModal.init(key);
    },

    downLoad(files) {
      files.forEach(file => this.$refs.SmFileManageModal.fileDownload(file));
    },
  },

  render() {
    return (
      <a-modal
        visible={this.visible}
        title="查看设备文件"
        bordered={this.bordered}
        onOk={this.ok}
        onCancel={this.close}
        width={800}
      >
        <span>
          <a-button
            type="primary"
            onClick={() => {
              this.addModel(this.modelId);
            }}
          >
            增加
          </a-button>
          <a-button
            style="margin-left:10px"
            onClick={() => {
              this.downLoad(this.selectedModelFiles);
            }}
          >
            下载
          </a-button>
        </span>
        <a-table
          style="padding-top:10px;"
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          rowSelection={{
            columnWidth: 30,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectedModelFiles = selectedRows.map(x => x.familyFile);
            },
          }}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1;
              },
              detailLevel: (text, record, index) => {
                return getModelDetailLevel(record.detailLevel);
              },

              thumb: (text, record, index) => {
                return record.thumb ? (
                  <div
                    style={{
                      width: `50px`,
                      height: `50px`,
                      backgroundImage: `url(${getFileUrl(record.thumb.url)})`,
                    }}
                    onClick={() => {
                      this.viewFile(record.thumb);
                    }}
                  />
                ) : (
                  ''
                );
              },

              operations: (text, record, index) => {
                return (
                  <span>
                    <a onClick={() => this.view(record.id)}>查看连接件</a>
                    <a-divider type="vertical" />
                    <a-dropdown trigger={['click']}>
                      <a onClick={e => e.preventDefault()}>
                        更多 <a-icon type="down" />
                      </a>
                      <a-menu slot="overlay">
                        <a-menu-item>
                          <a onClick={() => this.editModel(record)}>修改</a>
                        </a-menu-item>
                        <a-menu-item>
                          <a onClick={() => this.delete(record.id)}>删除</a>
                        </a-menu-item>
                      </a-menu>
                    </a-dropdown>
                  </span>
                );
              },
            },
          }}
        ></a-table>
        <SmStdBasicModelFileModal
          ref="SmStdBasicModelFileModal"
          axios={this.axios}
          onSuccess={() => this.refresh(false)}
        />
        <SmStdBasicViewConnectorModal
          ref="SmStdBasicViewConnectorModal"
          axios={this.axios}
          onSucess={() => this.refresh(false)}
        />
        <ThumbView
          ref="ThumbView"
          value={this.viewState}
          onInput={value => (this.viewState = value)}
        />
        <SmFileManageModal ref="SmFileManageModal" axios={this.axios} />
      </a-modal>
    );
  },
};
