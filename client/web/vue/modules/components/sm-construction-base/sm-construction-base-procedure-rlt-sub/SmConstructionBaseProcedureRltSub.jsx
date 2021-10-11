import './style';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import ApiProject from '../../sm-api/sm-project/Project';
import { requestIsSuccess } from '../../_utils/utils';
import { MoveTypeEnum, NodeTypeEnum } from '../sm-construction-base-sub-item/NodeTypeEnum';
import RltProcedureModal from './RltProcedureModal';
import SetRltProcedureModal from './SetRltProcedureModal';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';

let apiSubItemContent = new ApiSubItemContent();
let apiSubItem = new ApiSubItem();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查工程类型用
let apiProject = new ApiProject();
export default {
  name: 'SmConstructionBaseProcedureRltSub',
  props: {
    axios: { type: Function, default: null }, // axios
    border: { type: Boolean, default: true }, // 是否展示边框
  },
  data() {
    return {
      queryParams: {
        contentId: '00000000-0000-0000-0000-000000000000', // 详情id
        subItemId: '00000000-0000-0000-0000-000000000000', // 分部分项id
      },
      list: [],// table 数据源 列表元素只有 一个 ,方便 table装
      loading: false,// table 是否处于加载状态
      subItemList: [],// 项目列表
      procedureTypes: [],
    };
  },
  computed: {
    columns() {
      return [
        { title: '节点名称', dataIndex: 'name' },
        {
          title: '节点类型', dataIndex: 'nodeTypeStr', width: 200, customRender: (text, record, index) => {
            return record.nodeType === NodeTypeEnum.All ? undefined : (<span>{text}</span>);
          },
        },
        {
          title: '操作', customRender: (record) => {
            return (
              <div>
                {record.nodeType >= NodeTypeEnum.WorkSur ? (
                  <div style='display:inline;'>
                    <div style='display:inline;margin-right:10px' onClick={() => this.setRltComponent(record)}><a>关联构件</a></div>
                  </div>
                ) :
                  undefined
                }{record.nodeType === NodeTypeEnum.WorkSur ? (
                  <div style='display:inline;'>
                    <div style='display:inline;margin-right:10px' onClick={() => this.rltProcedure(record)}><a>关联工序</a></div>
                  </div>
                ) :
                  undefined
                }
                {record.nodeType === NodeTypeEnum.Procedure ? (
                  <div style='display:inline;'>
                    <div style='display:inline;margin-right:10px' onClick={() => this.setRltProcedure(record)}><a>工程量设置</a></div>
                    <div style='display:inline;' onClick={() => this.removeRltProcedure(record)}><a>删除</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Up)}>
                      <a>上移</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Down)}>
                      <a>下移</a></div>

                  </div>
                ) :
                  undefined
                }
              </div>
            );
          },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    await this.getSubItemList();
    await this.getProcedureTypes();
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
      apiSubItem = new ApiSubItem(this.axios);
      apiProject = new ApiProject(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },

    // 获取 工程类型 列表
    async getProcedureTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.ProjectType' });
      if (requestIsSuccess(res)) {
        this.procedureTypes = res.data;
      }
    },
    // 打开 关联工序 模态框
    rltProcedure(record) {
      this.$refs.RltProcedureModal.edit(record);
    },
    // 打开 关联构件 模态框
    setRltComponent(record) {
      console.log('打开关联构件Modal,还没有做');
    },
    // 设置 关联工序
    setRltProcedure(rltProcedure) {
      let id = rltProcedure.id;
      this.$refs.SetRltProcedureModal.edit(id);

    },
    // 获取工程(sub-item)列表
    async getSubItemList(val) {
      let res = await apiSubItem.getList({ maxResultCount: 999 });
      if (requestIsSuccess(res) && res.data) {
        // console.log(res);
        this.subItemList = res.data.items.filter(x => x.hasWorkSur); // 找已经被编制的 工程
        if (val === null || val === undefined) {
          this.queryParams.subItemId = this.subItemList[0].id;
          this.queryParams.contentId = this.subItemList[0].subItemContent.id;
        } else {
          this.queryParams.subItemId = this.subItemList.find(x => x.id === val).id;
          this.queryParams.contentId = this.subItemList.find(x => x.id === val).subItemContent.id;
        }
        await this.getSubItemContentTree();
      }
    },
    // 删除工序
    async removeRltProcedure(record) {
      let res = await apiSubItemContent.deleteRltProcedure(record.id);
      if (requestIsSuccess(res)) {
        this.list = [res.data];
        await this.getSubItemContentTree();
      }

    },
    // 获取 content 树
    async getSubItemContentTree() {
      this.loading = true;
      this.list = [];
      const res = await apiSubItemContent.getSingleTreeWithProcedure(this.queryParams.contentId);
      if (requestIsSuccess(res)) {
        this.list = [res.data];
        // console.log(this.list);
      }
      this.loading = false;
    },

    // 移动关联工序表
    async moveRltPro(record, moveType) {
      let res = await apiSubItemContent.moveRltProcedure(record.id, moveType);
      // console.log('res', res);
      if (requestIsSuccess(res)) {
        await this.getSubItemContentTree();
      }
    },

  },
  render() {
    return (
      <div>
        <sc-table-operator
          onSearch={() => {
            this.getSubItemContentTree();
          }}
          onReset={() => {
            this.getSubItemContentTree();
          }}
        >
          <a-form-item label='项目名称'>
            <a-select
              placeholder={'请选择项目名称'}
              value={this.queryParams.subItemId}
              onChange={(val) => {
                this.queryParams.subItemId = val;
                this.getSubItemList(val);
              }}>
              {/*<a-select-option value='00000000-0000-0000-0000-000000000000'>全部</a-select-option>*/}
              {this.subItemList.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>
        </sc-table-operator>

        {/*table 展示*/}
        {/*保证有数据的时候再渲染table 不然 defaultExpandAllRows 默认展开所有 这个 没有用……  */}
        {this.list && this.list.length ?
          <a-table
            dataSource={this.list}
            rowKey={record => record.id}
            columns={this.columns}
            defaultExpandAllRows={true}
            loading={this.loading}
            size="small"
            bordered={true}
            pagination={false}
          />
          : '暂无数据'
        }
        {/*关联工序Modal*/}
        <RltProcedureModal
          ref='RltProcedureModal'
          axios={this.axios}
          onSuccess={ () => {
            console.log('success');
            this.getSubItemContentTree();
          }}
        />
        {/*设置关联工序(工程量设置)Modal*/}
        <SetRltProcedureModal
          ref='SetRltProcedureModal'
          axios={this.axios}
          procedureTypes={this.procedureTypes}
          onSuccess={ () => {
            console.log('success');
            this.getSubItemContentTree();
          }}
        />
      </div>
    );
  },
};
