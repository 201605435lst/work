import ApiInfluenceRange from '../../sm-api/sm-std-basic/InfluenceRange';
import { ModalStatus, RepairLevel } from '../../_utils/enum';
import { pagination as paginationConfig } from '../../_utils/config';
import { requestIsSuccess } from '../../_utils/utils';

import './style/index.less';

let apiInfluenceRange = new ApiInfluenceRange();

export default {
  name: 'SmAddInfluenceModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    repairLevel: { type: Array, default: new Array() },
    multiple: { type: Boolean, default: true },
  },
  data() {
    return {
      influenceRanges: [],
      checkedInfluenceIds: [],
      checkedInfluences: [],
      status: ModalStatus.Hide,
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          width: 90,
          ellipsis: true,
        },
        {
          title: '维修级别',
          dataIndex: 'repairLevelStr',

          ellipsis: true,
        },
        {
          title: '影响范围',
          dataIndex: 'content',
          // scopedSlots: { customRender: 'index' },
          // width: 90,
          ellipsis: true,
        },
      ];
    },
  },
  watch: {
    status: {
      handler: function(value, oldVal) {
        if (value !== ModalStatus.Hide) {
        }
      },
      immediate: true,
    },
    repairLevel: {
      handler: function(value, oldVal) {
        if (value) {
          this.checkedInfluences = [];
        }
      },
    },
  },
  created() {
    this.initAxios();
    this.initData();
  },
  methods: {
    initAxios() {
      apiInfluenceRange = new ApiInfluenceRange(this.axios);
    },

    async initData(resetPage = true) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let param = {
        maxResultCount: this.queryParams.maxResultCount,
        repairLevel: this.repairLevel,
      };
      // console.log(param);
      param.skipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;

      let response = await apiInfluenceRange.getList(param, this.repairTagKey);
      if (requestIsSuccess(response)) {
        this.influenceRanges = response.data.items;
        this.totalCount = response.data.totalCount;
      }
    },

    open() {
      this.status = ModalStatus.Add;
      this.initData();
    },

    close() {
      this.status = ModalStatus.Hide;
      this.influenceRanges = [];
      this.checkedInfluenceIds = [];
    },

    ok() {
      // 数据提交
      let res = '';
      if (this.checkedInfluences.length > 0) {
        this.checkedInfluences.map(item => {
          res += item.content + '\r\n';
        });
      }
      this.$emit('success', res);
      this.close();
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.initData(false);
      }
    },

    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.checkedInfluences) {
          let target = this.influenceRanges.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.checkedInfluenceIds) {
          let temp = this.influenceRanges.find(item => item.id === id);
          if (!!temp) {
            _selected.push(JSON.parse(JSON.stringify(temp)));
          }
        }

        this.checkedInfluences = _selected;
      } else {
        this.checkedInfluences = selectedRows;
      }
    },
  },
  render() {
    return (
      <a-modal
        class="add-influence-modal"
        title={`选择标准影响范围`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
        width={800}
      >
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.influenceRanges}
          rowSelection={{
            type: 'checkbox',
            columnWidth: 30,
            selectedRowKeys: this.checkedInfluenceIds,
            onChange: (selectedRowKeys, selectedRows) => {
              this.checkedInfluenceIds = selectedRowKeys;
              this.updateSelected(selectedRows);
              // this.checkedInfluences = selectedRows;
            },
          }}
          scroll={this.isSimple || this.isFault ? { y: 300 } : undefined}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return (this.pageIndex - 1) * this.queryParams.maxResultCount + (index + 1);
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}
        <a-pagination
          style="margin-top:10px; text-align: right;"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
        />
      </a-modal>
    );
  },
};
