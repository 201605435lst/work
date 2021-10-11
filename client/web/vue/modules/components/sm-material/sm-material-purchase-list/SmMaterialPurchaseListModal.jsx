import { requestIsSuccess, objFilterProps, CreateGuid, vP, vIf,getPurchaseListTypeStatus,arrayGroupBy } from '../../_utils/utils';
import ApiPurchaseList from '../../sm-api/sm-material/PurchaseList';
import ApiMaterial from '../../sm-api/sm-material/Material';
import { ModalStatus,ApprovalStatus,PurchaseListTypeStatus } from '../../_utils/enum';
import SmCommonSelect from '../../sm-common/sm-common-select';
import ApiProblem from '../../sm-api/sm-problem/Problem';
import * as utils from '../../_utils/utils';
import SmMaterialMaterialSelectModal from '../sm-material-material-select-modal';
import SmMaterialPurchasePlanSelect from '../sm-material-purchase-plan-select';
import SmBpmWorkflowTemplatesSelect from '../../sm-bpm/sm-bpm-workflow-templates-select';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import moment from 'moment';
import './style';
let apiPurchaseList = new ApiPurchaseList();
let apiProblem = new ApiProblem();
let apiMaterial = new ApiMaterial();

// 定义表单字段常量
const formFields = [
  'name',
  'planTime',
  'workflowTemplateId',
  'purchasePlanId',
  'content',
  'type',
];

