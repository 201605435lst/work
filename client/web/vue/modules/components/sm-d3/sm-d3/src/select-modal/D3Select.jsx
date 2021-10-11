// 文件选择对话框
import SelectModal from './SelectModal';
export default {
  name: 'D3Select',
  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null }, //异步请求方法
    height: { type: Number, default: null }, // 当前选择框的大小
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容
    multiple: { type: Boolean, default: false }, //是否多选，默认单选
    placeholder: { type: String, default: '请点击选择' },
    bordered: { type: Boolean, default: true }, // 边框模式
    advancedCount: { type: Number, default: 2 },
    isSelect: { type: Boolean, default: false },//是否选择模式
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedValues: [], //已选择用料计划
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedValues;
    },

  },

  watch: {
    value: {
      handler(nVal, oVal) {
        if (nVal instanceof Array) {
          this.iValue = nVal;
        } else {
          this.iValue = !!nVal ? [nVal] : [];
        }
      },
      immediate: true,
    },
  },

  created() {
  },

  methods: {
    dSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },
    selected(value) {
      this.selectedValues = value;
      this.$emit(
        'change',this.selectedValues,
      );
    },
  },

  render() {
    let dValues = this.tags.map(item => {
      return (
        <div class="selected-item">
          <div class="selected-name"> 
            <a-tooltip placement="topLeft" title={item.name}>
              {item ? item.name : null}
            </a-tooltip>
          </div>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);
                this.selectedValues = this.selectedValues.filter(
                  _item => _item.id !== item.id,
                );
                this.$emit(
                  'change',this.selectedValues,
                );
              }}
            >
              <a-icon type="close" />
            </span>
          ) : (
            undefined
          )}
        </div>
      );
    });

    return (
      <div
        class={{
          'd-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.dSelect()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{dValues}</div>
        )}
        {/* 材料选择模态框 */}
        <SelectModal
          ref="SelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedValues}
          multiple={this.multiple}
          isSelect
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
          advancedCount={this.advancedCount}
        />
      </div>
    );
  },
};
