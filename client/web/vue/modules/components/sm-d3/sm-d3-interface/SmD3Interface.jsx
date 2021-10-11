import './style/index';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiConstructInterface from '../../sm-api/sm-technology/ConstructInterface';
import SmTechnologyTable from '../../sm-technology/sm-technology-components/src/SmTechnologyTable';
import SmTechnologyTimeLine from '../../sm-technology/sm-technology-components/src/SmTechnologyTimeLine';
import SmTechnologyInterfaceFlagModal from '../../sm-technology/sm-technology-interface-flag/SmTechnologyInterfaceFlagModal';
import SmTechnologyInterfaceFlagReformModal from '../../sm-technology/sm-technology-interface-flag/SmTechnologyInterfaceFlagReformModal';
import SmD3InterfaceModal from './SmD3InterfaceModal';
import { ConstructType } from '../../_utils/enum';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
let apiDataDictionary = new ApiDataDictionary();
let apiConstructInterface = new ApiConstructInterface();
export default {
  name: 'SmD3Interface',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object,
      default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    interfancePosition: { type: String, default: null },
    height: { type: String, default: '60%' },
    width: { type: String, default: '840px' },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iVisible: false,
      shwoInfo: false,
      dataSource: [],
      loading: false,
      totalCount: 0,
      dataDictonaries: [],
      pageIndex: 1,
      record: null,
      interfaceManagementTypeId: null,//工程类型
      maxResultCount: paginationConfig.defaultPageSize,
      tabKey: 'tujian',
      iInterfancePosition: null,
    };
  },

  computed: {},

  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    interfancePosition: {
      handler: function (value, oldValue) {
        this.iInterfancePosition = this.interfancePosition;
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    await this.tabSelect();
    setTimeout(() => {
      this.refresh();
    });
  },

  mounted() { },

  methods: {
    initAxios() {
      apiConstructInterface = new ApiConstructInterface(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        interfaceManagementTypeId: this.interfaceManagementTypeId,
      };
      let response = await apiConstructInterface.getList({
        skipCount: (this.pageIndex - 1) * this.maxResultCount,
        ...data,
      });

      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        if (page && this.totalCount && this.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.maxResultCount);
          if (this.totalCount % this.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
      this.loading = false;
    },
    async tabSelect() {
      let response = await apiDataDictionary.getValues({ groupCode: 'InterfaceManagementType' });
      if (requestIsSuccess(response)) {
        this.dataDictonaries = response.data;
        this.interfaceManagementTypeId = this.dataDictonaries[0] ? this.dataDictonaries[0].id : '';
        this.tabKey = this.dataDictonaries[0] ? this.dataDictonaries[0].id : '';
      }
    },
    view(record) {
      this.$refs.SmTechnologyTimeLine.view(record, 'flag');
    },
    /* 标记 */
    sign(record) {
      this.$refs.SmTechnologyInterfaceFlagModal.sign(record);
    },
    /* 整改 */
    reform(record) {
      this.$refs.SmTechnologyInterfaceFlagReformModal.reform(record);
    },
    add() {
      this.$refs.SmD3InterfaceModal.add(this.interfaceManagementTypeId, this.iInterfancePosition);
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
  },
  render() {
    let tableAction = [];
    {
      this.dataDictonaries && this.dataDictonaries.length > 0 ?
        this.dataDictonaries.map(item => {
          tableAction.push(<span
            class={{ selected: this.tabKey === item.id }}
            onClick={e => {
              this.tabKey = item.id;
              this.interfaceManagementTypeId = item.id;
              this.refresh();
            }}
          >
            {item.name}
          </span>);
        })
        : '';
    };
    return (

      <sc-panel
        class="d3-interface"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        height={this.height}
        width={this.width}
        animate="bottomEnter"
        forceRender
        icon="alert"
        resizable
        title={''}
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <span slot="icon">
          <span><a-icon type="interaction" /></span>
          <span style="margin-left:10px">接口管理</span>
          {this.iInterfancePosition ? <span style="margin-left:10px"><a-tooltip title="添加" placement="topLeft">
            <a-icon type="plus" onClick={this.add} />
          </a-tooltip> </span> : ""}
        </span>
        <template slot="headerExtraContent">
          <div class="tab-head">
            {tableAction}
          </div>
        </template>
        <SmTechnologyTable
          permissions={this.permissions}
          datas={this.dataSource}
          type="flag"
          isD3={true}
          pageIndex={this.pageIndex}
          maxResultCount={this.maxResultCount}
          onView={value => {
            this.view(value);
          }}
          onSign={value => {
            this.sign(value);
          }}
          onReform={value => {
            this.reform(value);
          }}
          onFlyTo={(data) => {
            this.$emit('flyTo', data);
          }}
          axios={this.axios}
        />
        {/* 分页器 */}
        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
        {/* 添加/编辑模板    ----整改*/}
        <SmTechnologyInterfaceFlagReformModal
          ref="SmTechnologyInterfaceFlagReformModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 标记记录    ----整改*/}
        <SmTechnologyTimeLine
          ref="SmTechnologyTimeLine"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 添加/编辑模板  -----接口标记*/}
        <SmTechnologyInterfaceFlagModal
          ref="SmTechnologyInterfaceFlagModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 添加/编辑模板 */}
        <SmD3InterfaceModal
          ref="SmD3InterfaceModal"
          axios={this.axios}
          onSuccess={(action, data) => {
            this.iInterfancePosition=null;
            this.refresh(false);
          }}
        />
      </sc-panel>



    );
  },
};
