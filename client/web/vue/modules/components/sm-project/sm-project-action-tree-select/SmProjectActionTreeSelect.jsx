import "./style/index";
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import { treeArrayItemAddProps } from '../../_utils/tree_array_tools';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
// import ApiBooksClassification from '../../sm-api/sm-project/BooksClassification';

// let apiBooksClassification = new ApiBooksClassification();



export default {
  name: 'SmProjectActionTreeSelect',
  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String], default: undefined }, //返回值
    disabled: { type: Boolean, default: false }, //是否禁用
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
    placeholder: { type: String, default: '请选择' }, //是否显示搜索
    permissions: { type: Array, default: () => [] },
    api: { type: Object, default: null },
  },
  data() {
    return {
      loading: false,
      name: null,
      open: false,//是否展开下拉菜单
      isAddEdit: false,
      iValue: undefined,
      iApi: null,//接口
      dataSource: [],
      treeData: [],
      isEdit: false,//是否编辑
      editId: null, // 编辑时的id
    };
  },
  computed: {

  },
  watch: {
    value: {
      handler: function (nVal, oVal) {
        this.iValue = nVal;
      },
      immediate: true,
    },
    dataSource: {
      handler(nVal, oVal) {
        this.treeData = nVal || [];
      },
      immediate: true,
    },
    api: {
      handler: function (val, oldVal) {
        if (this.api) {
          this.iApi = this.api;
          this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
  },
  created() {
    // this.initAxios();
    // this.refresh();
  },
  methods: {
    // initAxios() {
    //   apiBooksClassification = new ApiBooksClassification(this.axios);

    // },
    async refresh() {
      let response = await this.iApi.getList();
      if (requestIsSuccess(response) && response.data && response.data.items) {
        let _dataSource = treeArrayItemAddProps(response.data.items, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.dataSource = _dataSource;
      }
    },
    async save() {
      let response = null;
      let data = {
        name: this.name,
      };
      if (!this.isEdit) {
        response = await this.iApi.create(data);
      } else {
        response = await this.iApi.update({ id: this.editId, ...data });
      }
      if (requestIsSuccess(response)) {
        this.$message.success('操作成功');

        this.refresh();
        this.isEdit = false;
        this.isAddEdit = false;
        this.name=null;
        this.editId=null;

      }
    },
  },
  render() {
    return (
      <div class="sm-project-action-tree-select" ref="root">
        <a-select
          disabled={this.disabled}
          style="width: 100%"
          allowClear={this.allowClear}
          placeholder={this.placeholder}
          dropdownMatchSelectWidth
          ref="groupSelect"
          open={this.open}
          value={this.iValue}
          onFocus={() => {
            this.open = true;
          }}
          onBlur={() => {
            if (!this.isAddEdit) {
              this.open = false;
            }
          }}
          onSelect={() => {
            if (!this.inputState) {
              this.open = false;
            }
          }}
          onChange={value => {
            console.log(value);
            this.open = false;
            this.iValue = value;
            this.$emit('change', value);
            this.$refs.groupSelect.blur();
          }}
          getPopupContainer={() => this.$refs.root}
          notFoundContent="暂无数据"
          {...{
            scopedSlots: {
              dropdownRender: (vnode, props) => {
                return (
                  <div class="tree-select-content">
                    {vnode}
                    <a-divider style={{ margin: '4px 0' }} />
                    <div class="tree-select-action">
                      {this.isAddEdit ?
                        <div class="tree-select-add-edit">
                          <a-input
                            class="add-edit-input"
                            value={this.name}
                            placeholder="类型或名称"
                            size="small"
                            onChange={e => (this.name = e.target.value)}
                          />

                          <div class="add-edit-button">
                            <a-button
                              class="add-edit-button-save"
                              type="primary"
                              size="small"
                              onClick={() => this.save()}>
                              保存
                            </a-button>
                            <a-button
                              size="small"
                              onClick={() => {
                                this.isAddEdit = false;
                                this.isEdit = false;
                                this.$refs.groupSelect.focus();
                              }}
                            >
                              取消
                            </a-button>
                          </div>
                        </div>
                        : <div class="tree-select-icon">
                          <a-icon type="plus"
                            onClick={e => {
                              this.isAddEdit = true;//可以添加或者编辑
                              this.open = true;//模态框打开
                              this.isEdit = false;
                            }}
                          />
                        </div>
                      }
                    </div>
                  </div>
                );
              },
            },
          }}
        >
          {this.dataSource.map(item => {
            let result = item && item.title && `${item.title.length > 10 ? `${item.title.substring(0, 10)}...` : item.title}`;
            return (
              <a-select-option value={item.id}>
                <div class="items-title">
                  <span title={item.title}>{result}</span>
                  <div class="item-edit">
                    <span class="item-action">
                      <a-icon
                        type="edit"
                        onClick={e => {
                          e.stopPropagation();
                          this.isAddEdit = true;//可以添加或者编辑
                          this.open = true;//模态框打开
                          this.name = item.name;
                          this.editId=item.id;//编辑的id
                          this.isEdit = true;
                        }}
                      />
                    </span>
                  </div>
                </div>
              </a-select-option>

            );
          })}
        </a-select>
      </div>
    );
  },
};
