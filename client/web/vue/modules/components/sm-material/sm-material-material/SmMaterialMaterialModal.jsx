
import { requestIsSuccess, objFilterProps, vP, vIf } from '../../_utils/utils';
import ApiMaterial from '../../sm-api/sm-technology/Material';
import { ModalStatus } from '../../_utils/enum';
import MaterialTypeSelect from '../sm-material-material-type-select';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmResourceEquipmentSelect from '../../sm-resource/sm-resource-equipment-select';
let apiMaterial = new ApiMaterial();

// 定义表单字段常量
const formFields = [
  'name',
  'typeId',
  'spec',
  'unit',
  'price',
  'remark',
];

export default {
  name: 'SmMaterialMaterialModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      status: ModalStatus.Hide, // 模态框状态
    };
  },
  computed: {
    isShow() {
      return this.status == ModalStatus.View;
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
      apiMaterial = new ApiMaterial(this.axios);
    },
    save(){
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = JSON.parse(JSON.stringify(values));
          let data = {
            ..._values,
          };
          console.log(data);
          this.loading = true;

          if (this.status === ModalStatus.Add) {
            let response = await apiMaterial.create(data);
            if (requestIsSuccess(response)) {
              this.$message.success('添加成功');
              this.$emit('success');
              this.close();
            }
          } else if (this.status === ModalStatus.Edit) {
            let _data = { id: this.record.id, ...data };
            let response = await apiMaterial.update(_data);
            if (requestIsSuccess(response)) {
              this.$message.success('编辑成功');
              this.$emit('success');
              this.close();
            }
          }
          this.loading = false;
        }
      });
    },

    add(){
      this.status = ModalStatus.Add;
    },
    edit(record) {
      this.record = record;
      this.status = ModalStatus.Edit;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
    },

    // 详情
    view(record) {
      this.record = record;
      this.status = ModalStatus.View;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    return (
      <a-modal
        width={this.width}
        title={this.status == ModalStatus.Add ? "添加" : this.isShow ? "查看" : "编辑"}
        okText="保存"
        visible={this.visible}
        onOk={this.isShow ? this.close : this.save}
        onCancel={this.close}
      >
        <div class="material-modal">
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item label="材料名称" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料名称'}
                    disabled={this.isShow}
                    v-decorator={[
                      'name',
                      {
                        initialValue: undefined,
                        rules: [
                          {
                            required: true,
                            message: '请输入材料名称',
                            whitespace: true,
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="材料类别" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <DataDictionaryTreeSelect
                    axios={this.axios}
                    groupCode={'MaterialType'}
                    placeholder={this.isShow ? '' : '请选择材料类别'}
                    disabled={this.isShow}
                    ignore="MaterialType.Component"
                    v-decorator={[
                      'typeId',
                      {
                        initialValue: undefined,
                        rules: [
                          {
                            required: true,
                            message: '请选择材料类别',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item label="规格型号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料规格型号'}
                    disabled={this.isShow}
                    v-decorator={[
                      'spec',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="计量单位" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入计量单位'}
                    disabled={this.isShow}
                    v-decorator={[
                      'unit',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="材料价格" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input-number
                    style="width:100%"
                    min={0}
                    placeholder={this.isShow ? '' : '请输入材料价格'}
                    disabled={this.isShow}
                    formatter={value => `￥ ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                    v-decorator={[
                      'price',
                      {
                        initialValue: undefined,
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="备注" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-textarea
                    placeholder={this.isShow ? '' : '请输入材料备注'}
                    disabled={this.isShow}
                    v-decorator={[
                      'remark',
                      {
                        initialValue: '',
                        rules: [
                          {
                            message: '请输入备注',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </div>
      </a-modal>
    );
  },
};
