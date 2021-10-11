import { ModalStatus, RepairTagKeys, YearMonthPlanState } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import ApiYearMonthPlan from '../../sm-api/sm-cr-plan/YearMonthPlan';
import { requestIsSuccess, objFilterProps } from '../../_utils/utils';

let apiYearMonthPlan = new ApiYearMonthPlan();

const formFields = [
  'skyligetType',
  'equipmentLocation',
  'total',
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
  'col_13',
  'col_14',
  'col_15',
  'col_16',
  'col_17',
  'col_18',
  'col_19',
  'col_20',
  'col_21',
  'col_22',
  'col_23',
  'col_24',
  'col_25',
  'col_26',
  'col_27',
  'col_28',
];

export default {
  name: 'SmCrPlanMonthOfYearPlanModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    state: { type: Number, default: null }, //年度月表状态
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: {},
      skyligetTypes: [],
      sumDay: null,
    };
  },
  computed: {
    newFormFields() {
      let _newformFields = formFields;
      if (this.repairTagKey === RepairTagKeys.RailwayWired) {
        _newformFields.push('compiledOrganization');
      }
      if (this.sumDay > 28) {
        _newformFields.push('col_29');
        if (this.sumDay > 29) {
          _newformFields.push('col_30');
          if (this.sumDay > 30) {
            _newformFields.push('col_31');
          }
        }

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
      return this.record.skyligetType && this.skyligetTypes.length > this.record.skyligetType.length && this.record.skyligetType.length > 0;
    },
    isAllCheck() {
      return this.record.skyligetType && this.skyligetTypes.length == this.record.skyligetType.length;
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
    async edit(record, sumDay) {
      this.sumDay = sumDay;
      this.status = ModalStatus.Edit;
      this.record = JSON.parse(JSON.stringify(record));
      this.record.skyligetType = record.skyligetType ? record.skyligetType.split(',') : [];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(this.record, this.newFormFields) });
      });
    },
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    async ok() {

      // 数据提交
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            let _values = JSON.parse(JSON.stringify(values));
            let sum = 0;
            for (let item in _values) {
              if (item.indexOf('col') !== -1) {
                _values[item] = _values[item] ? _values[item] : 0;
                sum += _values[item];
              }
            }
            if (sum > 0) {
              if (this.repairTagKey === RepairTagKeys.RailwayWired) {
                if (_values.compiledOrganization == null || _values.compiledOrganization == undefined || _values.compiledOrganization.trim().length === 0) {
                  this.$message.warning('编制执行单位不能为空');
                  return;
                }
              }
              if (_values.equipmentLocation == null || _values.equipmentLocation == undefined || _values.equipmentLocation.trim().length === 0) {
                this.$message.warning('设备处所不能为空');
                return;
              }
              if (_values.skyligetType.length === 0) {
                this.$message.warning('天窗类型不能为空');
                return;
              }
            }
            if (sum !== _values.total) {
              this.$message.warning('当前年月月计划数量之和不等于总数量，请重新维护数据');
              return;
            }
            let data = {
              ..._values,
              id: this.record.id,
              skyligetType: _values.skyligetType.join(','),
              equipmentLocation: _values.equipmentLocation.trim(),
              compiledOrganization: _values.compiledOrganization ? _values.compiledOrganization.trim() : '',
            };

            let response = null;
            if (this.status === ModalStatus.Edit) {
              response = await apiYearMonthPlan.updateYearMonthPlan(data, this.repairTagKey);
              if (requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.close();
                this.$emit('success');
              }
            }

          }
        });
      }
    },
  },
  render() {
    let dailyArray = [];
    if (this.record) {
      for (let item in this.record) {
        if (item.indexOf('col') !== -1 && parseInt(item.replace('col_', '')) <= this.sumDay) {
          let monthName = item.replace('col_', '');
          let temp = <a-col sm={24} md={4}>
            <a-form-item label={`${monthName}`} label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
              <a-input-number
                placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                style="width:100%"
                disabled={this.state === YearMonthPlanState.Checking || this.state === YearMonthPlanState.Passed}
                min={0}
                precision={3}
                v-decorator={[
                  `${item}`,
                  {
                    initialValue: null,
                    rules: [
                      { pattern: /^(|[0-9])+(\.[0-9]+)?$/, message: '请输入正确数字' },
                      { required: true, message: '请输入正确数字' },
                    ],
                  },
                ]}
              />
            </a-form-item>
          </a-col>;
          dailyArray.push(temp);
        }
      }
    }
    return (
      <a-modal
        class="sm-cr-plan-month-of-year-plan-modal"
        width={1000}
        title={`${this.title}月度计划`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form} >
          <a-row gutter={16}>
            <a-col sm={24} md={16}>
              <a-form-item label="天窗类型" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-checkbox-group
                  options={this.skyligetTypes}
                  onChange={value => {
                    this.record.skyligetType = value;
                  }}
                  v-decorator={[
                    'skyligetType',
                    {
                      initialValue: [],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={8}>
              <a-form-item label="是否全选" label-col={{ span: 7 }} wrapper-col={{ span: 17 }}>
                <a-checkbox
                  checked={this.isAllCheck}
                  indeterminate={this.indeterminate}
                  onChange={value => {
                    this.record.skyligetType = value.target.checked ? this.skyligetTypes : [];
                    this.form.setFieldsValue({ 'skyligetType': this.record.skyligetType });
                  }}
                >
                  全选
                </a-checkbox>
              </a-form-item>
            </a-col>
            {this.repairTagKey === RepairTagKeys.RailwayWired ? <a-col sm={24} md={8}>
              <a-form-item label="编制执行单位" label-col={{ span: 10 }} wrapper-col={{ span: 14 }}>
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  v-decorator={[
                    'compiledOrganization',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col> : undefined}

            <a-col sm={24} md={this.repairTagKey === RepairTagKeys.RailwayWired ? 8 : 12}>
              <a-form-item label="设备处所" label-col={{ span: 7 }} wrapper-col={{ span: 17 }}>
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  v-decorator={[
                    'equipmentLocation',
                    {
                      initialValue: null,
                      rules: [
                        { required: this.isRequired, message: '请输入设备处所', whitespace: true },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={this.repairTagKey === RepairTagKeys.RailwayWired ? 8 : 12}>
              <a-form-item label="总数量" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  placeholder={this.status === ModalStatus.View ? '' : '请输入'}
                  // {...{
                  //   scopedSlots: {
                  //     addonAfter: () => {
                  //       let value = !isNaN(Number(this.record.times)) ? `每年${this.record.times}次` : this.record.times;
                  //       return <a-tooltip title={value} placement="topLeft"><div class="addonAfter">{value}</div></a-tooltip>;
                  //     },
                  //   },
                  // }}
                  disabled
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
            {dailyArray}
          </a-row>
        </a-form>
      </a-modal>
    );
  },
};
