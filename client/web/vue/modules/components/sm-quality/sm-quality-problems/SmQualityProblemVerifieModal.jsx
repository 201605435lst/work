import * as utils from '../../_utils/utils';
import { ModalStatus, SafeRecordState, SafeRecordType } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiProblem from '../../sm-api/sm-quality/QualityProblem';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import SmCommonSelect from '../../sm-common/sm-common-select';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
let apiProblem = new ApiProblem();
export default {
  name: 'SmQualityProblemVerifieModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      files: [],
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
    },
    verifie(record) {
      this.status = ModalStatus.Add;
      this.record = record;
      let _qualityProblemId = [];
      _qualityProblemId.push({
        id: record ? record.qualityProblemId : undefined,
        type: 3,
      });
      record.qualityProblemId = _qualityProblemId;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue({ qualityProblemId: record ? record.id : undefined });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.files = [];
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err && utils.compareDate(this.record.changeTime,values.time) == '>') {
          this.$message.warning('“验证时间”不应该早于“整改时间”');
        } else {
          await this.$refs.fileUpload.commit();
          let _files = [];
          this.files.map(item => {
            _files.push({
              fileId: item.id,
            });
          });
          let data = {
            ...values,
            userId: Object.assign(...values.userId) ? Object.assign(...values.userId).id : '',
            files: _files,
            type: SafeRecordType.Verify,
          };
          console.log(data);
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiProblem.createRecord(data);
          } else {
            this.close();
          }
          if (response && utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
    },
  },
  render() {
    return (
      <a-modal
        title={`问题验证`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        width={800}
      >

        <a-form form={this.form}>
          <a-form-item
            label="问题标题"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmCommonSelect
              disabled={true}
              placeholder={this.status == ModalStatus.View ? '' : '请选择问题标题'}
              API={apiProblem}
              v-decorator={[
                'qualityProblemId',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '问题标题不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />

          </a-form-item>
          <a-form-item
            label="验证时间"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              placeholder={this.status == ModalStatus.View ? '' : '请选择验证时间'}
              disabled={this.status == ModalStatus.View}
              style="width:100%"
              v-decorator={[
                'time',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '验证时间不能为空',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="验证人"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmSystemMemberSelect
              height={32}
              shouIconSelect={true}
              showUserTab={true}
              userMultiple={false}
              bordered={true}
              simple={true}
              placeholder={this.status == ModalStatus.View ? '' : '请选择'}
              axios={this.axios}
              disabled={this.status == ModalStatus.View}
              onChange={value => {
                console.log(Object.assign(...value));
              }}
              v-decorator={[
                'userId',
                {
                  initialValue: undefined,
                  rules: [{ required: true, message: '验证人不能为空' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="验证内容"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入验证内容'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'content',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="资料上传"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileUpload
              ref="fileUpload"
              disabled={this.status == ModalStatus.View}
              mode={this.status == ModalStatus.View ? "view" : "edit"}
              axios={this.axios}
              multiple
              onChange={item => console.log(item)}
              placeholder={this.status == ModalStatus.View ? '' : '影像图文资料上传'}
              onSelected={(item) => {
                this.files = item;
              }}
              fileList={this.files}
            />

          </a-form-item>
          <a-form-item
            label="验证结果"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-radio-group
              disabled={this.status == ModalStatus.View}
              onChange={(value) => {
                console.log(value);
              }}
              v-decorator={[
                'state',
                {
                  initialValue: SafeRecordState.Passed,
                },
              ]}
            >
              <a-radio key={`${SafeRecordState.Passed}`} value={SafeRecordState.Passed}>通过</a-radio>
              <a-radio key={`${SafeRecordState.NotPass}`} value={SafeRecordState.NotPass}>不通过</a-radio>
            </a-radio-group>
          </a-form-item>
        </a-form>

      </a-modal>
    );
  },
};
