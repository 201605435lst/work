import { ModalStatus, RepairTagKeys, YearMonthPlanState } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import ApiYearMonthPlan from '../../sm-api/sm-cr-plan/YearMonthPlan';
import { requestIsSuccess, objFilterProps } from '../../_utils/utils';
import './style/index';
let apiYearMonthPlan = new ApiYearMonthPlan();

const formFields = [
  'skyligetType',
  'equipmentLocation',
  'total',
  'planCount',
  'col_1',
  'col_2',
  'col_3',
  'col_4',
  'col_5',
  'col_6',
  'col_7',
  'col_8',
  'col_9',
  'col_10',
  'col_11',
  'col_12',
];

export default {
  name: 'SmCrPlanYearPlanModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    state: { type: Number, default: null }, //年表状态
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: {},
      skyligetTypes: [],
      skyligetTypeSel: [],
      queryParams: {
        treeCheckable: true,
        skyligetType: null,//选中的天窗类型字符串
      },
      message1: null,
      message2: null,
      message3: null,
      message4: null,
      tabColumns: [],
      tabData: [],
      times: null,
    };
  },
  computed: {
    newFormFields() {
      let _newformFields = formFields;
      if (this.repairTagKey === RepairTagKeys.RailwayWired) {
        _newformFields.push('compiledOrganization');
      }
      return _newformFields;
    },
    title() {
      return utils.getModalTitle(this.status);
    },
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    indeterminate() {
      return this.skyligetTypes.length > 0 && this.skyligetTypes.length > this.skyligetTypeSel.length && this.skyligetTypeSel.length > 0;
    },
    isAllCheck() {
      return this.skyligetTypes.length > 0 && this.skyligetTypes.length == this.skyligetTypeSel.length;
    },
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.inintData();
  },
  methods: {
    initAxios() {
      apiYearMonthPlan = new ApiYearMonthPlan(this.axios);
    },
    async inintData() {
      //加载天窗计划数据源
      let response = await apiYearMonthPlan.getSkylightTypeList(null, this.repairTagKey);
      if (requestIsSuccess(response)) {
        this.skyligetTypes = response.data;
        if (this.repairTagKey == RepairTagKeys.RailwayHighSpeed) {
          let index = this.skyligetTypes.indexOf("各网管");
          if (index != -1)
            this.skyligetTypes.splice(index, 1);
        }
      }
    },
    //编辑按钮
    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record = JSON.parse(JSON.stringify(record));
      this.record.skyligetType = record && record.skyligetType ? record.skyligetType.split(',') : [];
      this.skyligetTypeSel = this.record.skyligetType;
      this.times = this.record.times;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(this.record, this.newFormFields) });
      });
    },
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    messageInfo(item) {
      let messageInfo = null;
      switch (item) {
      case "3":
        messageInfo = this.message1;
        break;
      case "6":
        messageInfo = this.message2;
        break;
      case "9":
        messageInfo = this.message3;
        break;
      case "12":
        messageInfo = this.message4;
        break;
      }
      return messageInfo;
    },
    //计划量验证
    async identifyPlan(values) {
      let sum = 0;
      let times = this.times;
      let result = false;
      if (times == 1 || isNaN(times)) {
        let _values = JSON.parse(JSON.stringify(values));
        for (let item in _values) {
          if (item.indexOf('col') !== -1) {
            _values[item] = _values[item] ? _values[item] : 0;
            sum += _values[item];
          }
        }
        if (values.planCount > 0 && sum == 0) {
          this.message4 = "月计划总量不能为空";
          result = true;
        } else {
          this.message4 = null;
        }
      }
      if (times == 2) {
        let sum_1 = values.col_1 + values.col_2 + values.col_3 + values.col_4 + values.col_5 + values.col_6;
        if (values.planCount > 0 && sum_1 == 0) {
          this.message2 = "1月-6月计划总量不能为空";
          result = true;
        }
        else {
          this.message2 = null;
        }
        let sum_2 = + values.col_7 + values.col_8 + values.col_9 + values.col_10 + values.col_11 + values.col_12;
        if (!result && values.planCount > 0 && sum_2 == 0) {
          this.message4 = "7月-12月计划总量不能为空";
          result = true;
        } else {
          this.message4 = null;
        }
        sum = sum_1 + sum_2;
      }
      if (times == 4) {
        let sum_1 = values.col_1 + values.col_2 + values.col_3;
        if (values.planCount > 0 && sum_1 == 0) {
          this.message1 = "1月-3月计划总量不能为空";
          result = true;
        } else {
          this.message1 = null;
        }
        let sum_2 = values.col_4 + values.col_5 + values.col_6;
        if (!result && values.planCount > 0 && sum_2 == 0) {
          this.message2 = "4月-6月计划总量不能为空";
          result = true;
        } else {
          this.message2 = null;
        }
        let sum_3 = values.col_7 + values.col_8 + values.col_9;
        if (!result && values.planCount > 0 && sum_3 == 0) {
          this.message3 = "7月-9月计划总量不能为空";
          result = true;
        } else {
          this.message3 = null;
        }
        let sum_4 = values.col_10 + values.col_11 + values.col_12;
        if (!result && values.planCount > 0 && sum_4 == 0) {
          this.message4 = "10月-12月计划总量不能为空";
          result = true;
        } else {
          this.message4 = null;
        }
        sum = sum_1 + sum_2 + sum_3 + sum_4;
      }

      if (!result && sum>0 && values.planCount==0) {
        this.$message.error('年计划总数量不能为空');
        result = true;
      } 
      if (!result && sum != values.planCount) {
        this.message4 = "月计划量总和与年计划总量不相等";
        result = true;
      } 
      if(!result && sum == values.planCount){
        this.message4 = null;
      }
      return result;
    },
    async ok() {
      // 数据提交
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            let result = await this.identifyPlan(values);
            let _values = JSON.parse(JSON.stringify(values));
            let sum = 0;
            for (let item in _values) {
              if (item.indexOf('col') !== -1) {
                _values[item] = _values[item] ? _values[item] : 0;
                sum += _values[item];
              }
            }
            if (!result && sum > 0) {
              if (!values.compiledOrganization && this.repairTagKey == RepairTagKeys.RailwayWired) {
                this.$message.error('编制执行单位不能为空');
                result = true;
              }
              if (!result && !values.equipmentLocation) {
                result = true;
                this.$message.error('设备处所不能为空');
              }
            }
            if (!result) {
              let response = null;
              if (this.status === ModalStatus.Edit) {
                let params = {
                  ...values,
                  id: this.record.id,
                  skyligetType: values.skyligetType.join(','),
                };
                response = await apiYearMonthPlan.updateYearMonthPlan(params, this.repairTagKey);
                if (requestIsSuccess(response)) {
                  // this.isAllCheck = false;
                  this.record = null;
                  this.skyligetTypeSel = [];
                  this.queryParams.skyligetType = '';
                  this.$message.success('操作成功');
                  this.close();
                  this.$emit('success');
                }
              }
            }
          }
        });

      }
    },
  },
  render() {
    let monthArray = [];
    if (this.record) {
      for (let item in this.record) {
        if (item.indexOf('col') !== -1) {
          let monthName = item.replace('col_', '');
          let temp = <a-col sm={8} md={8}>
            <a-form-item
              label={`${monthName}月`}
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-input-number
                placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                style="width:100%"
                min={0}
                disabled={this.state === YearMonthPlanState.Checking || this.state === YearMonthPlanState.Passed}
                precision={3}
                v-decorator={[
                  `${item}`,
                  {
                    initialValue: null,
                    rules: [
                      { pattern: /^(|[0-9])+(\.[0-9]+)?$/, message: '请输入正确数字' },
                      { required:true, message: '请输入正确数字' },
                    ],
                  },
                ]}
              />
            </a-form-item>
            {monthName == 3 || monthName == 6 || monthName == 9 || monthName == 12 ?
              <a-col sm={24} md={24}>
                <span class="message">
                  {this.messageInfo(monthName)}
                </span>
              </a-col>
              : ""}
          </a-col>;
          monthArray.push(temp);
        }
      }
    }
    return (
      <a-modal
        class="sm-cr-plan-year-plan-modal"
        width={800}
        title={`${this.title}年计划`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form} >
          <a-row gutter={24}>
            <a-col sm={15} md={15}>
              <a-form-item
                label="天窗类型"
                label-col={{ span: 5 }}
                wrapper-col={{ span: 19 }}
              >
                <a-checkbox-group
                  options={this.skyligetTypes}
                  onChange={value => {
                    this.skyligetTypeSel = value;
                    this.queryParams.skyligetType = value.join(',');
                  }}
                  v-decorator={[
                    'skyligetType',
                    {
                      initialValue: [],
                      rules: [
                        { required: true, message: '请选择天窗类型' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={9} md={9}>
              <a-form-item
                label="是否全选"
                label-col={{ span: 9 }}
                wrapper-col={{ span: 15 }}
              >
                <a-checkbox
                  checked={this.isAllCheck}
                  indeterminate={this.indeterminate}
                  onChange={value => {
                    this.skyligetTypeSel = value.target.checked ? this.skyligetTypes : [];

                    this.queryParams.skyligetType = this.skyligetTypeSel.join(',');
                    this.$nextTick(() => {
                      this.form.setFieldsValue({ skyligetType: this.skyligetTypeSel });
                    });
                  }}
                >
                  全选
                </a-checkbox>
              </a-form-item>
            </a-col>
            {this.repairTagKey == RepairTagKeys.RailwayWired ?
              <a-col sm={24} md={24}>
                <a-form-item
                  label="编制执行单位"
                  label-col={{ span: 3 }}
                  wrapper-col={{ span: 21 }}
                >
                  <a-input
                    placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                    v-decorator={[
                      'compiledOrganization',
                      {
                        initialValue: null,
                        // rules: [
                        //   { required: true, message: '请输入编制执行单位' },
                        // ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              : ''}

            <a-col sm={24} md={24}>
              <a-form-item
                label="设备处所"
                label-col={{ span: 3 }}
                wrapper-col={{ span: 21 }}
              >
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  v-decorator={[
                    'equipmentLocation',
                    {
                      initialValue: null,
                      // rules: [
                      //   { required: true, message: '请输入设备处所' },
                      // ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="总设备数量"
                label-col={{ span: 6 }}
                wrapper-col={{ span: 18 }}
              >
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  disabled={this.state === YearMonthPlanState.Checking || this.state === YearMonthPlanState.Passed}
                  {...{
                    scopedSlots: {
                      addonAfter: () => {
                        let value=this.record?isNaN(this.record.times)?this.record && this.record.times:`每年${this.record?this.record.times:''}次`:'';
                        return <a-tooltip title={value} placement="topLeft"><div class="addonAfter">{value}</div></a-tooltip>;
                      },
                    },
                  }}
                  onChange={(event) => {
                    if(!isNaN(this.record.times)){
                      let value = event.target.value;
                      if (value) {
                        value = value * this.record.times;
                      }
                      this.$nextTick(() => {
                        this.form.setFieldsValue({ planCount: value });
                      });
                    }
                  }}
                  v-decorator={[
                    'total',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true, message: '请输入总设备数量',
                          pattern: /^(|[0-9])+(\.[0-9]+)?$/, message: '请输入正确数字',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="年计划总数量"
                label-col={{ span: 7 }}
                wrapper-col={{ span: 17 }}
              >
                <a-input-number
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  disabled={this.state === YearMonthPlanState.Checking || this.state === YearMonthPlanState.Passed}
                  style="width:100%"
                  min={0}
                  precision={3}
                  v-decorator={[
                    'planCount',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <div>
                <span>每月计划数量</span>
                <a-divider />
              </div>
            </a-col>
            {monthArray}
          </a-row>
        </a-form>
      </a-modal>
    );
  },
};
