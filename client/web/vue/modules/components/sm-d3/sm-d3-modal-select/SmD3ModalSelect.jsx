// 文件选择对话框
import SmD3ModalSelectModal from './SmD3ModalSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiEquipment from '../../sm-api/sm-resource/Equipments';
import './style/index.less';

let apiEquipment = new ApiEquipment();

export default {
  name: 'SmD3ModalSelect',
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
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedValue: [], //已选择
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      console.log("tafs",this.selectedValue);
      return this.selectedValue;
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
      apiEquipment = new ApiEquipment(this.axios);
    },

    dSelected() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选数据初始化
    initData() {
      let _selectedValue = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiEquipment.get(id);
            if (requestIsSuccess(response) && response.data) {
              _selectedValue.push({ ...response.data, groupName: response.data.group && response.data.name });
            }
          }
        });
      }
      this.selectedValue = _selectedValue;
    },

    selected(value) {
      this.selectedValue = value;
      console.log(value);
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedValue && this.selectedValue.length > 0
            ? this.selectedValue.map(item => item.id)
            : [],
        );
      } else {
        this.$emit(
          'change',
          this.selectedValue[0] ? this.selectedValue[0].id : null,
        );
      }
    },
  },
  render() {
    console.log(this.tags);
    let _value = this.tags.map(item => {
      return (
        <div class="selected-item">
          <div class="selected-name"> {item ? item.name : null} </div>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);

                this.selectedValue = this.selectedValue.filter(
                  _item => _item.id !== item.id,
                );
                this.selected(this.selectedValue);
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
          'sm-d3-modal-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.dSelected()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{_value}</div>
        )}

        {/* 机房选择模态框 */}
        <SmD3ModalSelectModal
          ref="SmD3ModalSelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedValue}
          multiple={this.multiple}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
          advancedCount={this.advancedCount}
        />
      </div>
    );
  },
};
