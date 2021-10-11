import './style/index.less';
import SmConstructionDispatchs from '../sm-construction-dispatchs';

export default {
  name: 'SmConstructionDispatchSelectModal',
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
  },
  data() {
    return {
      selectedDispatch: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedDispatch;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedDispatch = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedDispatch = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedDispatch);
      this.onClose();
    },
    onClose() {
      this.$emit('change');
      this.selectedDispatch = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="派工单选择"
        class="sm-construction-dispatch-select-modal"
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
                    this.selectedDispatch = this.selectedDispatch.filter(_item => _item.id !== item.id);
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
        <div class="selectedDispatch-list">
          <SmConstructionDispatchs
            axios={this.axios}
            isSimple={true}
            passed={true}
            isForDaily={true}
            iMultiple={this.multiple}
            selected={this.selectedDispatch}
            advancedCount={this.advancedCount}
            onChange={selected => {
              this.selectedDispatch = [];
              this.selectedDispatch = selected;
              console.log("selected",selected);
            }}
          />
        </div>
      </a-modal>
    );
  },
};
