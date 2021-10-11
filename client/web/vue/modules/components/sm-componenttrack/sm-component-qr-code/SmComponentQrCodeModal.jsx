import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess } from '../../_utils/utils';
import { form as formConfig, tips } from '../../_utils/config';
import ApiComponentTrack from '../../sm-api/sm-componenttrack/componentTrackRecord';
import ApiTrackProcess from '../../sm-api/sm-componenttrack/trackProcess';
import SmCommonSelect from '../../sm-common/sm-common-select';
let apiComponentTrack = new ApiComponentTrack();
let apiTrackProcess = new ApiTrackProcess();
// 定义表单字段常量
const formFields = [
  'trackProcessId',
];
export default {
  name: 'SmComponentQrCodeModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      dataSource:[],
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
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
  
        {
          title: '设备分类',
          ellipsis: true,
          dataIndex: 'componentCategory',
          scopedSlots: { customRender: 'componentCategory' },
        },
        {
          title: '设备编码',
          ellipsis: true,
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '设备型号',
          dataIndex: 'productCategory',
          ellipsis: true,
          scopedSlots: { customRender: 'productCategory' },
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
      apiComponentTrack = new ApiComponentTrack(this.axios);
      apiTrackProcess = new ApiTrackProcess(this.axios);
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.dataSource=[];
      this.status = ModalStatus.Hide;
    },
    batchDefault(datasource) {
      this.dataSource=datasource;
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _dataSourceId=[];
          _dataSourceId=this.dataSource.map(item=>item.id);
          let data = {
            ...values,
            componentTrackIds:_dataSourceId,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiComponentTrack.create(data);
          } else {
            this.close();
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
    return (
      <a-modal
        class="sm-sm-component-track-modal"
        title={`${this.title}跟踪计划`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        width={1000}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item label="计划名称" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmCommonSelect
                  placeholder={this.status == ModalStatus.View ? '' : '请选择跟踪计划名称'}
                  API={apiTrackProcess}
                  v-decorator={[
                    'trackProcessId',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '跟踪计划不能为空',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
        <a-row gutter={24}>
          <a-col sm={24} md={24}>
            <a-table
              rowKey={record => record.id}
              columns={this.columns}
              dataSource={this.dataSource}
              {...{
                scopedSlots: {
                  index: (text, record, index) => {
                    return index + 1; 
                  },
                  name: (text, record) => {
                    let name = record && record.equipment && record.equipment.name ? record.equipment.name : '';
                    return (
                      <a-tooltip placement="topLeft" title={name}>
                        <span>{name}</span>
                      </a-tooltip>
                    );
                  },
                  componentCategory: (text, record) => {
                    let result = record && record.equipment.componentCategory
                      ? record.equipment.componentCategory.parent
                        ? record.equipment.componentCategory.parent.name + '-' + record.equipment.componentCategory.name
                        : ''
                      : '';
                    return (
                      <a-tooltip placement="topLeft" title={result}>
                        <span>{result}</span>
                      </a-tooltip>
                    );
                  },
                  code: (text, record) => {
                    let result = record && record.equipment && record.equipment.code ? record.equipment.code : '';
                    return (
                      <a-tooltip placement="topLeft" title={result}>
                        <span>
                          {result}
                        </span>
                      </a-tooltip>
                    );
                  },
                  productCategory: (text, record) => {
                    let result = record && record.equipment.productCategory ? record.equipment.productCategory.name : '';
                    return (
                      <a-tooltip placement="topLeft" title={result}>
                        <span>
                          {result}
                        </span>
                      </a-tooltip>
                    );
                  },
                },
              }}
            >
            </a-table>
          </a-col>
        </a-row>
      </a-modal>
    );
  },
};
