import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiProject from '../../sm-api/sm-project/Project';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionBaseSubItemModal from './SmConstructionBaseSubItemModal';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import SubItemContentTableModal from './SubItemContentTableModal';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';

let apiSubItem = new ApiSubItem();
let apiSubItemContent = new ApiSubItemContent();
let apiProject = new ApiProject();

export default {
  name: 'SmConstructionBaseSubItem',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,       // 每页数量
        pageIndex: 1,                                      // 当前页1 这个在params 里面 也过滤掉了,放这里方便复制~
        totalCount: 0,                                      // 总数 这个在params 里面 也过滤掉了,放这里方便复制~
      },
      list: [],    // table 数据源
      projectList: [],    // 项目列表
      loading: false, // table 是否处于加载状态
    };
  },
  computed: {
    columns() {
      return [
        { title: '序号', dataIndex: 'id', scopedSlots: { customRender: 'id' }, width: 100 },
        { title: '工程名称', dataIndex: 'name' },
        { title: '创建人', dataIndex: 'creatorName' },
        { title: '创建时间', dataIndex: 'createTime' },
        { title: '是否编制', dataIndex: 'isDrawUp', scopedSlots: { customRender: 'isDrawUp' } },
        { title: '操作', width: 200 , scopedSlots: { customRender: 'operations' } },
      ];
    },
  },
  async created() {
    this.initAxios();
    await this.refresh();
    await this.getProjectList();
  },
  methods: {
    initAxios() {
      apiSubItem = new ApiSubItem(this.axios);
      apiProject = new ApiProject(this.axios);
      apiSubItemContent = new ApiSubItemContent(this.axios);
    },
    async getProjectList() {
      let res = await apiProject.getList({ maxResultCount: 999 });
      if (requestIsSuccess(res) && res.data) {
        this.projectList = res.data.items;
      }
    },
    async refresh() {
      this.loading = true;
      let res = await apiSubItem.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // console.log(res.data.items);
        this.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionBaseSubItemModal.add();
    },
    // 编制 冒泡冒出去,调用 他的人使用 路由 跳转
    async config(record) {
      // 编制的时候 检查下 有没有被编制过 ,没有被编制过的话给个初始的 content
      if (!record.isDrawUp) {
        let res = await apiSubItemContent.initContent(record.id);
        if (requestIsSuccess(res)) {
          await this.refresh();
          let contentRes = await apiSubItem.get(record.id);
          if (requestIsSuccess(contentRes)) {
            // console.log(contentRes.data.subItemContent.id);
            this.$emit("edit", contentRes.data.subItemContent.id);
          }
        }
      } else {
        console.log(record.subItemContent.id);
        this.$emit("edit", record.subItemContent.id);
      }

      // // 编制的时候 检查下 有没有被编制过 ,没有被编制过的话给个初始的 content
      // if (!record.isDrawUp) {
      //   let res = await apiSubItemContent.initContent(record.id);
      //   if (requestIsSuccess(res)) {
      //     // 路由跳转 到 编制界面
      //     this.$router.push("/components/sm-construction-base-draw-sub-item-cn/"+record.id);
      //     // await this.refresh();
      //     // let contentRes = await apiSubItem.get(record.id);
      //     // if (requestIsSuccess(contentRes)) {
      //     //   this.$refs.SubItemContentTableModal.config(contentRes.data);
      //     // }
      //   }
      // }else{
      //   // this.$refs.SubItemContentTableModal.config(record);
      //   // 路由跳转 到 编制界面
      //   this.$router.push("/components/sm-construction-base-draw-sub-item-cn/"+record.id);
      // }
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
            const response = await apiSubItem.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
    // 查看(打开添加模态框)
    view(record) {
      this.$refs.SubItemContentTableModal.view(record);
    },

    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
  },
  render() {
    return (
      <div>
        {/* 操作区 */}
        
        <a-button style="margin:20px 0;" type='primary' icon='plus' onClick={() => this.add()}>
          添加
        </a-button>
       
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          pagination={false}
          {...{
            scopedSlots: {
              /* 这个id 要和 columns[] 的  scopedSlots: { customRender: 'id' } 对应 */
              id: (text, record, index) => {
                let result = `${index + 1 + this.totalCount * (this.queryParams.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              isDrawUp: (text, record, index) => {
                return (<span>{text ? '是' : '否'}</span>);
              },
              operations: (record) => {
                return (
                  <div>
                    {record.isDrawUp ? (
                      <div style='display:inline' onClick={() => this.view(record)}><a>查看</a></div>) : undefined}
                    <div style='display:inline;margin-left:10px' onClick={() => this.config(record)}><a>编制</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
                  </div>
                );
              },
            },
          }}
        />

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.queryParams.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加模板*/}
        <SmConstructionBaseSubItemModal
          ref='SmConstructionBaseSubItemModal'
          axios={this.axios}
          projectList={this.projectList}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
        {/*添加模板*/}
        <SubItemContentTableModal
          ref='SubItemContentTableModal'
          axios={this.axios}
          projectList={this.projectList}
          onSuccess={async () => {
            await this.refresh();
          }}
        />

      </div>
    );
  },
};
