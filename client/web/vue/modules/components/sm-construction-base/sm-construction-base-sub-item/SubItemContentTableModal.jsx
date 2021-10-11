import './style';
import { tips as tipsConfig } from '../../_utils/config';
import * as utils                                 from '../../_utils/utils';
import { ModalStatus }                            from '../../_utils/enum';
import ApiSubItemContent                          from '../../sm-api/sm-construction-base/ApiSubItemContent';
import { requestIsSuccess }                       from '../../_utils/utils';
import { MoveTypeEnum, NodeTypeEnum }             from './NodeTypeEnum';
import SubItemContentModal                        from './SubItemContentModal';

let apiSubItemContent = new ApiSubItemContent();


// 分部分项编制界面
export default {
  name: 'SubItemContentTableModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status : ModalStatus.Hide, // 模态框状态
      list   : [],               // table 数据源 列表元素只有 一个 ,方便 table装
      loading: false,            // table 是否处于加载状态
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return this.status === ModalStatus.View ?
        [
          { title: '节点名称', dataIndex: 'name' },
          {
            title: '节点类型', dataIndex: 'nodeTypeStr', width: 200, customRender: (text, record, index) => {
              return record.nodeType === NodeTypeEnum.All ? undefined : (<span>{text}</span>);
            },
          },
        ]
        :
        [
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
                  {record.nodeType === NodeTypeEnum.WorkSur ? undefined :
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
        ];
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
    add(record) {
      this.$refs.SubItemContentModal.add(record);
    },
    edit(record) {
      this.$refs.SubItemContentModal.edit(record);
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

    async move(record, moveType) {
      let res = await apiSubItemContent.move(record.id, moveType);
      // console.log('res', res);
      if (requestIsSuccess(res)) {
        await this.refresh();
      }
    },
    async refresh() {
      this.loading = true;
      let res      = await apiSubItemContent.getSingleTree(this.record.subItemContent.id);
      if (requestIsSuccess(res) && res.data) {
        this.list = [res.data];
        // console.log(this.list);
      }
      this.loading = false;
    },
    // 查看
    async view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.list = [];
      // 根据 content id 获取 content 树
      if (record.subItemContent != null) {
        await this.refresh();
      }
    },
    // 编辑
    async config(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.list = [];
      await this.refresh();
    },
  },
  render() {
    return (
      <a-modal
        title={`分部分项编制`}
        width={800}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        footer={null}
      >
        {/*保证有数据的时候再渲染table 不然 defaultExpandAllRows 默认展开所有 这个 没有用……  */}
        {this.list && this.list.length ?
          <a-table
            dataSource={this.list}
            rowKey={record => record.id}
            columns={this.columns}
            defaultExpandAllRows={true}
            loading={this.loading}
            bordered={true}
            pagination={false}
          />
          : '暂无数据'
        }
        {/*分部分项 添加/编辑 modal  */}
        <SubItemContentModal
          ref='SubItemContentModal'
          axios={this.axios}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
      </a-modal>
    );
  },
};
