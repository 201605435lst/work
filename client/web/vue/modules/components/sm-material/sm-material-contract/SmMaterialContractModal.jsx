/**
 * 说明：物资合同管理
 * 作者：easten
 */
import { form as formConfig, tips, form } from '../../_utils/config';
import { requestIsSuccess, objFilterProps, getFileUrl } from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiContract from '../../sm-api/sm-material/Contract';
import FileUpload from '../../sm-file/sm-file-upload';
let apiContract =new ApiContract();
const formFields = [
  'id',
  'name',
  'date',
  'amount',
  'count',
  'remark',
];
export default {
  name: 'SmMaterialContractModal',
  components: { },
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 600 },
    height: { type: Number, default: 500 },
  },
  data() {
    return {
      form: {},
      record: null,
      title: '物资入库登记',
      status: ModalStatus.Hide,
      confirmLoading: false, //确定按钮加载状态
      selectedFileIds:[], // 选中的文件
      files:null,
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },
  watch: {},
  created() {
    this.form = this.$form.createForm(this, {});
    apiContract=new ApiContract(this.axios);
  },
  methods: {
    ok() {
      this.form.validateFields(async (err, values)=>{
        if (!err){
          let response;
          this.confirmLoading = true;
          if (this.status === ModalStatus.Add){
            let data = {
              ...values,
              fileIds: this.selectedFileIds,
            };
            if (this.selectedFileIds.length > 0) {
              _ = await this.$refs.fileUpload.commit(); // 提交文件
            }
            response = await apiContract.create(data);
            if (requestIsSuccess(response)){
              this.$message.info('添加成功');
              this.close();
              this.$emit('success');
              this.confirmLoading = false;
            }

          } else if (this.status === ModalStatus.Edit){
            let data = {
              ...values,
              fileIds: this.selectedFileIds,
              id:this.record.id,
            };
            response = await apiContract.update(data);
            if (requestIsSuccess(response)) {
              this.$message.info('更新成功');
              this.close();
              this.$emit('success');
              this.confirmLoading = false;
            }
          }
        }
      });
    },
    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
      this.confirmLoading = false;
    },
    add(){
      this.selectedFileIds = [];
      this.status = ModalStatus.Add;
      this.title = '新增物资合同';
      this.files=[];
    },
    view(record){
      this.status = ModalStatus.View;
      this.record = record;
      this.title = '物资合同详情';
      this.setFields(record);
    },
    edit(record){
      this.status = ModalStatus.Edit;
      this.record = record;
      this.title = '编辑物资合同';
      this.setFields(record);
    },
    async setFields(record){
      this.$nextTick(() => {
        this.form.setFieldsValue({...objFilterProps(record,formFields)});
      });
      let response=await apiContract.getFileByIds(record.id);
      if (requestIsSuccess(response)){
        this.files = response.data;
      }
    },
    // 文件选择
    fileSelected(files) {
      this.selectedFileIds = files.map(a => {
        return a.id;
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
        confirmLoading={this.confirmLoading}
      >
        <a-form form={this.form} class="record-form">
          <a-form-model-item
            label="合同名称："
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input placeholder='请输入合同名称' v-decorator={['name',{
              rules:[{required: true,message:'合同名称不能为空'}],
            }]} />
          </a-form-model-item>
          <a-form-model-item
            label='合同日期'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              style="width:100%"
              v-decorator={[
                'date',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '日期不能为空',
                    },
                  ],
                },
              ]}
            />
          </a-form-model-item>
          <a-form-model-item
            label="合同金额(万元)"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number placeholder='请输入合同金额' v-decorator={['amount',{
              rules:[{required: true,message:'金额不能为空'}],
            }]} 
            min={1}
            max={100000}
            style="width:100%"
            />
          </a-form-model-item>
          <a-form-model-item
            label='备注'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              v-decorator={['remark']}
              placeholder="请输入备注信息"
              auto-size={{ minRows: 2, maxRows: 6 }}
            />
          </a-form-model-item>
          <a-form-model-item
            label='备注'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <FileUpload
              style="width:96%"
              axios={this.axios}
              ref="fileUpload"
              fileList={this.files}
              multiple={true}
              onSelected={this.fileSelected}
              mode={this.status == ModalStatus.View ? 'view' : 'edit'}
              download
            />
          </a-form-model-item>
        </a-form>
      </a-modal>
    );
  },
};
