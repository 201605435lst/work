import './style/index.less';
import SmBpmWorkflowTemplates from '../sm-bpm-workflow-templates';

export default {
  name: 'SmBpmWorkflowTemplatesSelectModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    placeholder: { type: String, default: '请点击选择' },
    value: { type: [String, Array], default: null },//已选项
    railwayId: { type: String, default: null },//线路id
    stationId: { type: String, default: null },//站点Id
    advancedCount: { type: Number, default: 6 },
  },
  data() {
    return {
      selectedValues: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedValues;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        console.log(value);
        this.selectedValues = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedValues = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedValues);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedValues = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="审批流程选择"
        class="sm-bpm-workflow-templates-select-modal"
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
                    this.selectedValues = this.selectedValues.filter(_item => _item.id !== item.id);
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
        <div class="selected-list">
          <SmBpmWorkflowTemplates
            ref="flowTemplate"
            axios={this.axios} 
            select={true} 
            onSuccess={(key)=>this.selectedId=key} 
            selected={this.selectedValues}
            forCurrentUser
            onChange={selected => {
              this.selectedValues = [];
              this.selectedValues = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
