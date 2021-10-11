// 文件选择对话框
import SmBpmWorkflowTemplatesSelectModal from './SmBpmWorkflowTemplatesSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiWorkflowTemplate from '../../sm-api/sm-bpm/WorkflowTemplate';
let apiWorkflowTemplate = new ApiWorkflowTemplate();
import './style/index.less';
export default {
  name: 'SmBpmWorkflowTemplatesSelect',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null }, //异步请求方法
    height: { type: Number, default: null }, // 当前选择框的大小
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容
    placeholder: { type: String, default: '请点击选择' },
    bordered: { type: Boolean, default: true }, // 边框模式
    advancedCount: { type: Number, default: 2 },
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedValues: [], //已选择
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
        this.initAxios();
        this.initData();
      },
      immediate: true,
    },
  },

  created() {
  },

  methods: {
    initAxios() {
      apiWorkflowTemplate = new ApiWorkflowTemplate(this.axios);
    },

    modalSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },
    //已选数据初始化
    initData() {
      let _selectedValues = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiWorkflowTemplate.get(id);
            if (requestIsSuccess(response) && response.data) {
              _selectedValues.push({ ...response.data });
            }
          }
        });
      }
      this.selectedValues = _selectedValues;
    },
    selected(value) {
      this.selectedValues = value;
      this.$emit(
        'change',
        this.selectedValues[0] ? this.selectedValues[0].id : null,
      );
    },
  },
  render() {
    let manufacturers = this.tags.map(item => {
      return (
        <div class="selected-item">
          <div class="selected-name"> {item ? item.name : null} </div>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);
                this.selectedValues = this.selectedValues.filter(
                  _item => _item.id !== item.id,
                );
                this.selected(this.selectedValues);
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
          'sm-bpm-workflow-templates-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.modalSelect()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{manufacturers}</div>
        )}

        {/* 流程选择模态框 */}
        <SmBpmWorkflowTemplatesSelectModal
          ref="SmBpmWorkflowTemplatesSelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedValues}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
          advancedCount={this.advancedCount}
        />
      </div>
    );
  },
};
