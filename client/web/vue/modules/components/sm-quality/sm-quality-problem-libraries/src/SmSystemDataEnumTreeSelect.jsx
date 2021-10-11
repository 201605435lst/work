//根据枚举来，传入数字，传出数字，显示文本的选择框
import * as utils from '../../../_utils/utils';
import * as enums from '../../../_utils/enum';

export default {
  name: 'SmSystemDataEnumTreeSelect',
  props: {
    value: { type: [Number, String] },
    disabled: { type: Boolean, default: false },
    allowClear: { type: Boolean, default: true },
    placeholder: { type: String, default: '请选择' },
    enum: { type: String, default: '' },// 枚举的字段
    utils: { type: String, default: '' },// 获取文本的字段
  },
  data() {
    return {
      dataDictonaries: [], // 列表数据源
      iValue: null,
    };
  },
  watch: {
    value: {
      handler(nVal) {
        this.iValue = utils[this.utils](Number(nVal))||nVal || undefined;
      },
      immediate: true,
    },
  },
  render() {
    let riskLevels = [];
    for (let item in enums[this.enum]) {
      riskLevels.push(
        <a-select-option key={`${enums[this.enum][item]}`}>{utils[this.utils](Number(enums[this.enum][item]))}</a-select-option>,
      );
    }
    return (
      <a-select
        placeholder={this.placeholder}
        disabled={this.disabled}
        allowClear={this.allowClear}
        style="width:100%"
        value={this.iValue}
        onChange={index => { this.iValue = utils[this.utils](Number(index)); this.$emit('change', index); }}
      >
        {riskLevels}
      </a-select>
    );
  },
};