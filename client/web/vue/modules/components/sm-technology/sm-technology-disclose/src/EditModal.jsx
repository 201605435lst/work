/**
 * 说明：文件上传选择
 * 作者：easten
 */
import { ModalStatus } from '../../../_utils/enum';
import FileUpload from '../../../sm-file/sm-file-upload';
import { FileSizeTrans } from '../../../sm-file/sm-file-manage/src/common';
import { requestIsSuccess, getFileUrl, CreateGuid } from '../../../_utils/utils';
import SmVideo from '../../../sm-common/sm-video/SmVideo';
import SmFileImageView from '../../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
import ApiDisclose from '../../../sm-api/sm-technology/Disclose';
import OssRepository from '../../../sm-file/sm-file-manage/src/ossRepository';
import { SaveSingleFile } from '../../../sm-file/sm-file-manage/src/common';

let apiDisclose = new ApiDisclose();
let ossRepository = new OssRepository();

export default {
  name: 'EditModal',
  components: {},
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 450 },
    height: { type: Number, default: 300 },
    type: { type: String, default: '' }, // security  安全技术交底内容
  },
  data() {
    return {
      record: {},
      files: [],
      title: '附件编辑(视频仅限mp4格式)',
      status: ModalStatus.Hide,
      dataSource: [],
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    datas() {
      return this.dataSource.filter(a => !a.isDelete);
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'xh',
          ellipsis: true,
          align: 'center',
          width: 60,
          scopedSlots: { customRender: 'xh' },
        },
        {
          title: '附件名称',
          dataIndex: 'name',
          ellipsis: true,
          align: 'center',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          align: 'center',
          width: 60,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},
  created() {
    apiDisclose = new ApiDisclose(this.axios);
    ossRepository = new OssRepository();

  },
  methods: {
    async refresh() {
      let response = await apiDisclose.get({ id: this.record.id });
      if (requestIsSuccess(response) && response.data) {
        this.dataSource = response.data.map(item => {
          return {
            ...item,
            isView: true,
          };
        });
      }
    },
    async edit(record) {
      this.record = record;
      this.status = ModalStatus.Edit;
      this.refresh();
    },
    close() {
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },
    async ok() {
      if (this.status === ModalStatus.Edit) {
        let data = {
          items: [...this.dataSource],
          parentId: this.record.id,
        };
        if (this.selectedFileId != '') {
          _ = await this.$refs.fileUpload.commit(); // 提交文件
        }
        let response = await apiDisclose.updateAttachment(data);
        if (requestIsSuccess(response)) {
          this.$message.info('操作成功');
          this.close();
          this.$emit('success');
          this.confirmLoading = false;
        }
      }
    },
    fileSelected(files) {
      files.forEach(item => {
        this.selectedFileId = files.id;
        let size = item.size;
        let record = {
          id: CreateGuid(),
          name: item.name,
          size,
          isDelete: false,
          filesize: FileSizeTrans(size),
          url: item.relativeUrl,
          files: item.name,
          fileType: item.type,
          isView: false,
        };
        this.dataSource.push(record);
      });
    },
    // 删除
    delete(id) {
      this.dataSource.forEach(a => {
        if (a.id == id) {
          a.isDelete = true;
        }
      });
    },
    add() {
      this.$refs.fileUpload.fileSelect();
    },
    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      let videoTypes = ['ogv', '.webm', '.mp4'];
      let _file = JSON.parse(JSON.stringify({ ...file, type: file.fileType }));
      if (file.fileType === '.pdf') {
        this.$refs.SmFileDocumentView.view(_file);
      } else if (imgtypes.includes(_file.fileType)) {
        this.$refs.SmFileImageView.view(_file);
      } else if (videoTypes.includes(_file.fileType)) {
        this.$refs.SmVideo.preview(true, getFileUrl(_file.url), _file.name);
      } else {
        this.$message.warning('当前文件不支持预览，开始下载中...');
        this.downloadClick(_file);
      }
    },

    downloadClick(file) {
      let _this = this;
      if (file) {
        ossRepository
          .download(getFileUrl(file.url), () => { })
          .then(blob => {
            SaveSingleFile(`${file.name}${file.type}`, file.size, blob).then(a => {
              _this.$notification['success']({
                message: '温馨提示',
                description: `${file.name}下载成功`,
                duration: 2,
              });
            });
          });
      }
    },
  },
  render() {
    return (
      <a-modal
        title={this.title}
        onOk={this.ok}
        visible={this.visible}
        onCancel={this.close}
        width={this.width}
        height={this.height}
        confirmLoading={this.confirmLoading}
      >
        <div
          style={{
            display: 'flex',
            flexDirection: 'row',
            marginBottom: '10px',
            justifyContent: 'space-between',
            justifyItems: 'center',
          }}
        >
          <span>视频名称：{this.record.name}</span>
          <a-button type="primary" size="small" onClick={() => this.add()}>
            添加
          </a-button>
        </div>
        <div>
          <a-table
            columns={this.columns}
            dataSource={this.datas}
            rowKey={record => record.id}
            pagination={false}
            size="small"
            scroll={{
              y: 260,
            }}
            {...{
              scopedSlots: {
                xh: (text, record, index) => {
                  return index + 1;
                },
                name: (text, record, index) => {
                  return record.isView ? <a onClick={() => this.play(record)}>{text}</a> : text;
                  // onClick={() => this.play(record)}
                },
                size: (text, record, index) => {
                  return FileSizeTrans(record.size);
                },
                operations: (text, record) => {
                  return [
                    <span>
                      <a-button
                        style="padding:2px"
                        onClick={() => this.delete(record.id)}
                        type="link"
                      >
                        {' '}
                        删除
                      </a-button>
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
        </div>
        <FileUpload
          ref="fileUpload"
          axios={this.axios}
          single={false}
          custom
          multiple
          onSelected={this.fileSelected}
        />

        <SmVideo axios={this.axios} ref="SmVideo" />
        {/* 图片类预览组件 */}
        <SmFileImageView axios={this.axios} ref="SmFileImageView" />
        {/* 文档浏览组件 */}
        <SmFileDocumentView axios={this.axios} ref="SmFileDocumentView" />
      </a-modal>
    );
  },
};
