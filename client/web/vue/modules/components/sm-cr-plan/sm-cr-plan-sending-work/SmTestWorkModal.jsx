import { ModalStatus, SelectablePlanType, RepairLevel, StandTestType } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import ApiWorkOrder from '../../sm-api/sm-cr-plan/WorkOrder';
import './style';

let apiWorkOrder = new ApiWorkOrder();
export default {
  name: 'SmTestWorkModal',
  props: {
    axios: { type: Function, default: null },
    sendingWorkId: { type: String, default: null },
    disabled: { type: Boolean, default: false },
    isShow:{type:Boolean,default:true},//是否显示检修记录
  },
  data() {
    return {
      status: ModalStatus.Hide,
      cloumns: [],
      dataSource: [],
      dataObject: {},
      modalName: null,
      testNumber: null,
      loading: false,
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },
  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiWorkOrder = new ApiWorkOrder(this.axios);
    },

    async add(record) {
      this.status = ModalStatus.Add;
      let equipmentTestResultList = record.equipmentTestResultList;
      if (equipmentTestResultList == undefined || equipmentTestResultList.length === 0) {
        this.$message.error('测试项不存在');
        return;
      }

      this.testNumber = record.id;
      this.modalName = record.workContent;
      
      this.cloumns = [];
      if (this.isShow) {
        this.cloumns.push({
          title: '检修记录',
          width: 110,
          fixed: 'left',
          customRender: (text, row, index) => {
            return (
              <a-input
                value={row.checkResult}
                disabled={this.disabled}
                onChange={value => (row.checkResult = value.target.value)}
              ></a-input>
            );
          },
        });
      }
   
      equipmentTestResultList.map(item => {
        //阈值判断
        let isShow = false;
        let max = null;
        let min = null;
        if (item.maxRated !== undefined && item.minRated !== undefined) {
          max = item.maxRated;
          min = item.minRated;
        }
        //添加值
        this.dataObject[item.testName] = item.testResult;
        this.dataObject.checkResult = item.checkResult;
        this.dataObject.key = utils.CreateGuid();
        this.cloumns.push({
          title: item.testName,
          ellipsis: true,
          dataIndex: item.testName,
          width: 150,
          customRender: (text, row, index) => {
            let warnFont =
              "输入值与阈值不符" + `(${min}~${max}${record.unit == undefined ? '' : record.unit})`;
            let warnText = (
              <div class="warnTextContent" title={warnFont}>
                {warnFont}
              </div>
            );
            let items = [
              <a-input
                value={row[item.testName]}
                disabled={this.disabled}
                onChange={value => (row[item.testName] = value.target.value)}
              ></a-input>,
            ];
            let num = parseFloat(row[item.testName]);
            if (item.testType == StandTestType.Number && max != 0 && min != 0) {
              if (num > max || num < min) {
                isShow = true;
              } else {
                isShow = false;
              }
            }
            if (isShow) {
              items.push(warnText);
            }
          
            return { children: items };
          },
        });
      });
  
      if (!this.disabled) {
        this.cloumns.push({
          title: '操作',
          width: 60,
          fixed: 'right',
          customRender: (text, row, index) => {
            return (
              <a-icon
                type="delete"
                disabled={this.disabled}
                style="color:red"
                onClick={() => {
                  this.deleteItem(row.key);
                }}
              />
            );
          },
        });
      }
      //获取存在的测试附加项目
      let data = {
        workOrderId: this.sendingWorkId,
        number: this.testNumber,
      };
      let response = await apiWorkOrder.getWorkOrderTestAdditional(data);
      if (utils.requestIsSuccess(response) && response.data) {
        this.dataSource = response.data;
      }
    },
    close() {
      this.dataSource = [];
      this.status = ModalStatus.Hide;
    },
    async save() {
      if (!this.disabled) {
        this.loading = true;
        let jsonData = JSON.stringify(this.dataSource);
        let data = {
          workOrderId: this.sendingWorkId,
          testConctent: jsonData,
          number: this.testNumber,
        };
        let response = await apiWorkOrder.createWorkOrderTestAdditional(data);
        if (utils.requestIsSuccess(response) && response.data) {
          this.$message.info('添加成功');
          this.loading = false;
          this.close();
        }
      } else {
        this.close();
      }
     
    },

    //添加列
    addCloumns() {
      let dataObject = new Object();
      for (let item in this.dataObject) {
        dataObject[item] = this.dataObject[item];
        dataObject.key = utils.CreateGuid();
      }
      this.dataSource = [...this.dataSource, dataObject];
    },

    //删除列
    deleteItem(key) {
      this.dataSource = this.dataSource.filter(item => item.key !== key);
    },
  },
  render() {
    return (
      <a-modal
        class="SmTestWorkModal"
        visible={this.visible}
        onCancel={this.close}
        onOk={this.save}
        width={1400}
        title={`${this.modalName}添加测试值`}
        loading={this.loading}
        // destroyOnClose={true}
        // rowKey={record => record.key}
      >
        <a-button
          onClick={() => {
            this.addCloumns();
          }}
          type="primary"
          style="margin-bottom:10px"
          loading={this.loading}
          disabled={this.disabled}
        >
          添加
        </a-button>
        <a-table
          columns={this.cloumns}
          align="center"
          bordered
          dataSource={this.dataSource}
          pagination={false}
          scroll={{ x: 1350 }}
        ></a-table>
      </a-modal>
    );
  },
};