export default {
  name: 'SmMaterialPurchaseListModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
  },
  data() {
    return {
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      status: ModalStatus.Hide, // 模态框状态
      selectedMaterials: [], //物资表格数据
      materialOption: [], //根据专业查到的资料
      professionId: '', //所选专业Id
      fileList: [],
      comments:"",// 审批意见
      isApproval:false,
      titleName:null,
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    isShow() {
      return this.status == ModalStatus.View;
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'materialName',
          width: 128,
          scopedSlots: { customRender: 'materialName' },
        },
        {
          title: '类别',
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '规格/型号',
          dataIndex: 'spec',
          scopedSlots: { customRender: 'spec' },
        },
        {
          title: '单位',
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '数量',
          dataIndex: 'number',
          scopedSlots: { customRender: 'number' },
        },
        {
          title: '合同价',
          dataIndex: 'price',
          scopedSlots: { customRender: 'price' },
        },

      ];
      if (!this.isShow) {
        col.push(
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },

          },
        );
      }
      return col;
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiPurchaseList = new ApiPurchaseList(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
      apiProblem = new ApiProblem(this.axios);
    },
    async save(isSubmit) {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.selectedMaterials && this.selectedMaterials.length == 0) {
            this.$message.warning('采购物资不能为空');
          } else if (this.dataSource && this.dataSource.some(item => !item.materialId)) {
            this.$message.warning('名称不能为空');
          } else {
            _=await this.$refs.fileUpload.commit();
            let _files = [];
            this.fileList.map(item => {
              _files.push({
                fileId: item.id,
              });
            });
            let _purchasePlanId = [];
            values.purchasePlanId.map(item => {
              _purchasePlanId.push({
                purchasePlanId: item,
              });
            });
            let data = {
              ...values,
              submit: isSubmit,
              purchaseListRltFiles: _files,
              state:isSubmit?ApprovalStatus.OnReview:ApprovalStatus.ToSubmit,
              purchaseListRltMaterials: this.selectedMaterials,
              purchaseListRltPurchasePlan:_purchasePlanId,
            };
        
            this.loading = true;
            if (this.status === ModalStatus.Add) {
              let response = await apiPurchaseList.create(data);
              if (requestIsSuccess(response)) {
                this.$message.success('添加成功');
                this.$emit('success');
                this.close();
              }
            } else if (this.status === ModalStatus.Edit || this.status === ModalStatus.View) {
              let response = await apiPurchaseList.update({ id: this.record.id, ...data });
              if (requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success');
                this.close();
              }
            }
            this.loading = false;
          }
        }
      });
    },
    add() {
      this.status = ModalStatus.Add;
    },
    // 审批
    approval(record){
      this.view(record);
      this.titleName = "采购计划审批";
      this.isApproval=true;
    },
    edit(record) {
      this.status = ModalStatus.Edit;
      this.viewOrEditHandle(record);
    },
    // 详情
    view(record) {
      this.status = ModalStatus.View;
      this.viewOrEditHandle(record);
    },
    //数据回显
    viewOrEditHandle(record){
      this.record=record;
      this.form = this.$form.createForm(this, {});
      this.record.planTime =
            this.record && this.record.planTime 
              ? moment(this.record.planTime)
              : null;
      //构造附件
      let _files = [];
      if (this.record && this.record.purchaseListRltFiles.length > 0) {
        this.record.purchaseListRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _files.push({
              id: file.id,
              name: file.name,
              file:file,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      let _material=[];
      if(this.record && this.record.purchaseListRltMaterials.length>0){
        this.record.purchaseListRltMaterials.map(item=>{
          _material.push({
            ...item.material,
            number:item.number,
            price:item.price,
          });
        });
      }
      this.selectedMaterials=_material;
      this.fileList = _files;
      this.record.purchasePlanId=this.record.purchaseListRltPurchasePlan &&this.record.purchaseListRltPurchasePlan.length>0?this.record.purchaseListRltPurchasePlan.map(item=>item.purchasePlanId):[];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.selectedMaterials = [];
      this.status = ModalStatus.Hide;
      this.selectedMaterials= []; //物资表格数据
      this.isApproval=false;
      this.titleName=null,
      this.fileList= [];
    },

    addMaterialInfo() {
      this.$refs.SmMaterialMaterialSelectModal.add();
    },

    deleteMaterial(record) {
      this.selectedMaterials = this.selectedMaterials.filter(item => item.id != record.id);
    },
    // 材料选择回调
    materialSelected(state, data) {
      if (data.length > 0) {
        data.forEach(a => {
          let instance = this.selectedMaterials.find(b => b.id == a.id);
          if (instance != null) {
            instance.count++;
          } else {
            this.selectedMaterials.push({
              ...a,
              typeName: a.type.name,
              number: 1,
            });
          }
        });
      }
    },
    planMaterial(_,plans){
      if (plans.length > 0) {
        let _materialPlans=[];
        plans.map(item=>{_materialPlans.push(...item.purchasePlanRltMaterials);});
        let _selectedMaterials=[];
        _materialPlans.map(a=>{
          _selectedMaterials.push({
            ...a.material,
            number: a.number?a.number:1,
          });
        });
        console.log(_selectedMaterials);
        let material_= this.materialhandle(_selectedMaterials);
        this.selectedMaterials=[...material_];
      }else{
        this.selectedMaterials=[];
      }
    },
    materialhandle(val_Arr){
      let sorted = arrayGroupBy(val_Arr, 'id');
      let _res=[];
      sorted.map(item=>{
        let count=0;
        item.map(b=>count+=b.number);
        _res=[
          ..._res,
          {
            ...item[0],
            number:count,
          },
        ];
      });
      return _res;
    },
    // 驳回审批
    async rejected() {
      let _this = this;
      if(this.comments==""){
        this.$message.warn("请输入审批意见");
        return;
      }
      let data={
        planId:this.record?this.record.id:null,
        content:this.comments,
        state:ApprovalStatus.UnPass,
      };
      this.$confirm({
        title: "温馨提示",
        content: h => <div style="color:red;">确定要驳回此审批吗？</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response=await apiPurchaseList.process(data);
            if (requestIsSuccess(response)) {
              _this.$message.success("操作成功");
              _this.$emit("success");
              _this.close();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    // 通过审批
    async approved() {
      if(this.comments==""){
        this.$message.warn("请输入审批意见");
        return;
      }
      let data={
        planId:this.record?this.record.id:null,
        content:this.comments,
        state:ApprovalStatus.Pass,
      };
      let response=await apiPurchaseList.process(data);
      if (requestIsSuccess(response)){
        if(response.data){
          this.$message.success("操作成功");
          this.$emit("success");
          this.close();
        }
      }
    },
  },
  render() {
    let Options = [];
    for (let item in PurchaseListTypeStatus) {
      Options.push(
        <a-select-option key={PurchaseListTypeStatus[item]}>
          {getPurchaseListTypeStatus(PurchaseListTypeStatus[item])}
        </a-select-option>,
      );
    }
    let materialMessage = (
      <div class="materialTable">
        <a-col sm={22} md={22}  offset={2}>
          <a-table
            pagination={false}
            columns={this.columns}
            size='small'
            rowKey={record => record.id}
            dataSource={this.selectedMaterials}
            scroll={
              {
                y: 300,
              }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1;
                },
                materialName: (text, record, index) => {
                  return record.name;
                },
                type: (text, record, index) => {
                  let result = record && record.type ? record.type.name : '';
                  return result;

                },
                spec: (text, record) => {
                  return record.spec;
                },
                unit: (text, record) => {
                  return record.unit;
                },
                number: (text, record) => {
                  if (this.status != ModalStatus.View) {
                    return <a-input-number
                      value={record.number}
                      min={1}
                      max={1000000}
                      size="small"
                      onChange={(v) => {
                        record.number = v;
                      }}
                    />;
                  } else return text;
                },
                price: (text, record) => {
                  return [
                    <a-input-number
                      min={1}
                      size="small"
                      disabled={this.isShow}
                      formatter={value => `￥ ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                      parser={value => value.replace(/\￥\s?|(,*)/g, '')}
                      value={record.price}
                      onChange={value => {
                        record.price = value;
                      }}
                    ></a-input-number>,
                  ];
                },
                operations: (text, record) => {
                  return [
                    <a onClick={() => this.deleteMaterial(record)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                  ];
                },
              },
            }}
          >
          </a-table>

        </a-col>
      </div>
    );


    return (
      <a-modal
        width={this.width}
        title={this.isApproval?this.titleName:`${this.title}采购单`}
        okText="保存"
        visible={this.visible}
        //footer={null}
        // onOk={this.isShow ? this.close : this.save}
        onCancel={this.close}
      >
        <div class="sm-material-purchase-plan-material-modal">
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
                <a-form-item label="计划时间" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-date-picker
                    style="width:100%"
                    format="YYYY-MM-DD"
                    disabled={this.isShow}
                    disabledDate={current => { return current < Date.now() - 8.64e7; }}
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
              <a-col sm={12} md={12}>
                <a-form-item label="采购计划" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <SmMaterialPurchasePlanSelect
                    placeholder={this.isShow ? '' : '请选择采购计划'}
                    disabled={this.isShow}
                    axios={this.axios}
                    isSelect
                    multiple
                    size="small"
                    onChange={this.planMaterial}
                    v-decorator={[
                      'purchasePlanId',
                      {
                        initialValue: [],
                        rules: [
                          {
                            required: true,
                            message: '请选择采购计划',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item label="审批流程" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <SmBpmWorkflowTemplatesSelect
                    placeholder={this.isShow ? '' : '请选择审批流程'}
                    disabled={this.isShow}
                    axios={this.axios}
                    isSelect
                    size="small"
                    v-decorator={[
                      'workflowTemplateId',
                      {
                        initialValue:undefined,
                        rules: [
                          {
                            required: true,
                            message: '请选择审批流程',
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item label="采购方式" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-select
                    placeholder="采购方式"
                    style="width:100%"
                    disabled={this.status == ModalStatus.View}
                    allowClear
                    v-decorator={[
                      'type',
                      {
                        initialValue: undefined,
                        rules: [
                          {
                            required: true,
                            message: '请选择采购方式',
                          },
                        ],
                      },
                    ]}
                  >
                    {Options}
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item label="附件上传" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <SmFileUpload
                    mode={this.status == ModalStatus.View ? "view" : "edit"}
                    axios={this.axios}
                    height={30}
                    multiple
                    ref="fileUpload"
                    onSelected={(item) => {
                      this.fileList = item;
                    }}
                    placeholder={this.status == ModalStatus.View ? '' : '请选择文件'}
                    fileList={this.fileList}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-textarea
                    rows="2"
                    placeholder={this.status == ModalStatus.View ? '' : '请输入'}
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'content',
                      {
                        initialValue: null,
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              {materialMessage}
              {/* 审批意见 */}
              {this.isApproval?  <a-col sm={24} md={24} style="margin-top:25px;" >
                <a-form-item label="审批意见" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-textarea
                    placeholder="请输入审批意见"
                    value={this.comments}
                    onChange={(e)=>{
                      this.comments=e.target.value;
                    }}
                  />
                </a-form-item>
              </a-col>:''}
            </a-row>
          </a-form>
        </div>
        <template slot="footer">
          {this.isApproval?
            <div>
              <a-button size="small" key="back" onClick={this.close}>取消</a-button>
              <a-button size="small" key="rejected" type="danger"  onClick={this.rejected}>驳回</a-button>
              <a-button size="small" key="approved" type="primary"  onClick={this.approved}>通过</a-button>
            </div>:
            <div>
              <a-button type="primary" size="small" onClick={() => this.close()}>
          取消
              </a-button>
              <a-button type="primary" size="small" onClick={() =>  this.status == ModalStatus.View ?this.close():this.save(false)}>
                {this.status == ModalStatus.View?"确定":"保存"}
              </a-button>
              {this.status == ModalStatus.View?"":
                <a-button type="primary" disabled={this.record.state==ApprovalStatus.UnPass ? false :this.record.submit} size="small" onClick={() =>  this.save(true)}>
          提交
                </a-button>}
            </div>
          }
        </template>
        <SmMaterialMaterialSelectModal
          ref="SmMaterialMaterialSelectModal"
          axios={this.axios}
          onChange={this.materialSelected} 
        />
      </a-modal>
    );
  },
};