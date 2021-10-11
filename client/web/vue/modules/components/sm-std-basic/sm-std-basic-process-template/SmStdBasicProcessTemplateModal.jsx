import { ModalStatus, ServiceLifeUnit, ProcessType } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { form as formConfig } from '../../_utils/config';
import ApiProcessTemplate from '../../sm-api/sm-std-basic/ProcessTemplate';
import SmStdBasicProcessTemplateTreeSelect from '../sm-std-basic-process-template-tree-select';

let apiProcessTemplate = new ApiProcessTemplate();
const formFields = [
  'parentId',
  'prepositionId',
  'code',
  'name',
  'unit',
  'content',
  'type',
  'remark',
  'duration',
  'durationUnit',
];
export default {
  name: 'SmStdBasicProcessTemplateModal',
  props: {
    axios: { type: Function, default: null },
  },

  data() {
    return {
      form: {},
      status: ModalStatus.Hide,
      record: null,
      parentId: null,
      durationUnit: ServiceLifeUnit.Year, //工期单位
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

  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiProcessTemplate = new ApiProcessTemplate(this.axios);
    },

    add(record) {
      this.record = record;
      this.status = ModalStatus.Add;
      if (record != null || record != undefined) {
        this.parentId = record.id;
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ parentId: this.parentId });
      });
    },

    edit(record) {
     
      this.record = record;
      this.remark=record.remark;
      this.status = ModalStatus.Edit;
      let values={ ...utils.objFilterProps(record, formFields) };
      this.$nextTick(() => {
        this.form.setFieldsValue(values);
      });
    },

    cancel() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.parentId = null;
    },
    
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = { ...values, 
            remark:this.remark,
            durationUnit: this.durationUnit,
          };
          let response = null;
          if (this.status == ModalStatus.Add) response = await apiProcessTemplate.create(data);
          else if (this.status == ModalStatus.Edit) {
            data = { ...data, id: this.record.id };
            response = await apiProcessTemplate.update(data);
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            if (this.status === ModalStatus.Add) {
              this.$emit('success', 'Add');
            }
            if (this.status === ModalStatus.Edit) {
              this.$emit('success', data);
            }
            this.cancel();
          }
        }
      });
    },
  },

  render() {
    //工期单位
    let durationUnitOption = [];
    for (let item in ServiceLifeUnit) {
      durationUnitOption.push(
        <a-select-option key={ServiceLifeUnit[item]} value={ServiceLifeUnit[item]}>
          {utils.getServiceLifeUnit(ServiceLifeUnit[item])}
        </a-select-option>,
      );
    }
    //工序类型
    let processTypeOption = [];
    for (let item in ProcessType) {
      processTypeOption.push(
        <a-radio key={ProcessType[item]} value={ProcessType[item]}>
          {utils.getProcessTypeOption(ProcessType[item])}
        </a-radio>,
      );
    }
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}工序模板`}
        onOk={this.ok}
        onCancel={this.cancel}
      >
        <a-form form={this.form}>
          {(this.status == ModalStatus.Add && this.parentId != null) ||
          this.status == ModalStatus.Edit ? (
              <a-form-item
                label="父级"
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                <SmStdBasicProcessTemplateTreeSelect
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
            label="工作项单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              axios={this.axios}
              v-decorator={[
                'unit',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入工作项单位',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="工期"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-group compact style="width:100%">
              <a-input-number
                style="width:79%"
                min={0}
                placeholder={this.status == ModalStatus.View ? '' : '请输入工期！'}
                disabled={this.status === ModalStatus.View}
                v-decorator={[
                  'duration',
                  {
                    initialValue: 0,
                    rules: [{ required: true, message: '请输入工期！' }],
                  },
                ]}
              />
              <a-form-item>
                <a-input type="hidden"
                  v-decorator={[
                    'durationUnit',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
              <a-select
                disabled={this.status === ModalStatus.View}
                value={this.durationUnit}
                onChange={value => {
                  this.durationUnit = value;
                }}
              >
                {durationUnitOption}
              </a-select>
            </a-input-group>
          </a-form-item>
          <a-form-item
            label="工作内容"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              // disabled={this.status === ModalStatus.View}
              placeholder={'请输入工作内容'}
              v-decorator={[
                'content',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="工序类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-radio-group
              placeholder={this.status == ModalStatus.View ? '' : '请选择工序类型！'}
              // disabled={this.status === ModalStatus.View}
              v-decorator={[
                'type',
                {
                  initialValue: '',
                  rules: [{ required: true, message: '请选择维修类别！' }],
                },
              ]}
            >
              {processTypeOption}
            </a-radio-group>
          </a-form-item>
          <a-form-item
            label="前置任务"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmStdBasicProcessTemplateTreeSelect
              axios={this.axios}
              // disabled={this.status === ModalStatus.View}
              parentDisabled={true}
              // ref="SmStdBasicProcessTemplateTreeSelect"
              placeholder={'请选择前置任务'}
            
              v-decorator={[
                'prepositionId',
                {
                  initialValue: null,
                  
                },
              ]}
            />
          </a-form-item>
          {/* <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder="请输入备注"
              v-decorator={[
                'remark',
                {
                  initialValue: '',
                },
              ]}
              // value={this.remark}
              // onChange={item => {
              //   console.log(item.target.value);
                
              //   // this.remarkChange(item);
              // }}

            />
          </a-form-item> */}
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              // disabled={this.status === ModalStatus.View}
              placeholder='请输入工作内容'
              // v-decorator={[
              //   'remark',
              //   {
              //     initialValue: '',
              //   },
              // ]}
              value={this.remark}
              onChange={event=>{
                this.remark=event.target.value;
              }}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
