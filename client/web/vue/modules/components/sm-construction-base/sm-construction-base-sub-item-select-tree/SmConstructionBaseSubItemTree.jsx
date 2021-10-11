
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import { tips as tipsConfig } from '../../_utils/config';
import ApiProject from '../../sm-api/sm-project/Project';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import { MoveTypeEnum, NodeTypeEnum } from '../sm-construction-base-sub-item/NodeTypeEnum';

let apiSubItemContent = new ApiSubItemContent();
let apiProject = new ApiProject();
let apiSubItem = new ApiSubItem();
export default {
  name: 'SmConstructionBaseSubItemTree', // 分布分项-树
  props: {
    subItemContentId: { type: String, default: '39fcdc99-0f60-6476-fbfe-a20ef05b4374' },// 分部分项 id
    // subItemContentId: { type: String, default: undefined },// 分部分项 id
    axios: { type: Function, default: null }, // axios
    permissions: { type: Array, default: () => [] }, // 权限
    bordered: { type: Boolean, default: true }, // 边框
    showOperator: { type: Boolean, default: true }, // 是否显示操作栏()
    showSelectRow: { type: Boolean, default: false }, // 是否显示选择栏
    multiple: { type: Boolean, default: false },//是否多选
    selected: { type: Array, default: () => [] }, //待选计划所选工序规范集合
  },
  data() {
    return {
      list: [],// table 数据源 列表元素只有 一个 ,方便 table装
      loading: false,// table 是否处于加载状态
      selectedContentIds: [], // 选择的 content Ids
      iSelected: [],//已选 content 实体
      iValue: [],
    };
  },
  computed: {
    columns() {
      let baseColumns = [
        { title: '节点名称', dataIndex: 'name' },
        {
          title: '节点类型', dataIndex: 'nodeTypeStr', width: 200, customRender: (text, record, index) => {
            return record.nodeType === NodeTypeEnum.All ? undefined : (<span>{text}</span>);
          },
        },
      ];
      return this.showOperator ?
        [
          ...baseColumns,
          {
            title: '操作', customRender: (record) => {
              return (
                <div>
                  {record.nodeType === NodeTypeEnum.SubItem ? undefined :
                    (<div style='display:inline;margin-right:10px' onClick={() => this.add(record)}><a>添加</a></div>)
                  }
                  <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                  {record.nodeType === NodeTypeEnum.All ? undefined : (
                    <div style='display:inline;'>
                      <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
                      <div style='display:inline;margin-left:10px' onClick={() => this.move(record, MoveTypeEnum.Up)}>
                        <a>上移</a></div>
                      <div style='display:inline;margin-left:10px' onClick={() => this.move(record, MoveTypeEnum.Down)}>
                        <a>下移</a></div>
                    </div>
                  )}
                </div>
              );
            },
          },
        ] :
        baseColumns;
    },
  },
  watch:{
    // 监听prop的值改变(由调用的父组件改变),网络请求来刷新table
    subItemContentId:{
      handler: function (val, oldVal) {
        this.refresh();
      },
      immediate: false,
    },
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.selectedContentIds = value.map(item => item.id);
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
      apiProject = new ApiProject(this.axios);
      apiSubItem = new ApiSubItem(this.axios);
    },


    add(record) {
      this.$refs.SubItemContentModal.add(record);
    },
    edit(record) {
      this.$refs.SubItemContentModal.edit(record);
    },

    async move(record, moveType) {
      let res = await apiSubItemContent.move(record.id, moveType);
      // console.log('res', res);
      if (requestIsSuccess(res)) {
        await this.refresh();
      }
    },

    async refresh() {
      this.loading = true;
      this.list = [];
      let res= await apiSubItemContent.getSingleTree(this.subItemContentId);
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.name === null ? [] : [res.data];
      }
      this.loading = false;
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          // 删除角色业务逻辑
          return new Promise(async (resolve, reject) => {
            const response = await apiSubItemContent.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 找到所有 ids
        let selectIds = selectedRows.filter(x=>x.id);
        this.iSelected = selectedRows;
      } else {
        this.iSelected = selectedRows;
      }
      this.$emit('change', this.iSelected);
    },

  },
  render() {
    return (
      <div>
        {this.list.length ===0?"没有数据":(
          <a-tree-select
            value={this.selectedContentIds}
            style='width: 100%'
            allowClear={true}
            showSearch={true}
            treeData={this.list}
            treeNodeFilterProp='title'
            treeDefaultExpandAll={true}
            replaceFields={{ title: 'name', key: 'id' ,value:'id',children:'children'}}
            treeCheckable={this.multiple}
            showCheckedStrategy='SHOW_PARENT'
            searchPlaceholder="请选择分部分项"
            onChange={(value, label, extra) => {
              // value: ["guid","guid"]
              // lable:  ["嘟嘟工程","啦啦工程"]
              // 把这两个拼成 新的数组 [{id:xxx,name:xxx},{id:xxx,name:xxx}]
              if (this.multiple) {

                let zip = value.map((item, index, arr) => ({ id: item, name: label[index] }));
                this.selectedContentIds = value;
                this.updateSelected(zip);
              }else {
                let zip = { id: value, name: label};
                this.selectedContentIds = [ value ];
                this.updateSelected([zip]);
              }
            }
            }
          />
        )}



        {/*保证有数据的时候再渲染table 不然 defaultExpandAllRows 默认展开所有 这个 没有用……  */}
        {/*{this.list && this.list.length ?*/}
        {/*  <a-table*/}
        {/*    dataSource={this.list}*/}
        {/*    rowKey={record => record.id}*/}
        {/*    columns={this.columns}*/}
        {/*    defaultExpandAllRows={true}*/}
        {/*    loading={this.loading}*/}
        {/*    bordered={true}*/}
        {/*    pagination={false}*/}
        {/*    rowSelection={this.showSelectRow?{*/}
        {/*      type: this.multiple ? 'checkbox' : 'radio',*/}
        {/*      columnWidth: 30,*/}
        {/*      selectedRowKeys: this.selectContentIds, // 绑定 已选择的数据*/}
        {/*      onChange: (selectedRowKeys, selectedRows) => {*/}
        {/*        this.selectContentIds = selectedRowKeys;*/}
        {/*        this.updateSelected(selectedRows);*/}
        {/*      },*/}
        {/*    }:undefined}*/}
        {/*  />*/}
        {/*  : '暂无数据'*/}
        {/*}*/}
      </div>
    );
  },
};
