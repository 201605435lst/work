
import './style';
import { objFilterProps } from '../../_utils/utils';
import ApiMaterial from '../../sm-api/sm-material/Material';
import { ModalStatus } from '../../_utils/enum';
import MaterialTypeSelect from '../sm-material-material-type-select';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
let apiMaterial = new ApiMaterial();

// 定义表单字段常量
const formFields = [
  'name',
  'typeId',
  'professionId',
  'spec',
  'model',
  'unit',
  'price',
  'remark',
];

export default {
  name: 'SmMaterialInquireModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
  },
  data() {
    return {
      form: {}, // 表单
      status: ModalStatus.Hide, // 模态框状态
      listMessage:[], //差价数据
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
    columns() {
      return [
        {
          title:'材料库存',
          dataIndex:'amount',
          scopedSlots:{customRender: 'amount'},
        },
        {
          title:'计划用量',
          dataIndex:'planNumber',
        },
        {
          title:'材料量差',
          dataIndex:'gap',
          scopedSlots:{customRender: 'gap'},
          customCell: (record)=> {  //table组件单元格样式事件
            return {
              style: {
                color: record.gap && record.gap > 0 ? undefined : 'red',
              }};},
        },
      ];
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
    // 详情
    view(record) {
      this.listMessage = [];
      this.status = ModalStatus.View;
      this.listMessage.push({
        amount:record.amount ? record.amount : 0,
        planNumber:record.planNumber,
        gap:record.amount ? record.amount - record.planNumber : 0-record.planNumber,
      });
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record.material, formFields) });
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
        title={"物资信息查看"}
        visible={this.visible}
        onCancel={this.close}
      >
        <div class="material-inquire-modal">
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item label="材料名称" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料名称'}
                    disabled={this.isShow}
                    v-decorator={[
                      'name',
                      {
                        initialValue: '',
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
              <a-col sm={12} md={12}>
                <a-form-item label="材料类别" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <MaterialTypeSelect
                    axios={this.axios}
                    placeholder={this.isShow ? '' : '请选择材料类别'}
                    disabled={this.isShow}
                    v-decorator={[
                      'typeId',
                      {
                        initialValue: '',
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
              <a-col sm={12} md={12}>
                <a-form-item label="所属专业" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <DataDictionaryTreeSelect
                    axios={this.axios}
                    groupCode={'Profession'}
                    placeholder={this.isShow ? '' : '请选择所属专业'}
                    disabled={this.isShow}
                    v-decorator={[
                      'professionId',
                      {
                        initialValue: '',
                        rules: [
                          {
                            required: true,
                            message: '请选择所属专业',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item label="材料规格" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料规格'}
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
              <a-col sm={12} md={12}>
                <a-form-item label="材料型号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料型号'}
                    disabled={this.isShow}
                    v-decorator={[
                      'model',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item label="材料单位" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入材料单位'}
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
              <a-col sm={12} md={12}>
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
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
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
              <a-table
                columns={this.columns}
                rowKey={record => record.planNumber}
                dataSource={this.listMessage}
                bordered={true}
              ></a-table>
            </a-row>
          </a-form>
        </div>
      </a-modal>
    );
  },
};
