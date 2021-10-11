import ApiPartition from '../../../sm-api/sm-material/Partition';
import { form as formConfig, tips, form } from '../../../_utils/config';
import { ModalStatus } from '../../../_utils/enum';
import FileUpload from '../../../sm-file/sm-file-upload';
import { requestIsSuccess, objFilterProps, getFileUrl } from '../../../_utils/utils';
import PositionModal from './PositionModal';
import PicturePositionModal from './PicturePositionModal';
let apiPartition = new ApiPartition();

const formFields = ['name', 'description', 'remark'];
export default {
  name: 'PartitionModal',
  components: {},
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 450 },
    height: { type: Number, default: 300 },
    src: { type: String, default: '' },
  },
  data() {
    return {
      form: {},
      record: null,
      files: [],
      title: '',
      main: false,
      type: 0, // 对话框类型
      status: ModalStatus.Hide,
      confirmLoading: false, //确定按钮加载状态
      selectedFileId: '',
      point: [],
      iSrc: '',
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },
  watch: {
    src: {
      handler(nVal, oVal) {
        this.iSrc = nVal;
      },
      immediate: true,
    },
  },
  created() {
    apiPartition = new ApiPartition(this.axios);
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    add(record) {
      this.type = this.main ? 0 : record.type;
      this.main ? `新增顶级分区` : `新增分区`;
      this.status = ModalStatus.Add;
      this.main = record.id == null;
      this.record = record;
      this.files = [];
      this.point = [];
      if (record.type === 0 && record.file != null) {
        this.iSrc = getFileUrl(record.file.url);
      }
    },
    async edit(record) {
      if (record && record.id) {
        let response = await apiPartition.get(record.id);
        if (requestIsSuccess(response)) {
          this.record = response.data;
          this.main = this.record.parentId == null;
          this.status = ModalStatus.Edit;
          this.type = this.main ? 0 : this.record.type;
          this.title = this.type === 0 ? `编辑顶级分区` : `编辑分区`;
          this.files = this.record.file === null ? [] : [this.record.file];
          this.point = [this.record.x, this.record.y];
          this.$nextTick(() => {
            this.form.setFieldsValue({ ...objFilterProps(this.record, formFields) });
          });
          if (this.record.type === 0 && this.record.file != null) {
            this.iSrc = getFileUrl(this.record.file.url);;
          }
        }
      }

    },
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          this.confirmLoading = true;
          let response;
          if (this.status === ModalStatus.Add) {
            let data = {
              ...values,
              parentId: this.record === null ? null : this.record.id,
            };
            data.x = this.point[0];
            data.y = this.point[1];
            // 父级与子级的文件地址相同
            if (this.main) {
              data.fileId = this.selectedFileId === '' ? null : this.selectedFileId;

              if (this.selectedFileId != '') {
                _ = await this.$refs.fileUpload.commit(); // 提交文件
              }
            } else {
              data.topId = this.record.type === 0 ? this.record.id : this.record.topId;
            }
            let response = await apiPartition.create(data);
            if (requestIsSuccess(response)) {
              this.$message.info('添加成功');
              this.close();
              this.$emit('success');
              this.confirmLoading = false;
            }
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            let data = {
              ...values,
              id: this.record.id,
              parentId: this.record.parentId,
              x: this.point[0],
              y: this.point[1],
            };
            if (this.main) {
              data.fileId = this.selectedFileId === '' ? this.record.fileId : this.selectedFileId;
              _ = await this.$refs.fileUpload.commit(); //提交文件
            }
            let response = await apiPartition.update(data);
            if (requestIsSuccess(response)) {
              this.$message.success('编辑成功');
              this.close();
              this.$emit('success');
            }
          }
        }
      });
      //this.close();
    },
    fileSelected(files) {
      this.selectedFileId = files.id;
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
        bodyStyle={{ height: '300px' }}
        confirmLoading={this.confirmLoading}
      >
        <a-form form={this.form} class="partition-form">
          <a-form-item
            label="分区名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入分区名称'}
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入分区名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="分区描述"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入描述信息'}
              v-decorator={[
                'description',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              v-decorator={['remark']}
              placeholder="请输入备注信息"
              auto-size={{ minRows: 2, maxRows: 6 }}
            />
          </a-form-item>
          {this.main ? (
            <a-form-item
              label="料库位置"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-button
                size="small"
                type="primary"
                onClick={() => {
                  this.$refs.PositionModal.view();
                }}
              >
                点击拾取
              </a-button>
              {this.point.length > 0 ? <span>{`(${this.point[0]},${this.point[1]})`}</span> : null}
            </a-form-item>
          ) : (
            <a-form-item
              label="料库分区"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-button
                size="small"
                type="primary"
                onClick={() => {
                  this.$refs.PicturePositionModal.view();
                }}
              >
                点击拾取
              </a-button>
              {this.point.length > 0 ? <span>{`(${this.point[0]},${this.point[1]})`}</span> : null}
            </a-form-item>
          )}
          {this.main ? (
            <a-form-item
              label="分区图纸"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <FileUpload
                ref="fileUpload"
                fileList={this.files}
                axios={this.axios}
                height={30}
                title="点击上传分区图纸"
                accept="image/*"
                single={true}
                onSelected={this.fileSelected}
              />
            </a-form-item>
          ) : null}
        </a-form>
        <PositionModal
          ref="PositionModal"
          onSuccess={point => {
            this.point = point;
          }}
        />
        <PicturePositionModal
          src={this.iSrc}
          ref="PicturePositionModal"
          onSuccess={point => {
            this.point = point;
          }}
        />
      </a-modal>
    );
  },
};
