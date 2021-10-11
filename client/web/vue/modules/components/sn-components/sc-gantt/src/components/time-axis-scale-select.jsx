import Clickoutside from '../util/clickoutside';
export default {
  // 这个组件是 日期 选择框 日 周 月 季 年 视图
  directives: { Clickoutside },
  props: {
    viewWidth: {
      type: Number,
      default: 100,
    },
    viewTypeList: {
      type: Array,
      default() {
        return [];
      },

    },
    defaultValue: {
      type: Object,
      default() {
        return {};
      },

    },
    value: {
      type: Object,
      default() {
        return {};
      },

    },
    guestureGrantBodyMove: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isHide: true,
    };
  },
  computed: {
    left() {
      return this.viewWidth - 92;
    },
    top() {
      return 60;
    },

  },
  methods: {
    hide() {
      this.isHide = true;
    },
    show() {
      this.isHide = !this.isHide;
    },
    isSelected(key) {
      return this.value.key === key;
    },
    select(item) {
      this.isHide = true;
      this.$emit('change', item);
    },
    clickKey(e){
      let selectItem = this.viewTypeList[e.key];
      this.$emit('change', selectItem);
    },

  },
  render() {
    return <div class='time-axis-scale-select__3fTI trigger__3NoY'>
      <a-dropdown>
        <a-menu slot='overlay' onClick={this.clickKey}>
          {this.viewTypeList.map((item,key)=>{
            return <a-menu-item key={key}>{item.label}</a-menu-item>;
          })}
        </a-menu>
        <a-button size={'small'} style='margin-left: 8px'>{this.value.label}视图<a-icon type="caret-down" /></a-button>
      </a-dropdown>
    </div>;
  },
};
