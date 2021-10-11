/**
 * 说明：新增用料计划
 * 作者：easten
 */
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ModalStatus, ApprovalStatus } from '../../_utils/enum';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ApiQuantity from '../../sm-api/sm-technology/Quantity';
import SmMaterialMaterialSelectModal from '../../sm-material/sm-material-material-select-modal';
import ApiMaterial from '../../sm-api/sm-technology/Material';
import moment from 'moment';
import { v4 as uuid } from 'uuid';
let apiQuantity = new ApiQuantity();
let apiMaterial = new ApiMaterial();
export default {
  name: 'SmTechnologyMaterialPlanModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
    height: { type: Number, default: 800 },
    //view:{type:Boolean,default:false},// 详情模式  ，只展示 关联的内容信息
  },
  data() {
    return {
      status: ModalStatus.Hide,
      planName: `${moment().format("YYYY-MM-DD")}用料计划`,// 用料计划名称
      planDate: moment(),// 用料计划时间
      loading: false,
      selectedMaterials: [],// 材料id
      isCreate: false, // 是否新增材料
      selectVisible: false,// 材料选择框
      planId: null,// 用料计划id
      title: '',
      comments:"",// 审批意见
      isApproval:false, // 是否审批为审批弹窗
    };
  },
  computed: {
    visible() {
      return this.status != ModalStatus.Hide;
    },
    dataSourceColumns() {
      let columns = [{
        title: '序号',
        dataIndex: 'index',
        align:'center',
        width: 70,
        scopedSlots: { customRender: 'index' },
      },
      {
        title: '类别',
        align:'center',
        dataIndex: 'typeName',
        ellipsis: true,
        scopedSlots: { customRender: 'typeName' },
      },
      {
        title: '名称',
        dataIndex: 'name',
        ellipsis: true,
        scopedSlots: { customRender: 'name' },
      },
      {
        title: '规格型号',
        dataIndex: 'spec',
        ellipsis: true,
        scopedSlots: { customRender: 'spec' },
      },
      {
        title: '计量单位',
        dataIndex: 'unit',
        align:'center',
        width: 100,
        ellipsis: true,
        scopedSlots: { customRender: 'unit' },
      },
 
      {
        title: '数量',
        dataIndex: 'count',
        width: 150,
        align:'center',
        scopedSlots: { customRender: 'count' },
      }];
      if (this.status === ModalStatus.Add || this.status === ModalStatus.Edit) {
        columns.push({
          title: '操作',
          width: 100,
          scopedSlots: { customRender: 'operation' },
        });
      }
      return columns;
    },
    materials() {
      return this.selectedMaterials;
    },
  },
  watch: {
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiQuantity = new ApiQuantity(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
    },
    create() {
      this.title="用料计划新增";
      this.planName = `${moment().format("YYYY-MM-DD")}用料计划`;
      this.planDate = moment();
      this.status = ModalStatus.Add;
      this.isCreate = true;
      this.selectedMaterials = [];
    },
    view(record) {
      this.initState(record,"用料计划查看");
      this.status = ModalStatus.View;
 
    },
    edit(record) {
      this.initState(record,"用料计划编辑");
      this.status = ModalStatus.Edit;
      this.isCreate=true;
    },
    initState(record,title){
      this.title=title;
      this.planId = record.id;
      this.planName = record.planName;
      this.planDate = record.planTime;
      this.selectedMaterials = [];
      this.getMaterialById();
    },
    // 审批
    approval(record){
      this.view(record);
      this.title = "用料计划审批";
      this.isApproval=true;
    },
    // 数据提交保存
    async onOk() {
      if (this.planName == "") {
        this.$message.warn("计划名称不能为空");
        return;
      }
      if (this.planDate == ""||this.planDate==undefined) {
        this.$message.warn("计划时间不能为空");
        return;
      }
      if (this.materials.length == 0) {
        this.$message.warn("请添加材料信息");
        return;
      }
      let data = {
        planName: this.planName,
        planDate: this.planDate,
        materials: this.materials.map(a => {
          return {
            id: a.id,
            count: a.count,
          };
        }),
      };
      if (this.status === ModalStatus.Add) {
        let response = await apiMaterial.planCreate(data);
        if (requestIsSuccess(response)) {
          if (response.data) {
            this.$message.info("计划添加成功");
            this.$emit("success");
            this.onClose();
          }
        }
      }else if (this.status === ModalStatus.Edit){
        // 提交更新
        data.id=this.planId;
        let response = await apiMaterial.planUpdate(data);
        if (requestIsSuccess(response)){
          if (response.data) {
            this.$message.info("计划更新成功");
            this.$emit("success");
            this.onClose();
          }
        }
      }
      else {
        this.onClose();
      }
 
    },
    onClose() {
      this.selectedMaterials=[];
      this.status = ModalStatus.Hide;
      this.isCreate = false;
      this.isApproval=false;
    },
    // 删除
    async materialDelete(record) {
      if( this.status ===ModalStatus.Add){
        this.selectedMaterials = this.selectedMaterials.filter(a => a.id != record.id);
      }
      else if(this.status ===ModalStatus.Edit){
        // 执行删除操作
        let response=await apiMaterial.planMaterialDelete(record.id);
        if (requestIsSuccess(response)){
          if(response.data){
            this.$message.info("材料已删除");
            this.selectedMaterials = this.selectedMaterials.filter(a => a.id != record.id);
          }
        }
      }
    },
    // 添加材料
    materialAdd() {
      this.selectVisible = true;
    },
    // 材料选择回调
    materialSelected(state, data) {
      this.selectVisible = state;
      if (data && data.length > 0) {
        data.forEach(a => {
          let instance = this.selectedMaterials.find(b => b.id == a.id);
          if (instance != null) {
            instance.count++;
          } else {
            this.selectedMaterials.push({
              ...a,
              typeName: a.type.name,
              count: 1,
            });
          }
        });
      }
      console.log(this.selectedMaterials);
    },
    // 根据计划id获取材料信息
    async getMaterialById() {
      let response = await apiMaterial.getPlanMaterials(this.planId);
      if (requestIsSuccess(response)) {
        if (response) {
          let data = response.data;
          data.forEach(a => {
            this.selectedMaterials.push({
              ...a.material,
              count: a.count,
              typeName: a.material.type.name,
            });
          });
        }
      }
    },
    // 驳回审批
    async rejected() {
      let _this = this;
      if(this.comments==""){
        this.$message.warn("请输入审批意见");
        return;
      }
      let data={
        planId:this.planId,
        content:this.comments,
        status:ApprovalStatus.UnPass,
      };
      this.$confirm({
        title: "温馨提示",
        content: h => <div style="color:red;">确定要驳回此审批吗？</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response=await apiMaterial.process(data);
            if (requestIsSuccess(response)) {
              _this.$message.success("操作成功");
              _this.$emit("success");
              _this.onClose();
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
        planId:this.planId,
        content:this.comments,
        status:ApprovalStatus.Pass,
      };
      let response=await apiMaterial.process(data);
      if (requestIsSuccess(response)){
        if(response.data){
          this.$message.success("操作成功");
          this.$emit("success");
          this.onClose();
        }
      }
    },
  },
  render() {
    return (<a-modal
      width={this.width}
      title={this.title}
      okText="确定"
      visible={this.visible}
      onOk={this.onOk}
      onCancel={this.onClose}
    >
      <div class='sm-technology-material-plan-modal'>
        {/* 工具 */}
        <div class="head">
          <a-row gutter={16}>
            <a-col span={this.status == ModalStatus.View?12:11} class="m-col">
              <span style='width:20%'>计划名称：</span>
              <a-input
                placeholder="请输入查询关键字"
                class="m-form"
                size='small'
                style='width:80%'
                value={this.planName}
                disabled={this.status == ModalStatus.View}
                onInput={event => {
                  this.planName = event.target.value;
                }}
              />
            </a-col>
            <a-col span={this.status == ModalStatus.View?12:11} class="m-col">
              <span style='width:20%'>计划时间：</span>
              <a-date-picker
                class="m-form"
                size='small'
                style='width:80%'
                value={this.planDate?moment(this.planDate):null}
                disabled={this.status == ModalStatus.View}
                onChange={(date) => this.planDate = date}
              />
 
            </a-col>
            {this.status == ModalStatus.View ? null : <a-col span={2} class="m-col">
              <a-button style="width:100%" type="primary" size="small" onClick={this.materialAdd}>添加</a-button>
            </a-col>}
          </a-row>
        </div>
        {/* 已选择的内容 */}
        <div class='table-selected' style='margin-top:20'>
          <a-table
            columns={this.dataSourceColumns}
            rowKey={record => record.id}
            dataSource={this.materials}
            pagination={false}
            loading={this.loading}
            size="small"
            scroll={{ y: 300 }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1;
                },
                type: (text, record, index) => {
                  return text;
                },
                spec: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                name: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                count: (text, record, index) => {
                  if (this.isCreate) {
                    return <a-input-number
                      value={record.count}
                      min={1}
                      max={1000000}
                      size="small"
                      onChange={(v) => {
                        record.count = v;
                      }}
                    />;
                  } else return text;
                },
                operation: (text, record, index) => {
                  return <a onClick={() => this.materialDelete(record)}>删除</a>;
                },
              },
            }}
          >
          </a-table>
        </div>
        {/* 审批意见 */}
        {this.isApproval?<div style="margin-top:25px;">
          <a-textarea
            placeholder="请输入审批意见"
            auto-size={{ minRows: 2, maxRows: 6 }}
            value={this.comments}
            onChange={(e)=>{
              this.comments=e.target.value;
            }}
          />
        </div>:null}
        {/* 材料选择框 */}
 
        <SmMaterialMaterialSelectModal axios={this.axios} visible={this.selectVisible} onChange={this.materialSelected} />
      </div>
      <template slot="footer">
        {this.isApproval ? <div>
          <a-button size="small" key="back" onClick={this.onClose}>取消</a-button>
          <a-button size="small" key="rejected" type="danger"  onClick={this.rejected}>驳回</a-button>
          <a-button size="small" key="approved" type="primary"  onClick={this.approved}>通过</a-button>
        </div> :
          <div>
            <a-button size="small" key="back" onClick={this.onClose}>取消</a-button>
            <a-button size="small" key="submit" type="primary" onClick={this.onOk}>确定</a-button>
          </div>
        }
      </template>
    </a-modal>
    );
  },
};
 