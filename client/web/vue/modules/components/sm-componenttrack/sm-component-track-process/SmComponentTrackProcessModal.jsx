import * as utils from '../../_utils/utils';
import { ModalStatus,NodeType,MemberType } from '../../_utils/enum';
import { requestIsSuccess, objFilterProps, CreateGuid,getNodeTypeTitle } from '../../_utils/utils';
import { form as formConfig, tips } from '../../_utils/config';
import ApiTrackProcess from '../../sm-api/sm-componenttrack/trackProcess';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
let apiTrackProcess = new ApiTrackProcess();

// 定义表单字段常量
const formFields = [
  'name',
  'remark',
];
export default {
  name: 'SmComponentTrackProcessModal',
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
          title:'序号',
          dataIndex:'index',
          width:60,
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'节点名称',
          dataIndex:'name',
          width:220,
          scopedSlots:{customRender: 'name'},
        },
        {
          title:'节点类型',
          dataIndex:'nodeType',
          scopedSlots:{customRender: 'nodeType'},
        },
        {
          title:'标记人员',
          dataIndex:'userId',
          scopedSlots:{customRender: 'userId'},
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
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
      apiTrackProcess = new ApiTrackProcess(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record=record;
      this.dataSource=record.nodes?record.nodes:[];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.dataSource=[];
      this.status = ModalStatus.Hide;
    },
    addItem(){
      let data={
        id:CreateGuid(),
        name:null,
        nodeType:undefined,
        userId:null,
      };
      this.dataSource=[...this.dataSource,data];
    },
    deleteItem(id){
      console.log(id);
      this.dataSource=this.dataSource.filter(item=>id!=item.id);
    },
    handleValue(node){
      let result=[];
      result.push({
        id: node.userId,
        type: MemberType.User,
      });
      console.log("result",result);
      return result;
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if(this.dataSource && this.dataSource.length==0){
            this.$message.warning('未添加跟踪计划节点');
          }else if(this.dataSource && this.dataSource.some(item=>!(item.name && item.nodeType && item.userId))){
            this.$message.warning('节点信息不能为空');
          }else{
            let data = {
              ...values,
              nodes:this.dataSource,
            };
            console.log(this.dataSource);
            console.log(data);
            let response = null;
            if (this.status === ModalStatus.Add) {
              //添加
              response = await apiTrackProcess.create(data);
            } else if (this.status === ModalStatus.Edit) {
              // 编辑
              response = await apiTrackProcess.update({ id: this.record.id, ...data });
            } else {
              this.close();
            }
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.close();
              this.$emit('success');
            }
          }
        }
      });
      this.loading = false;
    },
  },
  render() {
    let Options=[];
    for(let item in NodeType){
      Options.push(
        <a-select-option key={NodeType[item]}>
          {getNodeTypeTitle(NodeType[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        class="sm-component-track-process-modal"
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
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入计划名称'}
                  disabled={this.status == ModalStatus.View  }
                  v-decorator={[
                    'name',
                    {
                      initialValue: '',
                      rules: [
                        {
                          required: true,
                          message: '请输入计划名称',
                          whitespace: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  rows="1"
                  placeholder={this.status == ModalStatus.View ? '' : '请输入'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'remark',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-button type="primary" onClick={this.addItem} class="add-button">添加节点</a-button>
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
                  name: (text, record, index) => {
                    return [
                      <a-input
                        allowClear
                        value={record.name}
                        placeholder={this.status === ModalStatus.View ? '' : '请输入节点名称'}
                        disabled={this.status == ModalStatus.View  }
                        onChange={e =>{
                          record.name = e.target.value;
                        }}
                      ></a-input>,
                    ];
                  },
                  nodeType: (text, record, index) => {
                    return [
                      <a-select
                        allowClear
                        style="width:100%"
                        placeholder={this.status === ModalStatus.View ? '' : '请选择节点类型'}
                        disabled={this.status == ModalStatus.View  }
                        value={record.nodeType}
                        onChange={value => {
                          record.nodeType = value;
                        }}
                      >
                        {Options}
                      </a-select>,
                    ];
                  },
                  userId: (text, record,index) => {
                    return [
                      <SmSystemMemberSelect
                        allowClear
                        height={32}
                        shouIconSelect={true}
                        showUserTab={true}
                        userMultiple={false}
                        bordered={true}
                        simple={true}
                        value={record && record.userId && record.userId.length>0?this.handleValue(record):[]}
                        placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                        axios={this.axios}
                        disabled={this.status == ModalStatus.View}
                        onChange={value => {
                          record.userId=Object.assign(...value) ? Object.assign(...value).id : '';
                        }}
                      />,
                    ];
                  },
                 
                  operations: (text, record) => {
                    console.log(record);
                    return [
                      <a onClick={() => this.deleteItem(record.id)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                    ];
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
