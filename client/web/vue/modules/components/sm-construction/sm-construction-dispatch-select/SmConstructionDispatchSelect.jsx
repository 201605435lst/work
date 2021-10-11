// 文件选择对话框
import SmConstructionDispatchSelectModal from './SmConstructionDispatchSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiDispatch from '../../sm-api/sm-construction/Dispatch';

import './style/index.less';

let apiDispatch = new ApiDispatch();


export default {
  name: 'SmConstructionDispatchSelect',

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
      selectedDispatchs: [], //已选择机房
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedDispatchs;
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
      apiDispatch = new ApiDispatch(this.axios);
    },

    dispatchSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选机房数据初始化
    initData() {
      let _selectedDispatchs = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiDispatch.get(id);
            if (requestIsSuccess(response)) {
              _selectedDispatchs.push(response.data);
            }
          }
        });
      }
      this.selectedDispatchs = _selectedDispatchs;
    },

    selected(value) {
      console.log(value);
      this.selectedDispatchs = value;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedDispatchs && this.selectedDispatchs.length > 0
            ? this.selectedDispatchs
            : [],
        );
      } else {
        console.log(this.selectedDispatchs[0]);
        this.$emit(
          'change',
          this.selectedDispatchs[0] ? this.selectedDispatchs[0] : null,
        );
      }
    },
  },
  render() {
    let _dispatch = this.tags.map(item => {
      return (
        <div class="selected-item">
          <div class="selected-name"> {item ? item.name : null} </div>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);
         
                this.selectedDispatchs = this.selectedDispatchs.filter(
                  _item => _item.id !== item.id,
                );
                this.selected(this.selectedDispatchs);
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
          'sm-construction-dispatch-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.dispatchSelect()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{_dispatch}</div>
        )}

        {/* 机房选择模态框 */}
        <SmConstructionDispatchSelectModal
          ref="SmConstructionDispatchSelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedDispatchs}
          multiple={this.multiple}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
          advancedCount={this.advancedCount}
        />
      </div>
    );
  },
};
