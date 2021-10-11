import './style/index.less';
import SmTechnologyMaterialPlan from '../../sm-technology/sm-technology-material-plan';

export default {
  name: 'SmMaterialMaterialPlanSelectModal',
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
    advancedCount: { type: Number, default: 6 },
    isSelect: { type: Boolean, default: false },//是否选择模式
  },
  data() {
    return {
      selectedMaterialPlan: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedMaterialPlan;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedMaterialPlan = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedMaterialPlan = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedMaterialPlan);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedMaterialPlan = [];
    
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="用料计划选择"
        class="sm-material-plan-selected-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'environment'} />
                <span class="selected-name"> {item ? item.planName : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedMaterialPlan = this.selectedMaterialPlan.filter(_item => _item.id !== item.id);
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
        <div class="selected-material-plan-list">
          <SmTechnologyMaterialPlan
            axios={this.axios}
            small={true}
            isMaterialPlanSelect={true}
            multiple={true}
            isSelect
            selected={this.selectedMaterialPlan}
            advancedCount={this.advancedCount}
            onChange={selected => {
              this.selectedMaterialPlan = [];
              this.selectedMaterialPlan = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
