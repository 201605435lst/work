import { requestIsSuccess, objFilterProps, CreateGuid, vP, vIf } from '../../_utils/utils';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import MaterialSelectByProfession from '../sm-material-material-select-by-profession';
import ApiPurchase from '../../sm-api/sm-material/Purchase';
import ApiMaterial from '../../sm-api/sm-material/Material';
import { ModalStatus } from '../../_utils/enum';
import moment from 'moment';

let apiPurchase = new ApiPurchase();
let apiMaterial = new ApiMaterial();

// 定义表单字段常量
const formFields = [
  'name',
  'planTime',
  'number',
  'price',
];

export default {
  name: 'SmMaterialPurchaseModal',
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
      materialOption: [], //根据专业查到的资料
      professionId:'', //所选专业Id

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
      return this.isShow ? [
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
          scopedSlots:{customRender: 'profession'},
        },
        {
          title:'名称',
          dataIndex:'materialName',
          width:128,
          scopedSlots:{customRender: 'materialName'},
        },
        {
          title:'类别',
          dataIndex:'type',
          scopedSlots:{customRender: 'type'},
        },
        {
          title:'型号',
          dataIndex:'model',
          scopedSlots:{customRender: 'model'},
        },
        {
          title:'规格',
          dataIndex:'spec',
          scopedSlots:{customRender: 'spec'},
        },
        {
          title:'单位',
          dataIndex:'unit',
          scopedSlots:{customRender: 'unit'},
        },
        {
          title:'数量',
          dataIndex:'number',
          scopedSlots:{customRender: 'number'},
        },
        {
          title:'合同价',
          dataIndex:'price',
          scopedSlots:{customRender: 'price'},
        },
      ] : 
        [
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
            scopedSlots:{customRender: 'profession'},
          },
          {
            title:'名称',
            dataIndex:'materialName',
            width:128,
            scopedSlots:{customRender: 'materialName'},
          },
          {
            title:'类别',
            dataIndex:'type',
            scopedSlots:{customRender: 'type'},
          },
          {
            title:'型号',
            dataIndex:'model',
            scopedSlots:{customRender: 'model'},
          },
          {
            title:'规格',
            dataIndex:'spec',
            scopedSlots:{customRender: 'spec'},
          },
          {
            title:'单位',
            dataIndex:'unit',
            scopedSlots:{customRender: 'unit'},
          },
          {
            title:'数量',
            dataIndex:'number',
            scopedSlots:{customRender: 'number'},
          },
          {
            title:'合同价',
            dataIndex:'price',
            scopedSlots:{customRender: 'price'},
          },
          {
            title:'操作',
            dataIndex:'operations',
            scopedSlots: { customRender: 'operations' },
            fixed: 'right',
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
    },
    save(isSubmit){
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = JSON.parse(JSON.stringify(values));
          let data = {
            ..._values,
            state: isSubmit ? 2 : 1,
            purchaseRltMaterials:this.listMessage,
          };
          this.loading = true;

          if (this.status === ModalStatus.Add) {
            let response = await apiPurchase.create(data);
            if (requestIsSuccess(response)) {
              this.$message.success('添加成功');
              this.$emit('success');
              this.close();
            }
          } else if (this.status === ModalStatus.Edit) {
            let _data = { id: this.record.id, updateState: isSubmit ? true : false, ...data };
            console.log(_data);
            let response = await apiPurchase.update(_data);
            if (requestIsSuccess(response)) {
              this.$message.success('操作成功');
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
      this.addMaterialInfo(record.purchaseRltMaterials);
      
    },
    // 详情
    view(record) {
      this.status = ModalStatus.View;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
      this.addMaterialInfo(record.purchaseRltMaterials);
      
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.listMessage = [];
      this.materialOption = [];
      this.professionId = '';
      this.status = ModalStatus.Hide;
      //this.$emit('change',false);
    },
    
    addMaterialInfo(value){
      let newMaterialDataList = [];
      if (value !== null) {
        value.map(item => {
          this.listMessage.push({
            key:item.id,
            profession: item.material.professionId,
            materialId: item.material.id,
            type:item.material.type ? item.material.type.name : null,
            model: item.material.model,
            spec: item.material.spec,
            unit: item.material.unit,
            number: item.number,
            price: item.price,
          });
        });
      }
      else {//空白添加
        newMaterialDataList = [
          {
            key: CreateGuid(),
            profession: '',
            materialId: '',
            type: null,
            model:'',
            spec: '',
            unit:'',
            number: 1,
            price: '',
          },
        ];
      }
      this.listMessage = [...this.listMessage, ...newMaterialDataList];
    },

    deleteMaterial(key){
      this.listMessage = this.listMessage.filter(item => item.key != key);
    },

    //根据专业获取材料
    async getListByProfessionId(id,index) {
      if(id != this.professionId){
        this.professionId = id;
        this.listMessage[index].type = null;
        this.listMessage[index].model = '';
        this.listMessage[index].spec = '';
        this.listMessage[index].unit = '';
        this.listMessage[index].number = 1;
        this.listMessage[index].price = '';

        let response = await apiMaterial.getByProfessionId(id);
        if (requestIsSuccess(response)) {
          this.materialOption = [];
          for (let item of response.data) {
            this.materialOption.push(<a-select-option key={item.id}>
              {item.name}
            </a-select-option>);
          }
        }
      }
    },

    async getMaterialDetail(index,id){
      if(id){
        let response = await apiMaterial.get(id);
        if (requestIsSuccess(response)) {
          let data = response.data;
          this.listMessage[index].type = data.type ? data.type.name : null;
          this.listMessage[index].model = data.model;
          this.listMessage[index].spec = data.spec;
          this.listMessage[index].unit = data.unit;
          this.listMessage[index].price = data.price;
        }
      }
    },
  },
  render() {
    let materialMessage = (
      <div class="materialTable">
        {this.isShow ? undefined :
          <a-button type="primary" icon="plus" size="small" shape="round" class="physicalButton" onClick={()=>this.addMaterialInfo(null)} disabled={this.isShow}>新增</a-button>
        }
        <a-table
          columns={this.columns}
          dataSource={this.listMessage}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1; 
              },
              profession: (text, record,index) => {
                return [
                  <DataDictionaryTreeSelect
                    axios={this.axios}
                    groupCode={'Profession'}
                    placeholder={this.isShow ? '' : '所属专业'}
                    value={record.profession}
                    disabled={this.isShow}
                    onChange={value => {this.listMessage[index].profession = value;}}
                  />,
                ];
              },
              materialName: (text, record, index) => {
                return [
                  <MaterialSelectByProfession
                    axios={this.axios}
                    placeholder='请选择材料'
                    value={record.materialId}
                    disabled={this.isShow}
                    professionId={this.listMessage[index].profession}
                    onChange={value => {
                      this.listMessage[index].materialId = value;
                      this.getMaterialDetail(index,value);
                    }}
                  />,
                ];
              },
              type: (text, record, index) => {
                return [
                  <a-input
                    value={record.type}
                    disabled={true}
                  ></a-input>,
                ];
              },
              model: (text, record) => {
                return [
                  <a-input
                    value={record.model}
                    disabled={true}
                  ></a-input>,
                ];
              },
              spec: (text, record) => {
                return [
                  <a-input
                    value={record.spec}
                    disabled={true}
                  ></a-input>,
                ];
              },
              unit: (text, record) => {
                return [
                  <a-input
                    disabled={true}
                    value={record.unit}
                  ></a-input>,
                ];
              },
              number: (text, record) => {
                return [
                  <a-input-number
                    min={1}
                    disabled={this.isShow}
                    precision={0} //精度为0，只能为整数
                    value={record.number}
                    onChange={value =>{
                      record.number = value;
                    }}
                  ></a-input-number>,
                ];
              },
              price: (text, record) => {
                return [
                  <a-input-number
                    min={1}
                    disabled={this.isShow}
                    formatter={value => `￥ ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                    parser={value => value.replace(/\￥\s?|(,*)/g, '')}
                    value={record.price}
                    onChange={value =>{
                      record.price = value;
                    }}
                  ></a-input-number>,
                ];
              },
              operations: (text, record) => {
                return [
                  <a onClick={() => this.deleteMaterial(record.key)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                ];
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
        title={this.status == ModalStatus.Add ? "新建采购计划" : this.isShow ? "查看" : "编辑"}
        okText="保存"
        visible={this.visible}
        //footer={null}
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
                <a-form-item label="采购计划时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
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
            </a-row>
          </a-form>
        </div>
        <template slot="footer">
          <a-button type="primary" onClick={() => this.close()}>
                取消
          </a-button>
          <a-button type="primary" onClick={() => this.isShow ? this.close() : this.save(false)}>
                保存
          </a-button>
          {this.record.state == 1 ? 
            <a-button type="primary" onClick={() => this.save(true)}>
                  提交
            </a-button>
            : undefined}
        </template>
      </a-modal>
    );
  },
};