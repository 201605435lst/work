import './style/index.less';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import { requestIsSuccess } from '../../_utils/utils';
import { dropdownStyle } from '../../_utils/config';
import SmConstructionBaseSubItemTree from '../../sm-construction-base/sm-construction-base-sub-item-select-tree/SmConstructionBaseSubItemTree';

let apiSubItem = new ApiSubItem();

export default {
  name: 'SmConstructionBaseSubItemSelectTreeModal', // 分部分项-选择树-modal
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
      queryParams: {
        subItemId: undefined, // 根据选择工程的id搜索
        maxResultCount: 999,// 每页数量
        pageIndex: 1,// 当前页1 这个在params 里面 也过滤掉了,放这里方便复制~
        totalCount: 0,// 总数 这个在params 里面 也过滤掉了,放这里方便复制~
      },
      selectedSubItems: [],
      iVisible: false,
      projectList: [],// 项目列表
      subItemList: [],// 工程列表
      subItemContentId: undefined, // contentId
    };
  },

  computed: {
    tags() {
      return this.selectedSubItems;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedSubItems = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedSubItems = this.value;
      },
      immediate: true,
    },
  },

  async created() {

    this.initAxios();
    this.getSubItemList();
  },

  methods: {
    initAxios() {
      apiSubItem = new ApiSubItem(this.axios);
    },
    
    // 获取工程 列表
    async getSubItemList() {
      let res = await apiSubItem.getList(this.queryParams);
      if (requestIsSuccess(res) && res.data) {
        this.subItemList = res.data.items.filter(x => x.isDrawUp);
        if (this.multiple) {
          this.selectedSubItems = [];
          this.$refs.SmConstructionBaseDrawSubItem.selectContentIds = [];
          this.$refs.SmConstructionBaseDrawSubItem.iSelected = [];
        }
      }
    },
    onOk() {
      this.$emit('ok', this.selectedSubItems);
      this.onClose();
    },
    onReset() {
      this.queryParams.pageIndex = 1;
      this.queryParams.subItemId = undefined;
      this.subItemList = [];
      this.subItemContentId = undefined;
    },
    onClose() {
      this.onReset();
      this.$emit('change', false);
      this.selectedSubItems = [];
    },
  },

  render() {
    return (
      <a-modal
        width={1200}
        title="分部分项选择"
        class="sm-basic-selectedSubItems-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected" style="display:none;">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'bank'} />
                <span class="selected-name"> {item ? item.name : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedSubItems = this.selectedSubItems.filter(_item => _item.id !== item.id);
                    console.log("this.selectedSubItems ", this.selectedSubItems);
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

          {/* 操作区 */}
          <sc-table-operator
            size="small"
            lg={6}
            gutter={6}
            onSearch={() => {
              this.getSubItemList();
            }}
            onReset={this.onReset}
          >
            <a-form-item label='工程'>
              <a-select
                size='small'
                showSearch={true}
                dropdownStyle={dropdownStyle}
                optionFilterProp="children"
                dropdownMatchSelectWidth
                placeholder={'请选择工程名称'}
                value={this.queryParams.subItemId}
                onChange={(val) => {
                  this.queryParams.subItemId = val;
                  this.subItemContentId = val;
                  this.getSubItemList();
                }}>
                {this.subItemList.map(x => (<a-select-option title={x.name} value={x.subItemContent.id}>{x.name}</a-select-option>))}
              </a-select>
            </a-form-item>
          </sc-table-operator>
          <SmConstructionBaseSubItemTree
            ref="SmConstructionBaseDrawSubItem"
            subItemContentId={this.subItemContentId}
            axios={this.axios}
            isSimple={true}
            multiple={this.multiple}
            showOperator={false}
            showSelectRow={true}
            selected={this.selectedSubItems}
            onChange={selected => {
              this.selectedSubItems = [];
              this.selectedSubItems = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
