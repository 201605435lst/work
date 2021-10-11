/**
 * 说明：新增用料计划
 * 作者：easten
 */
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ApiQuantity from '../../sm-api/sm-technology/Quantity';
import moment from 'moment';
let apiQuantity = new ApiQuantity();
export default {
  name: 'SmTechnologyMaterialPlanModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
    height: { type: Number, default: 800 },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      planName: `${moment().format('YYYY-MM-DD')}用料计划`,// 用料计划名称
      planDate: moment().format("YYYY-MM-DD") || null,// 用料计划时间
      loading: false,
      quantities: [],// 选择的工程量
    };
  },
  computed: {
    visible() {
      return this.status != ModalStatus.Hide;
    },
    dataSourceColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 70,
          align: 'center',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '专业名称',
          dataIndex: 'speciality',
          ellipsis: true,
          scopedSlots: { customRender: 'speciality' },
        },
        {
          title: '系统1',
          dataIndex: 'system1',
          ellipsis: true,
          scopedSlots: { customRender: 'system1' },
        },
        {
          title: '系统2',
          dataIndex: 'system2',
          ellipsis: true,
          scopedSlots: { customRender: 'system2' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '材料名称',
          dataIndex: 'productCategoryName',
          scopedSlots: { customRender: 'productCategoryName' },
          ellipsis: true,
        },
        {
          title: '规格/型号',
          dataIndex: 'spec',
          scopedSlots: { customRender: 'spec' },
          ellipsis: true,
        },
        {
          title: '工程数量',
          dataIndex: 'count',
          width: 100,
          align: 'center',
        },
        {
          title: '计量单位',
          dataIndex: 'unit',
          width: 100,
          align: 'center',
        },
      ];
    },
  },
  watch: {
  },
  async created() {
    this.initAxios();
    // this.refresh();
  },
  methods: {
    initAxios() {
      apiQuantity = new ApiQuantity(this.axios);
    },
    create(records) {
      this.status = ModalStatus.Add;
      // 执行操作
      this.quantities = records;
      // 重新初始化名称和时间
      this.planName = `${moment().format('YYYY-MM-DD')}用料计划`;// 用料计划名称
      this.planDate = moment().format("YYYY-MM-DD");// 用料计划时间
    },
    // 数据刷新
    async onOk() {
      if (this.planName == "") {
        this.$message.warn("计划名称不能为空");
        return;
      }
      if (this.planDate == "" || this.planDate == undefined) {
        this.$message.warn("计划时间不能为空");
        return;
      }
      let data = {
        planName: this.planName,
        planDate: this.planDate,
        materials: this.quantities.map(a => {
          return { id: a.id };
        }),
      };
      let response = await apiQuantity.createMaterialPlan(data);
      if (requestIsSuccess(response)) {
        if (response.data) {
          this.$message.success("用料计划生成成功");
          this.$emit("success");
          this.onClose();
        }
      }

    },
    onClose() {
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    return (<a-modal
      width={this.width}
      title='物料计划生成'
      okText="确定"
      visible={this.visible}
      onOk={this.onOk}
      onCancel={this.onClose}
    >
      <div class='sm-material-material-select-modal'>
        {/* 工具 */}
        <div class="head">
          <a-row gutter={16}>
            <a-col span={12} class="m-col">
              <span style='width:80px'>计划名称：</span>
              <a-input
                placeholder="请输入查询关键字"
                class="m-form"
                size='small'
                allowClear
                value={this.planName}
                onInput={event => {
                  this.planName = event.target.value;
                  // this.refresh();
                }}
              />
            </a-col>
            <a-col span={12} class="m-col">
              <span style='width:80px'>计划时间：</span>
              <a-date-picker 
                class="m-form" size='small' value={this.planDate ? moment(this.planDate) : null} onChange={(date) => this.planDate = date} />
            </a-col>
          </a-row>
        </div>
        {/* 已选择的内容 */}
        <div class='table-selected' style='margin-top:20'>
          <a-table
            columns={this.dataSourceColumns}
            rowKey={record => record.id}
            dataSource={this.quantities}
            pagination={false}
            loading={this.loading}
            size="small"
            scroll={{ y: 200 }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1;
                },
                spec: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                speciality: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                system1: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                system2: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                name: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
                productCategoryName: (text, record, index) => {
                  return <a-tooltip title={text}>{text}</a-tooltip>;
                },
              },
            }}
          >
          </a-table>
        </div>
      </div>
    </a-modal>
    );
  },
};
