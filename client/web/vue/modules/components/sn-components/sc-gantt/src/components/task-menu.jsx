
// import aDropdown from 'ant-design-vue/lib/dropdown';
import 'ant-design-vue/lib/dropdown/style/css';


export default {
  // 这个组件是左边的任务菜单 右键 弹出来的 按钮列表
  components: {
    // aDropdown,
  },
  props: {
    barInfo: {
      type: Object,
      default: () => {
      },
    },
  },
  data() {
    return {
      visible: false,
      isDelete: false,
    };
  },
  watch: {
    visible(val) {
      if (!val) {
        this.isDelete = false;
      }
    },
  },
  mounted() {

  },
  methods: {
    close() {
      this.visible = false;
    },
    deleteTask() {
      this.isDelete = true;
    },
    insertTask() {
      this.$emit('insertTask');
      this.visible = false;
    },
    inserChildTask() {
      this.$emit('inserChildTask', this.barInfo);
      this.visible = false;
    },
    /**
     * 任务向右边移动
     * barInfo 任务条数据
     */
    moveRightTask() {
      // 必须有上一个兄弟节点
      let index = this.barInfo._index;
      if (index <= 0) return;

      this.$emit('moveRightTask', this.barInfo);
      this.visible = false;
    },
    /**
     * 任务向左移动
     * barInfo 任务条数据
     */
    moveLeftTask() {
      let depth = this.barInfo._depth;
      if (depth <= 0) return;

      this.$emit('moveLeftTask', this.barInfo);
      this.visible = false;
    },
    confirmDelete() {
      this.$emit('deleteTask', () => {
        this.visible = false;
        this.isDelete = false;
      });
    },
  },
  render() {
    return <div class={` menu-trigger__2YZ4 ${this.visible?'opened':''} `} onKeydown={() => {
      console.log('按键');
    }}>
      <a-dropdown  trigger={ ['click'] }>
        <a class='ant-dropdown-link' onClick={e => e.preventDefault()}><a-icon type='dash' /></a>
        <a-menu slot='overlay'>
          <a-menu-item key="1"> <a-icon type="user" />查看任务详情 </a-menu-item>
          <a-menu-divider />
          <a-menu-item key="2"> <a-icon type="user" />升级</a-menu-item>
          <a-menu-divider />
          <a-menu-item key="3"> <a-icon type="user" />降级</a-menu-item>
          <a-menu-divider />
          <a-menu-item key="4"> <a-icon type="user" />插入新任务</a-menu-item>
          <a-menu-divider />
          <a-menu-item key="5"> <a-icon type="user" />编辑任务</a-menu-item>
          <a-menu-divider />
          <a-menu-item onClick={this.confirmDelete} key="6"> <a-icon type="user" />删除</a-menu-item>
        </a-menu>
      </a-dropdown>
    </div>;
  },
};
