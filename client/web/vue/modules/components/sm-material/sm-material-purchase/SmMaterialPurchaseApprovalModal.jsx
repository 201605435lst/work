import { requestIsSuccess, objFilterProps, CreateGuid, vP, vIf, getApprovalIdea } from '../../_utils/utils';
import ApiPurchase from '../../sm-api/sm-material/Purchase';
import ApiMaterial from '../../sm-api/sm-material/Material';
import ApiWorkflow from '../../sm-api/sm-bpm/Workflow';
import KFormBuild from '../../sm-bpm/sm-bpm-form-design/src/components/KFormBuild';
import { ModalStatus, ApprovalIdea, WorkflowStepState, WorkflowState } from '../../_utils/enum';
import moment from 'moment';

let apiPurchase = new ApiPurchase();
let apiMaterial = new ApiMaterial();
let apiWorkflow = new ApiWorkflow();

// 定义表单字段常量
const formFields = [
  'name',
  'planTime',
  'number',
  'price',
];

export default {
  name: 'SmMaterialPurchaseApprovalModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
  },
  data() {
    return {
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      status: ModalStatus.Hide, // 模态框状态
      listMessage: [], //物资表格数据
      textarea:'', //审批意见
      workflow: {
        state: WorkflowState.Finished,
        name: '',
      },
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
      return[
        {
          title:'序号',
          dataIndex:'index',
          width:60,
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'专业',
          dataIndex:'profession',
          width:134,
          //scopedSlots:{customRender: 'profession'},
        },
        {
          title:'名称',
          dataIndex:'materialName',
          width:128,
          //scopedSlots:{customRender: 'materialName'},
        },
        {
          title:'类别',
          dataIndex:'type',
        },
        {
          title:'型号',
          dataIndex:'model',
        },
        {
          title:'规格',
          dataIndex:'spec',
        },
        {
          title:'单位',
          dataIndex:'unit',
        },
        {
          title:'数量',
          dataIndex:'number',
        },
        {
          title:'合同价',
          dataIndex:'price',
        },
      ];
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiPurchase = new ApiPurchase(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
      apiWorkflow = new ApiWorkflow(this.axios);
    },

    // 审批
    async approval(record) {
      this.status = ModalStatus.View;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
      record.purchaseRltMaterials.map(item => {
        this.listMessage.push({
          profession: item.material.profession ? item.material.profession.name : null,
          materialName: item.material.name,
          type: item.material.type ? item.material.type.name : null,
          model: item.material.model,
          spec: item.material.spec,
          unit:item.material.unit,
          number: item.number,
          price: item.price,
        });
      });
      let response = await apiWorkflow.get(record.workflowId);
      if (requestIsSuccess(response)) {
        this.workflow = response.data;
      }
    },

    // 处理流程
    async process(stepState) {
      return new Promise(async (resolve, reject) => {
        let response = await apiWorkflow.process({
          sourceNodeId: this.workflow.currentUserActivedStep.source,
          targetNodeId: this.workflow.currentUserActivedStep.target,
          stepState,
          comments: this.textarea,
          id: this.workflow.id,
        });

        if (requestIsSuccess(response)) {
          if (stepState === WorkflowStepState.Approved) {
            this.$message.success('审批成功');
          } else if (stepState === WorkflowStepState.Stopped) {
            this.$message.success('审批已拒绝');
          }
          this.$emit('success');
          this.close();
        } else {
          reject();
        }
          
      });
    },



    // 关闭模态框
    close() {
      this.form.resetFields();
      this.listMessage = [];
      this.status = ModalStatus.Hide;
      //this.$emit('change',false);
    },
  },
  render() {
    let _approvalIdea = [];
    for (let item in ApprovalIdea) {
      _approvalIdea.push(
        <a-select-option key={ApprovalIdea[item]}>
          {getApprovalIdea(ApprovalIdea[item])}
        </a-select-option>,
      );
    }
    let materialMessage = (
      <div class="materialTable">
        <a-table
          columns={this.columns}
          dataSource={this.listMessage}
          rowKey={record => record.id}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1; 
              },
            },
          }}
        >
        </a-table>
      </div>
    );


    return (
      <a-modal
        width={this.width}
        title={"采购计划审批"}
        //okText="保存"
        visible={this.visible}
        // onOk={this.isShow ? this.close : this.save}
        onCancel={this.close}
      >
        <div class="material-modal">
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={12} md={12}>
                <a-form-item label="计划名称" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    placeholder={this.isShow ? '' : '请输入计划名称'}
                    disabled={this.isShow}
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
              <a-col sm={12} md={12}>
                <a-form-item label="用料计划时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                  <a-date-picker
                    style="width:100%"
                    format="YYYY-MM-DD"
                    disabled={this.isShow}
                    disabledDate={current => {return current < Date.now()- 8.64e7;}}
                    v-decorator={[
                      'planTime',
                      {
                        initialValue: moment(),
                        rules: [
                          {
                            required: true,
                            message: '计划时间',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              {materialMessage}

              <a-col sm={12} md={12}>
                <a-form-item label="快捷选择" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-select
                    placeholder="请选择"
                    onChange={value => this.textarea = getApprovalIdea(value)}
                  >
                    {_approvalIdea}
                  </a-select>
                </a-form-item>
              </a-col><a-col sm={24} md={24}>
                <a-form-item label="审批意见" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-textarea
                    placeholder="请输入审批意见"
                    rows="4"
                    value={this.textarea}
                    onInput={value => this.textarea = value.target.value}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </div>
        <template slot="footer">
          <a-button type="primary" onClick={() => this.close()}>
                取消
          </a-button>
          <a-button type="primary" onClick={() => this.process(WorkflowStepState.Stopped)}>
                拒绝
          </a-button>
          <a-button type="primary" onClick={() => this.process(WorkflowStepState.Approved)}>
                  同意
          </a-button>
        </template>
      </a-modal>
    );
  },
};