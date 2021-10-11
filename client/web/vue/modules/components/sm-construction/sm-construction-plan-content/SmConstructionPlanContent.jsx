
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment'; // 用于时间处理
import ApiPlanContent from '../../sm-api/sm-construction/ApiPlanContent';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import {pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionPlanContentModal from './SmConstructionPlanContentModal';
import SmConstructionBaseSubItemSelectTreeModal
  from '../../sm-construction-base/sm-construction-base-sub-item-select-tree/SmConstructionBaseSubItemSelectTreeModal';
import  SmConstructionPlanContentFrontSelectTreeModal from './SmConstructionPlanContentFrontSelectTreeModal';

let apiPlanContent = new ApiPlanContent();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查类型用

export default {
  name: 'SmConstructionPlanContent',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: true },
    showOperator: { type: Boolean, default: true }, // 是否显示操作栏()
    showSelectRow: { type: Boolean, default: true }, // 是否显示选择栏
    planId: { type: String, default: null }, // 计划 Id
    isModalState:{type:Boolean,default:false}, //是否是 模态框状态
  },
  data() {
    return {
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
      },
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      selectPlanContentIds: [], // 选择的 施工计划详情 ids (选择框模式的时候用)
      selectedEntity:undefined, // 选择的实体(任务计划详情 content )
      dicTypes: [],
      selectedSubItems: [], //已选择分部分项
      importModelVisible: false,
      selectId: undefined, // 单选的id
      clickTimeChange:null, // 单击事件的延时变量
      rowMultiple: false, // table Row 是否是 多选模式
    };
  },
  computed: {
    addBtnTitle() { // 添加按钮 标题
      if (this.list.length === 0) {
        return "新增任务";
      }
      return "新增子级";
    },
    addBtnDisable() { // 添加按钮 是否禁用
      if (this.list.length === 0) { // 长度是0 不禁用
        return false;
      }
      if (this.rowMultiple) { // 多选状态 禁用
        return true;
      }
      if (this.selectPlanContentIds.length === 1) { // 如果 多选了也禁用
        return false;
      }
      return true;
    },
    btnDisable() { // 按钮 是否 禁用
      if (this.selectedEntity === undefined) { // 没有选择任何东西  的话
        return true;
      }
      return this.rowMultiple;
    },
    customRow() { // return 一个 lambda
      return (record,index) => ({
        on: { // 事件
          click: (event) => {
            clearTimeout(this.clickTimeChange);
            this.clickTimeChange = setTimeout( // 因为双击事件会触发 单击事件,所以定义 一个定时器 ,当 0.3秒内 点击两次的话就不算单击啦
              () => {
                if (this.rowMultiple) { // 多选的话 就 往里面加
                  if (this.selectPlanContentIds.includes(record.id)) { // 包含 往里面 删
                    this.selectPlanContentIds = this.selectPlanContentIds.filter(x => x !== record.id);
                  }else{ // 不包含 就添加
                    this.selectPlanContentIds.push(record.id);
                  }
                }else{ // 单选就替换
                  this.selectPlanContentIds = [record.id];
                  this.selectedEntity =record;
                }
              },
              300,
            );
          },
          dblclick: (event) => {
            clearTimeout(this.clickTimeChange); // 清除延时
            event.preventDefault();
          },
          contextmenu: (event) => {
          },
          mouseenter: (event) => {
          },  // 鼠标移入行
          mouseleave: (event) => {
          },
        },
      });
    },
    columns() {
      let baseColumns = this.isModalState ?
        [
          {
            title: '任务名称', dataIndex: 'name', align: 'left', customRender: (text, record, index) => {
              return <span>{text}{record.isMilestone ? <a-tag style='margin-left:5px' color='blue'>里程碑</a-tag> : undefined}</span>;
            },
          },
          {
            title: '操作', width: 180, customRender: (record) => {
              return (
                <div>
                  <div onClick={() => this.import(record)}><a>引用</a></div>
                </div>
              );
            },
          },
        ]
        :
        [
          {
            title: '任务名称', dataIndex: 'name', align: 'left', customRender: (text, record, index) => {
              return <span>{text}{record.isMilestone ?
                <a-tag style='margin-left:5px' color='blue'>里程碑</a-tag> : undefined}</span>;
            },
          },
          {
            title: '前置任务', dataIndex: 'antecedents', customRender: (text, record, index) => {

              if (text.length === 0) {
                return (<span />);
              }
              const map = text.map(x => x.name);
              if (text.length === 1) {
                return (<span>{map.join(', ')}</span>);
              }
              return (
                <a-tooltip placement='top' title={text.map(x => x.name).join(',')}>
                  {map[0] + '...'}
                </a-tooltip>

              );
            },
          },
          { title: '工期', dataIndex: 'period' },
          {
            title: '开始时间', dataIndex: 'planStartTime', customRender: (text, record, index) => {
              return (<span>{moment(text).format('YYYY-MM-DD')}</span>);
            },
          },

          {
            title: '完成时间', dataIndex: 'planEndTime', customRender: (text, record, index) => {
              return (<span>{moment(text).format('YYYY-MM-DD')}</span>);
            },
          },
        ];
      return this.showOperator ? [
        ...baseColumns,
      ] : baseColumns;
    },
  },

  created() {
    this.initAxios();
    this.refresh();
    this.getDicTypes();
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiPlanContent = new ApiPlanContent(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    // 返回 上一级(总体计划图标)
    back() {
      console.log(' 返回 上一级(总体计划 table)');
      this.$emit('back');
    },
    // 切换 选择模式
    switchSelectType() {
      this.rowMultiple = !this.rowMultiple;
      this.selectPlanContentIds = []; // 同时清空选择的东西
    },
    async selected(value) {
      console.log(value);
      let subItemContentIds = value.map(x => x.id);
      let res = await apiPlanContent.import(this.selectId, subItemContentIds);
      if (requestIsSuccess(res)) {
        await this.refresh();
      }
    },
    // 刷新获取list
    async refresh() {
      // 刷新表之前先把 数据 弄回初始化
      this.list = [];
      this.selectPlanContentIds = [];
      this.selectedEntity = undefined;

      this.loading = true;
      let res = await apiPlanContent.getSingleTree(this.planId);
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data;
      }
      this.loading = false;
    },
    import(record) {
      this.importModelVisible=true;
      this.selectId = record.id;
    },
    // 获取 类型 列表
    async getDicTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.ProjectType' });
      if (requestIsSuccess(res) && res.data) {
        this.dicTypes = res.data;
      }
    },

    // 分页事件
    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
    // 切换 列表视图/ 横道图
    switchView() {
      console.log('切换 列表视图/ 横道图');
    },
    // 导入项目文件
    importProjectFile() {
      console.log('这个目前不做');
    },
    // 添加(打开添加模态框)
    add() {
      if (this.list.length === 0) { // 没有一个就新建 一个
        this.$refs.SmConstructionPlanContentModal.add(this.planId);
      }else{ // 否则就新建一个子级
        this.addChild({ id: this.selectPlanContentIds[0]});
      }
    },
    // 添加子级
    addChild(record) {
      const contentId = record.id;
      this.$refs.SmConstructionPlanContentModal.addChild(contentId);
    },
    // 升级
    async  lvUp(id) {
      let res = await apiPlanContent.lvUp(id);
      if (requestIsSuccess(res)) {
        await this.refresh();
      }
    },
    // 降级
    async  lvDown(id) {
      let res = await apiPlanContent.lvDown(id);
      if (requestIsSuccess(res)) {
        await this.refresh();
      }
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      this.$refs.SmConstructionPlanContentModal.edit(record);
    },
    async addFrontTask(record) {
      console.log(record);
      this.$refs.SmConstructionPlanContentFrontSelectTreeModal.addFrontTask(record,this.list);
    },

    remove(ids) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiPlanContent.deleteRange(ids);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    return (
      <div>
        {/* 操作区 */}
        {/*<sc-table-operator*/}
        {/*  onSearch={() => {*/}
        {/*    this.refresh();*/}
        {/*  }}*/}
        {/*  onReset={() => {*/}
        {/*    this.queryParams.searchKey = '';*/}
        {/*    this.queryParams.pageIndex = 1;*/}
        {/*    this.refresh();*/}
        {/*  }}*/}
        {/*>*/}
        {/*  <a-form-item label='关键字'>*/}
        {/*    <a-input*/}
        {/*      placeholder={'请输入关键字'}*/}
        {/*      value={this.queryParams.searchKey}*/}
        {/*      onInput={async event => {*/}
        {/*        this.queryParams.searchKey = event.target.value;*/}
        {/*        this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示*/}
        {/*        this.refresh();*/}
        {/*      }}*/}
        {/*    />*/}
        {/*  </a-form-item>*/}
        {/*  <a-form-item label='类型'>*/}
        {/*    <a-select*/}
        {/*      placeholder={'请选择类型'}*/}
        {/*      value={this.queryParams.typeId}*/}
        {/*      onChange={(val) => {*/}
        {/*        this.queryParams.typeId = val;*/}
        {/*        this.refresh();*/}
        {/*      }}>*/}
        {/*      {this.dicTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}*/}
        {/*    </a-select>*/}
        {/*  </a-form-item>*/}


        {/*  <template slot='buttons'>*/}
        {/*    {this.isModalState===false &&*/}
        {/*    ([*/}
        {/*      <a-button size='small' type='primary' icon='plus' onClick={() => this.add()}*/}
        {/*        disabled={this.addBtnDisable}>*/}
        {/*        {this.addBtnTitle}*/}
        {/*      </a-button>,*/}

        {/*      <a-button size='small' type='primary' icon='plus' onClick={() => this.importProjectFile()}>*/}
        {/*        导入项目文件*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.switchView()}>*/}
        {/*        横道图*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.addFrontTask(this.selectedEntity)}  disabled={this.btnDisable}>*/}
        {/*        设置前置任务*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.lvUp(this.selectedEntity.id)}*/}
        {/*        disabled={this.btnDisable}>*/}
        {/*        升级*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.lvDown(this.selectedEntity.id)}*/}
        {/*        disabled={this.btnDisable}>*/}
        {/*        降级*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.edit(this.selectedEntity)}*/}
        {/*        disabled={this.btnDisable}>*/}
        {/*        编辑*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='danger' icon='' onClick={() => this.remove(this.selectPlanContentIds)}*/}
        {/*        disabled={this.selectPlanContentIds.length === 0}>*/}
        {/*        删除*/}
        {/*      </a-button>,*/}
        {/*      <a-button size='small' type='primary' icon='' onClick={() => this.back()}>*/}
        {/*        返回*/}
        {/*      </a-button> ,*/}
        {/*      <a-button size="small" type='primary' icon='' onClick={() => this.import(this.selectedEntity)} disabled={this.btnDisable}>*/}
        {/*        引用*/}
        {/*      </a-button>,*/}
        {/*    ])*/}
        {/*    }*/}
        {/*  </template>*/}
        {/*</sc-table-operator>*/}
        {/*展示区*/}
        {this.list && this.list.length ?
          <a-table
            dataSource={this.list}
            rowKey={record => record.id}
            columns={this.columns}
            customRow={this.customRow}
            loading={this.loading}
            size="small"
            bordered={this.bordered}
            pagination={false}
            defaultExpandAllRows={true}
            rowSelection={this.showSelectRow ? {
              selectedRowKeys:this.selectPlanContentIds,
              // columnWidth: 30,
              type: this.rowMultiple ? 'checkbox' : 'radio',
              fixed: true,
              onChange: selectedRowKeys => {
                this.selectPlanContentIds = selectedRowKeys;
              },
            } : undefined}
          />
          :
          '暂无数据'
        }


        {/*分页*/}
        {/*<a-pagination*/}
        {/*  style='margin-top:10px; text-align: right;'*/}
        {/*  total={this.queryParams.totalCount}*/}
        {/*  pageSize={this.queryParams.maxResultCount}*/}
        {/*  current={this.queryParams.pageIndex}*/}
        {/*  onChange={this.onPageChange}*/}
        {/*  onShowSizeChange={this.onPageChange}*/}
        {/*  showSizeChanger={true}*/}
        {/*  showQuickJumper={true}*/}
        {/*  size={this.isSimple || this.isFault ? 'small' : ''}*/}
        {/*  showTotal={paginationConfig.showTotal}*/}
        {/*/>*/}

        {/*添加/编辑模板*/}
        <SmConstructionPlanContentModal
          ref='SmConstructionPlanContentModal'
          axios={this.axios}
          onSuccess={async () => {
            this.list = [];
            await this.refresh();
          }}
        />
        {/*添加前置任务-Modal*/}
        <SmConstructionPlanContentFrontSelectTreeModal
          ref='SmConstructionPlanContentFrontSelectTreeModal'
          axios={this.axios}
          onSuccess={async () => {
            this.list = [];
            await this.refresh();
          }}
        />
        {/*引用分布分项选择框*/}
        <SmConstructionBaseSubItemSelectTreeModal
          ref='SmConstructionBaseSubItemSelectTreeModal'
          visible={this.importModelVisible}
          axios={this.axios}
          multiple={true}
          value={this.selectedSubItems}
          onOk={this.selected}
          onChange={() => this.importModelVisible = false}
          onSuccess={async () => {
            this.list = [];
            await this.refresh();
          }}

        />
      </div>
    );
  },



};
