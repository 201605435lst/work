import './style/index.less';
import SmConstructionBaseStandard from '../sm-construction-base-standard/SmConstructionBaseStandard';


export default {
  name: 'SmConstructionBaseStandardSelectModal', // 选择框的modal
  model: { // 把visible 属性暴露出去,方便表单验证
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    placeholder: { type: String, default: '请选择工序指引' },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
  },
  data() {
    return {
      selectedStandards: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedStandards;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedStandards = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedStandards = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedStandards);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedStandards = [];
    },
  },

  render() {
    return (
      <a-modal
        width={1000}
        title="工序规范指引选择框"
        class="sm-basic-selectedStandards-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'bank'} />
                <span class="selected-name"> {item ? item.name : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedStandards = this.selectedStandards.filter(_item => _item.id !== item.id);
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
        <div>
          <SmConstructionBaseStandard
            axios={this.axios}
            multiple={this.multiple}
            showOperator={false}
            showSelectRow={true}
            selected={this.selectedStandards}
            onChange={selected => {
              this.selectedStandards = [];
              this.selectedStandards = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
