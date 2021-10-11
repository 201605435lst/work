import 'ant-design-vue/lib/dropdown/style/css';

export default {
  props: {
    isDelete: { type: Boolean, default: false },
    barInfo: { type: Object, default: () => { } },

  },
  data() {
    return {

    };
  },
  methods: {
    close() {
      this.$emit('close');
    },

    confirmDelete() {
      this.$emit('confirmDelete');
    },
    deleteTask() {
      this.$emit('deleteTask');
    },
    inserChildTask() {
      this.$emit('inserChildTask', this.barInfo);
    },

    insertTask() {
      this.$emit('insertTask');
    },

    /**
     * 任务向右边移动
     * barInfo 任务条数据
     */
    moveRightTask() {
      this.$emit('moveRightTask', this.barInfo);
    },

    /**
     * 任务向左移动
     * barInfo 任务条数据
     */
    moveLeftTask() {
      this.$emit('moveLeftTask', this.barInfo);
    },
  },
  render() {
    return <div class='dropdown__u1N0 slide-enter-done' data-role='popup' x-placement='top-start'>
      <div class='dropdown-container__1UDy'>
        <div class='popup-menu__3OeO popup__2xg9'>
          <div class='popup-menu-header__1u4j popup-head__291F'>
            任务菜单
            <div>
              <span onClick={this.close} class='icon__YImk icon dls-icon icon-remove popup-menu-close__102T' data-role='icon' />
            </div>
          </div>
          <div class='separator-full__2UhN' data-role='separator'>

          </div>
          <div class='popup-menu-view__fI0k popup-body__37yU'>
            {this.isDelete === false ?
              <ul class='menu__21sM main-view__KEvn' data-role='menu'>
                <li class='menu-item__dkeQ' data-role='menu-item'>
                  <span class='text'>点击进入任务详情</span>
                </li>
                {this.barInfo._index > 0 ?
                  <li
                    onClick={this.moveRightTask}
                    class='menu-item__dkeQ'
                    data-role='menu-item'>
                    <span class='text'>向右缩进一级</span>
                    <span class='hotkey'><b>tab</b></span>
                  </li> : undefined
                }

                {this.barInfo._depth > 0 ?
                  <li
                    onClick={this.moveLeftTask}
                    class='menu-item__dkeQ'
                    data-role='menu-item'
                  >
                    <span class='text'>向左提升一级</span>
                    <span class='hotkey'><b>shift</b>+<b>tab</b></span>
                  </li> : undefined
                }
                {this.barInfo._depth > 0 &&
                  <li
                    class='menu-item__dkeQ'
                    data-role='menu-item'
                    onClick={this.insertTask}
                  >
                    <span class='text'>插入新任务</span>
                    <span class='hotkey'><b>enter</b></span>
                  </li>
                }

                <li
                  onClick={this.inserChildTask}
                  class='menu-item__dkeQ'
                  data-role='menu-item'
                >
                  <span class='text'>插入子任务</span>
                </li>
                <a class='popup-menu-link__wpP-'>
                  <li class='menu-item__dkeQ' data-role='menu-item'>
                    <span class='text'>切换任务类型</span>
                  </li>
                </a>
                <li class='separator__3vM5' data-role='separator' />
                <a class='popup-menu-link__wpP-'>
                  <li onClick={this.deleteTask} class='menu-item__dkeQ danger__2yG_' data-role='menu-item'>
                    <span class='text'>移到回收站</span>
                  </li>
                </a>
              </ul>
              :
              <div class='archive__3_58'>
                <p>你确定要把 1 个任务移到回收站吗？</p>
                <button
                  onClick={this.confirmDelete}
                  class='btn-type-primary__2LYT btn-danger__1BqI btn-block__1VgC'
                  type='button'
                  data-role='button'
                >
                  <span>移到回收站</span>
                </button>
              </div>
            }


          </div>
        </div>
      </div>
    </div>;

  },
};
