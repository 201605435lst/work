import './style/index.less';
import SmConstructionBaseSection from '../sm-construction-base-section/SmConstructionBaseSection';


export default {
  name: 'SmConstructionBaseSectionSelectModal', // 选择框的modal
  model: { // 把visible 属性暴露出去,方便表单验证
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, // 编辑/查看模式
    placeholder: { type: String, default: '请点击选择施工区段' },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
  },
  data() {
    return {
      selectedSections: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedSections;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedSections = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedSections = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedSections);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedSections = [];
    },
  },

  render() {
    return (
      <a-modal
        width={1000}
        title="施工区段选择"
        class="sm-basic-selectedSections-modal"
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
                    this.selectedSections = this.selectedSections.filter(_item => _item.id !== item.id);
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
          <SmConstructionBaseSection
            axios={this.axios}
            multiple={this.multiple}
            showOperator={false}
            showSelectRow={true}
            selected={this.selectedSections}
            onChange={selected => {
              this.selectedSections = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
