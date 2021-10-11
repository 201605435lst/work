import { requestIsSuccess } from '../../_utils/utils';
import ApiLabel from '../../sm-api/sm-regulation/Label';
import SmRegulationLabelModal from './SmRegulationLabelModal';

let apiLabel = new ApiLabel();

export default {
  name: 'SmRegulationLabelTreeSelect',

  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    height: { type: Number, default: 70 }, // 当前选择框的大小
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容
    multiple: { type: Boolean, default: true }, //是否多选，默认多选
    placeholder: { type: String, default: '请点击选择标签' },
    bordered: { type: Boolean, default: true }, // 边框模式
    simple: { type: Boolean, default: false }, //view状态下的显示
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedLabels: [], //已选择标签
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },

    tags() {
      return this.selectedLabels;
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
        this.initAxios();
        this.initData();
      },
      immediate: true,
    },
  },

  created() {
    this.initAxios();
    this.initData();
  },

  methods: {
    initAxios() {
      apiLabel = new ApiLabel(this.axios);
    },

    labelSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选签章数据初始化
    initData() {
      let _selectedLabels = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiLabel.get(id);
            if (requestIsSuccess(response)) {
              _selectedLabels.push(response.data);
            }
          }
        });
      }
      this.selectedLabels = _selectedLabels;
    },

    selected(value) {
      this.selectedLabels = value;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedLabels && this.selectedLabels.length > 0
            ? this.selectedLabels.map(item => item.id)
            : [],
        );
      } else {
        this.$emit('change', this.selectedLabels[0] ? this.selectedLabels[0].id : null);
      }
    },
  },

  render() {
    let labels = null;
    if (this.simple) {
      labels = (
        <div style="display:flex;font-size:15px;">
          {this.tags.map(item => {
            return <div style="padding-right:5px;margin-bottom:3px">{item ? item.name : null}</div>;
          })}
        </div>
      );
    } else {
      labels = this.tags.map(item => {
        return (
          <div class="selected-item">
            <div class="selected-name"> {item ? item.name : null} </div>
            {!this.disabled ? (
              <span
                class="btn-close"
                onClick={e => {
                  e.stopPropagation();
                  this.iValue = this.iValue.filter(id => id !== item.id);
                  if (this.multiple) {
                    this.$emit('change', this.iValue);
                  } else {
                    this.$emit('change', this.iValue[0]);
                  }
                  this.selectedLabels = this.selectedLabels.filter(_item => _item.id !== item.id);
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
    }

    return (
      <div
        class={{
          'label-select-panel': true,
          'label-select-panel-simple': this.simple,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={{
          height: this.bordered ? this.height + 'px' : 'auto',
        }}
        onClick={() => this.labelSelect()}
      >
        {this.tags.length == 0 ? (
          <label class="tip">{this.placeholder}</label>
        ) : (
          <div class="selected">{labels}</div>
        )}

        <SmRegulationLabelModal
          ref="SmRegulationLabelModal"
          axios={this.axios}
          permissions={this.permissions}
          visible={this.iVisible}
          value={this.selectedLabels}
          multiple={this.multiple}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
        />
      </div>
    );
  },
};
