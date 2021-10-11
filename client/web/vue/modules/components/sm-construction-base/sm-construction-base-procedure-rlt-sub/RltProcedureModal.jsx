import './style';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import ApiProject from '../../sm-api/sm-project/Project';
import { requestIsSuccess } from '../../_utils/utils';
import { MoveTypeEnum, NodeTypeEnum } from '../sm-construction-base-sub-item/NodeTypeEnum';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';

let apiSubItemContent = new ApiSubItemContent();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查工程类型用
let apiSubItem = new ApiSubItem();
let apiProject = new ApiProject();
let apiProcedure = new ApiProcedure();


export default {
  name: 'RltProcedureModal',
  props: {
    axios: { type: Function, default: null }, // axios
    border: { type: Boolean, default: true }, // 是否展示边框
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      queryParams: {
        typeId: undefined,// 根据工序类型查询  给 undefined 这个 属性就不传后端去(json就没有这个字段)
        name: undefined,// 名字模糊查询 给 undefined 这个 属性就不传后端去(json就没有这个字段)
        maxResultCount: 999,// 每页数量 - 这里不需要分页，给最大值
        pageIndex: 1,// 当前页1 这个在params 里面 也过滤掉了,放这里方便复制~
        totalCount: 0,// 总数 这个在params 里面 也过滤掉了,放这里方便复制~
      },
      list: [],// table 数据源 列表元素只有 一个 ,方便 table装
      list_origin: [],// table 原始数据
      rltList: [],// table 数据源 右边的列表
      loading: false,// table 是否处于加载状态
      rltLoading: false,// table 是否处于加载状态
      subItemList: [],// 项目列表
      procedureTypes:[], // 工程类型 字典表
      contentId:undefined,// contentId ,编辑(配置工序的时候)传进来赋值,用于查询关联工序表
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
    // 工序 table 的 columns
    produceColumns() {
      return [
        { title: '工序名称', dataIndex: 'name'},
        { title: '工种', dataIndex: 'procedureWorkers', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '工程量清单', dataIndex: 'procedureMaterials', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '设备类型', dataIndex: 'procedureEquipmentTeams', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '操作', customRender: (text,record,index) => {
          return (<div>
            <div onClick={() => this.moveRight(record,index)}><a>添加</a></div>
          </div>);
        }},
      ];
    },
    // 关联工序 table 的 columns
    rltProduceColumns() {
      return [
        { title: '工序名称', dataIndex: 'procedure', customRender: (text, record, index) => <span>{text.name}</span> },
        { title: '操作', customRender: (text,record,index) => {
          return (<div>
            <div style='display:inline' onClick={() => this.moveLeft(record,index)}><a>移出</a></div>
            <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Up)}><a>上移</a></div>
            <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Down)}><a>下移</a></div>
          </div>);
        }},
      ];
    },
  },
  async created() {
    this.initAxios();
    // 模态框 也是 跟着 页面 加载 的，这个created 也就是说 只 执行一次
    await this.getProcedureList();
    await this.getProcedureTypes();
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
      apiSubItem = new ApiSubItem(this.axios);
      apiProject = new ApiProject(this.axios);
      apiProcedure = new ApiProcedure(this.axios);
    },

    // 给 string[] 加悬浮,不然列表放不下
    getElement(text, names) {
      let span = <span>{text.length === 1 ? names : names[0] + '...'}</span>;
      let toolTip = (<a-tooltip placement='bottom' title={names.join('  ,')}>{span}</a-tooltip>);
      return text.length === 0 ? undefined : toolTip;
    },

    // 移动关联工序表
    async moveRltPro(record, moveType) {
      let res = await apiSubItemContent.moveRltProcedure(record.id, moveType);
      // console.log('res', res);
      if (requestIsSuccess(res)) {
        await this.getRltProceduresByContentId();
      }
    },

    // (模态框)自己调用打开自己的方法
    edit(record) {
      this.status = ModalStatus.Edit;
      // 给 contentId 赋值
      this.contentId =record.id;
      this.getRltProceduresByContentId(this.contentId);

    },
    // 获取 工程类型 列表
    async getProcedureTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.ProjectType' });
      if (requestIsSuccess(res) && res.data) {
        this.procedureTypes = res.data;
      }
    },
    // 根据 contentId 获取关联工序表
    async getRltProceduresByContentId() {
      this.rltLoading = true;
      let res = await apiSubItemContent.getRltProceduresByContentId(this.contentId);
      if (requestIsSuccess(res)) {
        this.rltList = res.data;
        // 然后 把 关联工序的 内容 剪去
        this.list = this.list_origin.filter(x=>!this.rltList.map(y=>y.procedure.name).includes(x.name));
      }
      this.rltLoading = false;
    },
    // 获取工序列表
    async getProcedureList() {
      this.loading = true;
      let res = await apiProcedure.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res)) {
        this.list_origin = res.data.items;
        this.list = this.list_origin.filter(x=>!this.rltList.map(y=>y.procedure.name).includes(x.name));
        // console.log(res.data.items);
        this.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },

    // 右移 - 添加 procedure
    async moveRight(record,index) {
      let res = await apiSubItemContent.addProcedure(this.contentId, [...this.rltList.map(x => x.procedure.id), record.id]);
      if (requestIsSuccess(res)) {
        await this.getRltProceduresByContentId();
      }
    },
    // 左移
    async moveLeft(record, index) {
      let filter = this.rltList.map(x=>x.procedure.id).filter((x,i)=>i!==index);
      let res = await apiSubItemContent.addProcedure(this.contentId, filter);
      if (requestIsSuccess(res)) {
        await this.getRltProceduresByContentId();
      }
    },

    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
    // Modal 提交
    async ok() {
      // let response = await apiProcedure.configProcedure(this.record.id, data1);
      // if (utils.requestIsSuccess(response)) {
      this.close();
      this.$message.success('关联工序成功');
      this.$emit('success');
      // }
    },
  },
  render() {
    return (
      <a-modal
        title={'关联工序'}
        visible={this.visible}
        width="80%"
        onCancel={this.close}
        destroyOnClose
        okText={'保存'}
        onOk={this.ok}
      >
        <sc-table-operator
          lg={6}
          gutter={6}
          onSearch={() => {
            this.getProcedureList();
          }}
          onReset={() => {
            this.queryParams.typeId = undefined;
            this.queryParams.name = undefined;
            this.getProcedureList();
          }}
        >
          <a-form-item label='工程类型'>
            <a-select
              placeholder={'请选择工程类型'}
              value={this.queryParams.typeId}
              onChange={(val) => {
                this.queryParams.typeId = val;
                this.getProcedureList();
              }}>
              {this.procedureTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>
          <a-form-item label='工序名称'>
            <a-input
              placeholder={'请输入工序名称'}
              value={this.queryParams.name}
              onInput={async event => {
                this.queryParams.name = event.target.value;
                this.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.getProcedureList();
              }}
            />
          </a-form-item>
        </sc-table-operator>
        {/*table 表格*/}
        <a-row gutter={24}>
          <a-col span={13}>
            {/*左边工序表格*/}
            <a-table
              dataSource={this.list}
              size={'small'}
              rowKey={record => record.id}
              columns={this.produceColumns}
              loading={this.loading}
              bordered={true}
              pagination={false}
            />
          </a-col>
          <a-col span={11}>
            {/*右边关联工序表格*/}
            <a-table
              dataSource={this.rltList}
              size={'small'}
              rowKey={record => record.id}
              columns={this.rltProduceColumns}
              loading={this.rltLoading}
              bordered={true}
              pagination={false}
            />
          </a-col>
        </a-row>

      </a-modal>
    );
  },
};
