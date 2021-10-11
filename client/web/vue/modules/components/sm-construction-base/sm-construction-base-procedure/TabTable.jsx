import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus, PageState } from '../../_utils/enum';
import { vIf } from '../../_utils/utils';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';
import { TabEnum } from './TabEnum';

let apiProcedure = new ApiProcedure();


// 每个tab 都有table ,所以封装下
export default {
  name: 'TabTable',
  props: {
    axios: { type: Function, default: null },
    // 选择的ids
    selectIds: { type: Array, default: () => [] },
    // 所有 列表
    allList: { type: Array, default: () => [] },
    // 表格的 columns
    columns: { type: Array, default: () => [] },
    type: { type: Number, default: () => 0 },
  },
  data() {
    return {
      selectedDocumentIds: [],//表格的选中项
      loading: false,
      list: [],
    };

  },
  computed: {},

  watch: {
    // 监听 allList ,因为allList 一开始是空的,后来 填充了值,然后 data-source 绑定的是更新后的值
    allList: {
      handler: function(value, oldValue) {
        this.list = value;
      },
      immediate: true,
    },
    selectIds: {
      handler: function(value, oldVal) {
        // console.log('tab内部prop selectIds', value);
        this.selectedDocumentIds = value;
      },
      // immediate设为true后，则监听的这个对象会立即输出
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiProcedure = new ApiProcedure(this.axios);
    },
  },
  render() {
    let customRow = (record, index) => ({
      on: {
        // 表格双击事件监听
        dblclick: () => {
          if (this.selectedDocumentIds.findIndex(x => x === record.id) === -1) {
            this.$message.error('请先勾选然后在编辑!');
          } else {
            this.list[index].editable = true;
          }
        },
      },
    });
    return (
      <div>
        <a-table
          columns={this.columns}
          customRow={customRow}
          dataSource={this.list}
          rowKey={record => record.id}
          loading={this.loading}
          border={true}
          pagination={false}
          rowSelection={{
            selectedRowKeys: this.selectedDocumentIds,
            onChange: (selectedRowKeys) => {
              this.selectedDocumentIds = selectedRowKeys;
              this.$emit('selectIds', { type: this.type, selectIds: selectedRowKeys, list: this.list });
            },
          }}
          {...{
            scopedSlots: {
              count: (text, record, index) => {
                return record.editable ? (
                  <a-input-number
                    min={0}
                    max={1000000}
                    value={record.count}
                    onChange={event => {
                      record.count = event;
                      this.list[index].count = event;
                      this.$emit('outList', { type: this.type, list: this.list });
                    }}
                  />
                ) : (
                  record.count
                );
              },
              operations: (text, record) => {
                return (
                  <span>{!record.editable
                    ?
                    (<a onClick={() => {
                      if (this.selectedDocumentIds.findIndex(x => x === record.id) === -1) {
                        this.$message.error('请先勾选然后在编辑!');
                        return;
                      }
                      record.editable = true;
                    }}>编辑</a>)
                    :
                    (<a onClick={() => {
                      record.editable = false;
                    }}>保存</a>)
                  }
                  </span>
                );
              },
            },
          }}
        />
      </div>
    );
  },
};
