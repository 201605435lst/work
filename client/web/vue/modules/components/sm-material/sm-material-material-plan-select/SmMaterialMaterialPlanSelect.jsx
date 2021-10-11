// 文件选择对话框
import SmMaterialMaterialPlanSelectModal from './SmMaterialMaterialPlanSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiMaterialPlan from '../../sm-api/sm-technology/Material';
import './style/index.less';

let apiMaterialPlan = new ApiMaterialPlan();

export default {
  name: 'SmMaterialMaterialPlanSelect',

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
      selectedMaterialPlan: [], //已选择用料计划
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedMaterialPlan;
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
      apiMaterialPlan = new ApiMaterialPlan(this.axios);
    },
    materialPlanSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },
    //已选计划数据初始化
    initData() {
      let _selectedMaterialPlan = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiMaterialPlan.get(id);
            if (requestIsSuccess(response)) {
              _selectedMaterialPlan.push(response.data);
            }
          }
        });
      }
      this.selectedMaterialPlan = _selectedMaterialPlan;
    },

    selected(value) {
      console.log(value);
      this.selectedMaterialPlan = value;
      this.$emit(
        'change',this.selectedMaterialPlan,
      );
    },
  },

  render() {
    let materialplans = this.tags.map(item => {
      return (
        <div class="selected-item">
          <div class="selected-name"> 
            <a-tooltip placement="topLeft" title={item.planName}>
              {item ? item.planName : null}
            </a-tooltip>
          </div>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);
                this.selectedMaterialPlan = this.selectedMaterialPlan.filter(
                  _item => _item.id !== item.id,
                );
                this.$emit(
                  'change',this.selectedMaterialPlan,
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
          'material-plan-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.materialPlanSelect()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{materialplans}</div>
        )}

        {/* 材料选择模态框 */}
        <SmMaterialMaterialPlanSelectModal
          ref="SmMaterialMaterialPlanSelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedMaterialPlan}
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
