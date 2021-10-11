/**
 * 说明：文件上传选择
 * 作者：easten
 */
import { form as formConfig, tips, form } from '../../../_utils/config';
import { ModalStatus } from '../../../_utils/enum';
import FileUpload from '../../../sm-file/sm-file-upload';
import {FileSizeTrans} from '../../../sm-file/sm-file-manage/src/common';
import { requestIsSuccess, objFilterProps, getFileUrl } from '../../../_utils/utils';
import ApiDisclose from '../../../sm-api/sm-technology/Disclose';
const formFields = ['name', 'size','filesize', 'createtime', 'url','files'];
let apiDisclose = new ApiDisclose();
export default {
  name: 'FileSelect',
  components: {},
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 450 },
    height: { type: Number, default: 300 },
    type: { type: String, default: '' },  // security  安全技术交底内容
  },
  data() {
    return {
      form: {},
      record: {},
      files: [],
      title: '',
      main: false,
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
    apiDisclose=new ApiDisclose(this.axios);
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    add() {
      this.title = '上传交底视频';
      this.status = ModalStatus.Add;
      this.files = [];
    },
    edit(record) {
      this.record = record;
      this.status = ModalStatus.Edit;
      this.title = '附件编辑';
      this.files = record.file === null ? [] : [record.file];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
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
              ...this.record,
              type:this.type==='security'?1:0,
            };
            data.name=values.name;
            if (this.selectedFileId != '') {
              _ = await this.$refs.fileUpload.commit(); // 提交文件
            }
            let response = await apiDisclose.create(data);
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
      let name=files.name;
      let size=files.size;
      this.record={
        name,
        size,
        filesize:FileSizeTrans(size),
        url:files.relativeUrl,
        files:name,
      };
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(this.record, formFields) });
      });
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
            label="交底视频"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <FileUpload
              ref="fileUpload"
              fileList={this.files}
              axios={this.axios}
              height={30}
              title="点击上传交底视频(仅限mp4格式)"
              accept="video/mp4"
              single={true}
              onSelected={this.fileSelected}
              v-decorator={[
                'files',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择交底视频',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="视频名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '自动填充'}
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入视频名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="视频大小"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              placeholder={this.status == ModalStatus.View ? '' : '自动计算'}
              v-decorator={[
                'filesize',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>         
        </a-form>
      </a-modal>
    );
  },
};
