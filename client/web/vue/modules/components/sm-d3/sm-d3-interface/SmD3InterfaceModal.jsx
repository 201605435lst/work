import * as utils from '../../_utils/utils';
import { ModalStatus, MarkType } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiConstructInterface from '../../sm-api/sm-technology/ConstructInterface';
import { getMarkType } from '../../_utils/utils';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmResourceEquipmentSelect from '../../sm-resource/sm-resource-equipment-select';
let apiConstructInterface = new ApiConstructInterface();
import moment from 'moment';
import { stringify } from 'qs';

// 定义表单字段常量
const formFields = ['name', 'code', 'professionId', 'materialSpec', 'marerialName', 'marerialCount', 'gisData', 'builderId'];
export default {
  name: 'SmD3InterfaceModal',
  props: {
    axios: { type: Function, default: null },

  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      interfaceManagementTypeId: null,
      position: null,
      mPosituion: null,
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
  watch: {
    interfaceManagementTypeId: {
      handler: function (val, oldval) {
        this.iInterfaceManagementTypeId = val;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiConstructInterface = new ApiConstructInterface(this.axios);
    },
    /* lon,lat,alt */
    async add(interfaceManagementTypeId, val) {

      if (val) {
        let _value=JSON.parse(val);
        this.position = `${_value.lon} ,${_value.lat} ,${_value.alt} `;
        this.mPosituion = val;
      }
      this.status = ModalStatus.Add;
      this.interfaceManagementTypeId = interfaceManagementTypeId;
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.interfaceManagementTypeId = null;
      this.status = ModalStatus.Hide;
      this.position = '';
      this.mPosituion = null;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
            interfaceManagementTypeId: this.interfaceManagementTypeId,
            position: this.mPosituion,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiConstructInterface.create(data);
          }
          if (utils.requestIsSuccess(response)) {
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
    let Options = [];
    for (let item in MarkType) {
      if (MarkType[item] != MarkType.NoCheck) {
        Options.push(
          <a-select-option key={`${MarkType[item]}`}>
            {getMarkType(MarkType[item])}
          </a-select-option>,
        );
      }
    }
    return (
      <a-modal
        class="sm-d3-interface-modal"
        title={`${this.title}接口`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? '保存' : '确定'}
        onOk={this.ok}
        width={800}
      >

        <a-form form={this.form}>
          <a-form-item
            label="接口名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入接口名称'}
              style="width:100%"
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '接口名称不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="编号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入编号'}
              style="width:100%"
              v-decorator={[
                'code',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '编号不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="位置"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={true}
              placeholder={''}
              style="width:100%"
              value={this.position}
            />
          </a-form-item>
          <a-form-item
            label="土建单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionaryTreeSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              groupCode={'ConstructionUnit'}
              placeholder="请选择"
              v-decorator={[
                'builderId',
                {
                  initialValue: undefined,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="专业"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionaryTreeSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              groupCode={'Profession'}
              placeholder="请选择"
              v-decorator={[
                'professionId',
                {
                  initialValue: undefined,
                },
              ]}
            />
          </a-form-item>
          {/* <a-form-item
                        label="设备名称"
                        label-col={formConfig.labelCol}
                        wrapper-col={formConfig.wrapperCol}
                    >
                        <SmResourceEquipmentSelect
                            axios={this.axios}
                            placeholder='请选择设备'
                            disabled={this.status == ModalStatus.View}
                            v-decorator={[
                                'equipmentId',
                                {
                                    initialValue: null,
                                },
                            ]}
                        />
                    </a-form-item> */}
          <a-form-item
            label="材料名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入材料名称'}
              style="width:100%"
              v-decorator={[
                'marerialName',
                {
                  initialValue: null,

                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="材料规格"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入材料规格'}
              style="width:100%"
              v-decorator={[
                'materialSpec',
                {
                  initialValue: null,

                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="材料数量"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入材料数量'}
              style="width:100%"
              min={0}
              v-decorator={[
                'marerialCount',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          {/* <a-form-item
                        label="检查状况"
                        label-col={formConfig.labelCol}
                        wrapper-col={formConfig.wrapperCol}
                    >
                        <a-select
                            placeholder="请选择"
                            style="width:100%"
                            disabled={this.status == ModalStatus.View}
                            allowClear
                            v-decorator={[
                                'markType',
                                {
                                    initialValue: undefined,
                                    rules: [
                                        {
                                            required: true,
                                            message: '请选择检查情况',
                                            whitespace: true,
                                        },
                                    ],
                                },
                            ]}
                        >
                            {Options}
                        </a-select>
                    </a-form-item> */}
          <a-form-item
            label="接口数据"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="2"
              placeholder={this.status == ModalStatus.View ? '' : '请输入接口数据'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'gisData',
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
