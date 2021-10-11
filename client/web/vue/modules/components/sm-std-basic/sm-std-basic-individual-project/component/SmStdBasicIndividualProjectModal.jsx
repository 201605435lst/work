import { ModalStatus } from '../../../_utils/enum';
import * as utils from '../../../_utils/utils';
import { form as formConfig } from '../../../_utils/config';
import ApiIndividualProject from '../../../sm-api/sm-std-basic/IndividualProject';
import SmDataDictionaryTreeSelect from '../../../sm-system/sm-system-data-dictionary-tree-select';
import SmStdBasicIndividualProjectTreeSelect from '../../sm-std-basic-individual-project-tree-select';

let apiIndividualProject = new ApiIndividualProject();
const formFields = ['parentId', 'code', 'name', 'specialtyId', 'remark'];
export default {
  name: 'SmStdBasicIndividualProjectModal',

  props: {
    axios: { type: Function, default: null },
  },

  data() {
    return {
      form: {},
      status: ModalStatus.Hide, // 模态框状态
      confirmLoading: false,//确定按钮加载状态
      record: null,
      parentId: null,
      specialtyId:null,
    };
  },

  computed: {
    title() {
      return utils.getModalTitle(this.status);
    },

    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiIndividualProject = new ApiIndividualProject(this.axios);
    },

    add(record) {
      this.record = record;
      this.status = ModalStatus.Add;
      if (record != null || record != undefined) {
        this.parentId = record.id;
        this.specialtyId=record.specialtyId;
      }
      else
      {
        this.parentId =null;
        this.specialtyId=null;
      }
      this.$nextTick(() => {  setTimeout(() => {
        let _values ={ parentId: this.parentId,areaId:this.areaId,specialtyId:this.specialtyId,standardCodeId:this.standardCodeId};
        this.form.setFieldsValue(_values);
       
      });  });
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.parentId=record.parentId;
      if (record != null) {
        this.$nextTick(() => {setTimeout(() => {
          this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields)});
        
        });});
      }
      if (record == null) {
        this.status = ModalStatus.Hide;
      }
    },

    close() {
      this.status = ModalStatus.Hide;
      this.parentId = null;
      this.form.resetFields();
      this.confirmLoading = false;
    },

    ok() {
      if (this.status == ModalStatus.View) {
        this.close();
      }
      else{
        this.form.validateFields(async (err, values) => {
          let _values = values;
          if (!err) 
          {
            let data = { ...values };
            let response = null;
            if (this.status == ModalStatus.Add) 
              response = await apiIndividualProject.create(data);
            else if (this.status == ModalStatus.Edit)
            {
              data = { ...data, id: this.record.id };
              response = await apiIndividualProject.update(data);
            }
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              if (this.status === ModalStatus.Add) {
                this.$emit('success', 'Add');
              }
              if (this.status === ModalStatus.Edit) {
                this.$emit('success', data);
                this.$emit('getParent');
              }
              this.close();
            }
          }
        });
      }
    },
  },

  render() {
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}单项工程`}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
      >
        <a-form form={this.form}>
          {(this.status == ModalStatus.Add && this.parentId != null) ||
          this.status == ModalStatus.Edit ? (
              <a-form-item
                label="父级"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <SmStdBasicIndividualProjectTreeSelect
                  axios={this.axios}
                  disabled
                  v-decorator={[
                    'parentId',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            ) : (
              undefined
            )}

          <a-form-item
            label="名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入名称'}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入名称',
                    },
                    {
                      max: 200,
                      message: '名称最多输入200字符',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="编码"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入编码'}
              v-decorator={[
                'code',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入编码',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="专业"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmDataDictionaryTreeSelect
              axios={this.axios}
              showSearch={true}
              parentDisabled={true}
              ref="SmDataDictionaryTreeSelect"
              placeholder={this.parentId != null ? '' : '请选择专业'}
              disabled={this.parentId != null}
              groupCode={'Profession'}
              v-decorator={[
                'specialtyId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择专业！' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder={'请输入备注'}
              v-decorator={[
                'remark',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
