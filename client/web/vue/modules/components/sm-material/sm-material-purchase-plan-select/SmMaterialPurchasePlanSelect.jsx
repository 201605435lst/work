// 文件选择对话框
import SmMaterialPurchasePlanSelectModal from './SmMaterialPurchasePlanSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiPurchasePlan from '../../sm-api/sm-material/PurchasePlan';
import './style/index.less';

let apiPurchasePlan = new ApiPurchasePlan();


export default {
  name: 'SmMaterialPurchasePlanSelect',

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
    size: { type: String, default: 'default' },
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedPurchaseLists: [], //已选择采购计划
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedPurchaseLists;
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
      apiPurchasePlan = new ApiPurchasePlan(this.axios);
    },
    purchasePlanSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选采购计划数据初始化
    initData() {
      let _selectedPurchaseLists = [];
      if (this.iValue && this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiPurchasePlan.get(id);
            if (requestIsSuccess(response)) {
              _selectedPurchaseLists.push(response.data);
            }
          }
        });
      }
      this.selectedPurchaseLists = _selectedPurchaseLists;
    },

    selected(value) {
      console.log(value);
      this.selectedPurchaseLists = value;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedPurchaseLists && this.selectedPurchaseLists.length > 0
            ? this.selectedPurchaseLists.map(item => item.id)
            : [],this.selectedPurchaseLists,
        );
      } else {
        this.$emit(
          'change',
          this.selectedPurchaseLists && this.selectedPurchaseLists[0] ? this.selectedPurchaseLists[0].id : null,this.selectedPurchaseLists,
        );
      }
    },
  },
  render() {
    let materialListPlans = this.tags.map(item => {
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
             
                this.selectedPurchaseLists = this.selectedPurchaseLists.filter(
                  _item => _item.id !== item.id,
                );
                this.selected(this.selectedPurchaseLists);

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
          'purchase-plan-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={this.height && this.tags.length > 0 ? {
          height: this.height + 'px',
        } : {
          height: '32px',
        }}
        onClick={() => this.purchasePlanSelect()}
      >
        {this.tags.length == 0 ? (
          <span class="tip">{this.placeholder}</span>
        ) : (
          <div class="selected-box">{materialListPlans}</div>
        )}
        {/* 采购计划选择模态框 */}
        <SmMaterialPurchasePlanSelectModal
          ref="SmMaterialPurchasePlanSelectModal"
          axios={this.axios}
          size={this.size}
          visible={this.iVisible}
          value={this.selectedPurchaseLists}
          multiple={this.multiple}
          stationId={this.stationId}
          isSelect
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
          advancedCount={this.advancedCount}
        />
      </div>
    );
  },
};
