
import './style';
import ApiEntity from '../../sm-api/sm-namespace/Entity';

let apiEntity = new ApiEntity();

// 公共选择模
export default {
  name: 'CommonSelectModal',
  model: { // 把visible 属性暴露出去,方便表单验证
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, // 编辑/查看模式
    title:{type:String, default: '选择总体计划'}, // modal 标题
    placeholder: { type: String, default: '请点击选择总体计划' },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
    modalWidth:{type:Number, default: 1200} , // modal 宽度 默认 800
  },
  data() {
    return {
      iVisible: false,
    };
  },
  computed: {

  },
  watch: {

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
  },
  async created() {

  },
  methods: {
    onOk() {
      this.$emit('ok');
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
    },
  },
  render() {
    return (
      <a-modal
        width={this.modalWidth}
        title={this.title}
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        {/*input div 插槽*/}
        {this.$slots.modalDiv}
        {/*默认插槽*/}
        {this.$slots.default}
      </a-modal>
    );
  },
};
