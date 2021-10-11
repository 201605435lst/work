import './style/index.less';
import SmMaterialPurchasePlan from '../sm-material-purchase-plan';

export default {
  name: 'SmMaterialPurchasePlanSelectModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    placeholder: { type: String, default: '请点击选择' },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
    railwayId: { type: String, default: null },//线路id
    stationId: { type: String, default: null },//站点Id
    advancedCount: { type: Number, default: 6 },
    size: { type: String, default: 'default' },
    isSelect: { type: Boolean, default: false },//是否选择模式
  },
  data() {
    return {
      selectedPurchaseLists: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedPurchaseLists;
    },
  },
  watch: {
    value: {
      handler: function (value) {
        this.selectedPurchaseLists = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedPurchaseLists = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedPurchaseLists);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedPurchaseLists = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="采购计划选择"
        class="sm-purchase-lists-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'environment'} />
                <span class="selected-name"> {item ? item.name : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedPurchaseLists = this.selectedPurchaseLists.filter(_item => _item.id !== item.id);
                  }}
                >
                  <a-icon type="close" />
                </span>
              </div>;
            })

          ) : (
            <span style="margin-left:10px;">请选择</span>
          )}
        </div>
        <div class="selected-purchase-list">
          <SmMaterialPurchasePlan
            axios={this.axios}
            isModalSelect={true}
            multiple={this.multiple}
            isSelect
            size={this.size}
            selected={this.selectedPurchaseLists}
            advancedCount={this.advancedCount}
            onChange={selected => {
              this.selectedPurchaseLists = [];
              this.selectedPurchaseLists = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
