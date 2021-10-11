import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import { NodeTypeEnum } from './NodeTypeEnum';

let apiSubItemContent = new ApiSubItemContent();
// const formFields = ['name', 'nodeType','parentId'];
// 分部分项-详情 编辑添加界面
export default {
  name: 'SubItemContentModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      record: {},               // 表单绑定的对象
      form: { parentId: "00000000-0000-0000-0000-000000000000" },               // 表单
      nodeTypes: [],               // 可选择的 节点,根据 不同的 content NodeType 这个数组 也会不同
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return [
        { title: '节点名称', dataIndex: 'name' },
        { title: '节点类型', dataIndex: 'nodeTypeStr', width: 200, scopedSlots: { customRender: 'nodeTypeStr' } },
        { title: '操作', scopedSlots: { customRender: 'operations' } },
      ];
    },
    formFields() {
      return this.status === ModalStatus.Add ?
        ['name', 'nodeType', 'parentId', 'remarks']
        :
        ['name', 'parentId', 'remarks'];
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {}); // 创建表单
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
    },
    getNodeTypesByType(nodeType) {
      if (nodeType === NodeTypeEnum.All) { // 最高层级
        return [
          { id: NodeTypeEnum.Pro, name: "单位工程" },
          { id: NodeTypeEnum.SubPro, name: "子单位工程" },
          { id: NodeTypeEnum.Pos, name: "分部工程" },
          { id: NodeTypeEnum.SubPos, name: "子分部工程" },
        ];
      }
      if (nodeType === NodeTypeEnum.Pro) { // 单位
        return [{ id: NodeTypeEnum.SubItem, name: "子分项工程" }, { id: NodeTypeEnum.Item, name: "分项工程" }];
      }
      if (nodeType === NodeTypeEnum.SubPro) { //子单位
        return [{ id: NodeTypeEnum.SubItem, name: "子分项工程" }, { id: NodeTypeEnum.Item, name: "分项工程" }];
      }
      if (nodeType === NodeTypeEnum.Pos) { // 分部
        return [{ id: NodeTypeEnum.SubPos, name: "子分部工程" }, { id: NodeTypeEnum.Item, name: "分项工程" }];
      }
      if (nodeType === NodeTypeEnum.SubPos) { // 子分部
        return [{ id: NodeTypeEnum.Item, name: "分项工程" }];
      }
      if (nodeType === NodeTypeEnum.Item) { // 分项
        return [{ id: NodeTypeEnum.SubItem, name: "子分项工程" }, { id: NodeTypeEnum.Item, name: "分项工程" }];
      }
      // if (nodeType === NodeTypeEnum.SubItem) { // 子分项
      //   return [{ id: NodeTypeEnum.WorkSur,name:"作业面" }];
      // }
      return [];

    },
    add(record) {
      this.status = ModalStatus.Add;
      this.nodeTypes = this.getNodeTypesByType(record.nodeType);
      this.form.parentId = record.id;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.nodeTypes = this.getNodeTypesByType(record.nodeType);
      // 根据 record.nodeType 显示不同的 nodeType list
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, this.formFields) });
      });
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          // console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加设备台班信息
            response = await apiSubItemContent.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiSubItemContent.update(this.record.id, { ...values });
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        title={`分部分项${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >

        <a-form form={this.form}>
          <a-form-item
            label='父id'
            style={{ display: 'none' }}
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('parentId', {
                initialValue: this.form.parentId,
              })(<a-input placeholder={'请输入父id'} />)
            }
          </a-form-item>
          <a-form-item
            label='节点名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('name', {
                initialValue: '',
                rules: [{ required: true, message: '请输入节点名称！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入节点名称'} />)
            }
          </a-form-item>
          {/*<a-form-item
            label='备注'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('remarks', {
                initialValue: '',
                rules: [],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入备注'} />)
            }
          </a-form-item>*/}
          {this.status !== ModalStatus.Edit && this.nodeTypes.length > 0 ? (
            <a-form-item
              label='节点类型'
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}>
              {
                this.form.getFieldDecorator('nodeType', {
                  initialValue: undefined, // 选择框的话 默认值不要给0 给undefined
                  rules: [{ required: true, message: '请选择节点类型' }],
                })(
                  <a-select placeholder={'请选择节点类型'}>
                    {this.nodeTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
                  </a-select>,
                )
              }
            </a-form-item>
          ) : (<div></div>)}

        </a-form>

      </a-modal>
    );
  },
};
