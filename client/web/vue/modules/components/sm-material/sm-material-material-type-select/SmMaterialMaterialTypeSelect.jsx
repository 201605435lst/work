import './style';
import { requestIsSuccess, vIf, vP, getCategory } from '../../_utils/utils';
import ApiMaterialType from '../../sm-api/sm-material/MaterialType';
import { Category } from '../../_utils/enum';
let apiMaterialType = new ApiMaterialType();

export default {
  name: 'SmMaterialMaterialTypeSelect',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    disabled: { type: Boolean, default: false }, //是否禁用
    value: { type: [Array, String], default: undefined }, //返回值
    allowClear: { type: Boolean, default: true }, //是否清除
    placeholder: { type: String, default: '请选择' }, //是否显示搜索
    search: { type: Boolean, default: false }, //是否是条件查询状态，默认false，即有添加按钮
  },
  data() {
    return {
      open: false, // select 组件的打开属性
      inputState: false, // 输入框的状态，用于新增内容
      iValue: undefined,
      inputValue: '', // 输入框中的值
      selectValue: null, // 选择框中的值
      selectItems: [],
      isEdit: false, // 是否编辑
      editId: null, // 编辑状态的id
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function(n, o) {
        this.iValue = n;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.getData();
  },
  methods: {
    initAxios() {
      apiMaterialType = new ApiMaterialType(this.axios);
    },
    async getData() {
      let response = await apiMaterialType.getList();
      if (requestIsSuccess(response)) {
        this.selectItems = response.data.items;
      }
    },
    // 保存类别信息
    async saveTag() {
      if (this.isEdit) {
        this.editGroup(this.editId);
      } else {
        // 保存数据
        let param = {
          name: this.inputValue,
          category: this.selectValue,
        };
        let response = await apiMaterialType.create(param);
        if (requestIsSuccess(response)) {
          if (response.data) {
            this.getData();
            this.inputState = false;
          }
        }
      }
    },
    // 编辑类别信息
    async editGroup(id) {
      let param = {
        id,
        name: this.inputValue,
        category: getCategory(this.selectValue),
      };
      let response = await apiMaterialType.update(param);
      if (requestIsSuccess(response)) {
        if (response.data) {
          //刷新数据
          this.getData();
          this.inputState = false;
        }
      }
    },
  },
  render() {
    //类别的类别
    let categoryOptions = [];
    for (let item in Category) {
      categoryOptions.push(
        <a-select-option key={Category[item]}>
          {getCategory(Category[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-material-material-type-select" ref="root">
        <a-select
          disabled={this.disabled}
          ref="groupSelect"
          open={this.open}
          allowClear={this.allowClear}
          placeholder={this.placeholder}
          style="width: 100%"
          dropdownMatchSelectWidth
          notFoundContent="暂无数据"
          onFocus={() => {
            this.open = true;
          }}
          onBlur={() => {
            if (!this.inputState) {
              this.open = false;
            }
          }}
          value={this.iValue}
          onChange={v => {
            this.open = false;
            this.iValue = v;
            this.$emit('change', v);
            this.$refs.groupSelect.blur();
          }}
          onSelect={() => {
            if (!this.inputState) {
              this.open = false;
            }
          }}
          getPopupContainer={() => this.$refs.root}
          // 下拉扩展
          dropdownRender={(vnode, props) => {
            return (
              <div>
                {vnode}
                {this.search ? undefined : 
                  <div>
                    <a-divider style={{ margin: '4px 0' }} />
                    <div style={{ padding: '8px', cursor: 'pointer', textAlign: 'center' }}>
                      {this.inputState ? (
                        <div class="f-tag-add">
                          {/* 新增标签 */}
                          <a-input
                            value={this.inputValue}
                            placeholder="材料类别..."
                            size="small"
                            style="width: 60%"
                            onChange={e => (this.inputValue = e.target.value)}
                          />
                          <a-select
                            placeholder={'所属类别'}
                            value={this.selectValue}
                            size="small"
                            style="width: 40%"
                            onChange={value => this.selectValue = value}
                          >
                            {categoryOptions}
                          </a-select>
                          <a-button type="primary" size="small" onClick={() => this.saveTag()}>
                            保存
                          </a-button>
                          <a-button
                            size="small"
                            onClick={() => {
                              this.inputState = false;
                              // 点击取消时下拉框需要重新获取焦点
                              this.$refs.groupSelect.focus();
                              this.isEdit = false;
                              this.editId = null;
                            }}
                          >
                            {' '}
                            取消
                          </a-button>
                        </div>
                      ) : (
                        <a-icon
                          type="plus"
                          onClick={() => {
                            this.inputState = true;
                            this.open = true;
                            this.isEdit = false;
                          }}
                        />
                      )}
                    </div>  
                  </div>}
              </div>
            );
          }}
        >
          {this.selectItems.map(a => {
            let result = `${a.name.length > 10 ? `${a.name.substring(0, 10)}...` : a.name}`;
            return (
              <a-select-option value={a.id}>
                <div class="f-select-option-item">
                  <span class="item-title" title={a.name}>{result}</span>
                  {/* 编辑操作 */}
                  {this.search ? undefined : 
                    <div class="f-select-edit">
                      <a-icon
                        class="f-select-edit-icon"
                        type="edit"
                        onClick={e => {
                          e.stopPropagation();
                          // 准备编辑
                          this.inputState = true;
                          this.isEdit = true;
                          this.inputValue = a.name;
                          this.selectValue = getCategory(a.category);
                          this.editId = a.id;
                        }}
                      />
                    </div>}
                </div>
              </a-select-option>
            );
          })}
        </a-select>
      </div>
    );
  },
};
