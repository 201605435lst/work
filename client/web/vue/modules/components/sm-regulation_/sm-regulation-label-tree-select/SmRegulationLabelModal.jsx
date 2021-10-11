import './style';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import moment from 'moment';
import ApiLabel from '../../sm-api/sm-regulation/Label';
import permissionsSmRegulation from '../../_permissions/sm-regulation';

let apiLabel = new ApiLabel();

export default {
  name: 'SmRegulationLabelModal',

  model: {
    prop: 'visible',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    visible: { type: Boolean, default: false },
    placeholder: { type: String, default: '请点击选择标签' },
    value: { type: [String, Array], default: null }, //已选项
    multiple: { type: Boolean, default: true }, // 是否多选
    selected: { type: Array, default: () => [] }, //所选标签
  },

  data() {
    return {
      dataSource: [], //数据源
      pageIndex: 1,
      totalCount: 0,
      //columnKey: 'creationTime', //默认排序索引创建时间
      order: 'descend', //默认排序方式降序
      selectedRowKeys: [],
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },

      selectedLabels: [],
      selectedLabelsIds: [],
      iVisible: false,
      iSelected: [], //已选标签

      name: '',
      classify: '',
      creationTime: '',
    };
  },

  computed: {
    tags() {
      return this.selectedLabels;
    },

    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '标签名',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '标签类别',
          dataIndex: 'classify',
          scopedSlots: { customRender: 'classify' },
        },
        {
          title: '创建时间',
          dataIndex: 'creationTime',
          width: 160,
          sorter: () => {},
          scopedSlots: { customRender: 'creationTime' },
        },
        {
          title: '操作',
          dataIndex: 'citation',
          scopedSlots: { customRender: 'citation' },
        },
      ];
    },
  },

  watch: {
    value: {
      handler: function(value, oldValue) {
        this.selectedLabels = value;
        this.selectedLabelsIds = value.map(item => item.id);
      },
      immediate: true,
    },

    visible: {
      handler: function(value, oldValue) {
        this.iVisible = value;
        this.selectedLabels = this.value;
        this.selectedLabelsIds = this.value.map(item => item.id);
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
      apiLabel = new ApiLabel(this.axios);
    },

    onOk() {
      this.$emit('ok', this.selectedLabels);
      this.onClose();
    },

    onClose() {
      this.$emit('change', false);
      this.selectedLabels = [];
      this.selectedLabelsIds = [];
    },

    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.selectedLabels.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedLabelsIds) {
          let installationSite = this.selectedLabels.find(item => item.id === id);
          if (!!installationSite) {
            _selected.push(JSON.parse(JSON.stringify(installationSite)));
          }
        }
        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }
    },

    async delete(record) {
      let response = await apiLabel.delete(record.id);
      if (requestIsSuccess(response)) {
        this.$message.success('删除成功');
        this.refresh(false, this.pageIndex);
      }
    },

    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        //   columnKey: this.columnKey,
        order: this.order ? this.order : 'descend',
      };
      let response = await apiLabel.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
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

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    //添加项目成员
    add() {
      let newUserDataList = [];
      //空白添加
      newUserDataList = [
        {
          name: '',
          classify: '',
        },
      ];
      this.dataSource = [...this.dataSource, ...newUserDataList];
    },

    async save(record) {
      let response = null;
      if (record.id != null) {
        response = await apiLabel.update(record);
        if (requestIsSuccess(response)) {
          this.$message.success('更新成功');
          this.refresh(false, this.pageIndex);
        }
      } else {
        response = await apiLabel.create(record);
        if (requestIsSuccess(response)) {
          this.$message.success('保存成功');
          this.refresh(false, this.pageIndex);
        }
      }
    },
  },

  render() {
    return (
      <a-modal
        width={850}
        title="标签选择"
        class="sm-basic-selectedLabels-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return (
                <div class="selected-item">
                  <a-icon style={{ color: '#f4222d' }} type={'bank'} />
                  <span class="selected-name"> {item ? item.name : null} </span>
                  <span
                    class="btn-close"
                    onClick={() => {
                      this.selectedLabels = this.selectedLabels.filter(
                        _item => _item.id !== item.id,
                      );
                      this.selectedLabelsIds = this.selectedLabelsIds.filter(id => id !== item.id);
                    }}
                  >
                    <a-icon type="close" />
                  </span>
                </div>
              );
            })
          ) : (
            <span style="margin-left:10px;">请选择</span>
          )}
        </div>

        <div>
          <div style="display:flex;justify-content:space-between;margin-bottom:7px">
            <div>
              {vIf(
                <a-button type="primary" icon="plus" onClick={() => this.add()}>
                  新增
                </a-button>,
                vP(this.permissions, permissionsSmRegulation.Labels.Create),
              )}
            </div>
            <div>
              <a-input-search
                placeholder="请输入标签名称"
                style="width: 200px"
                allowClear
                value={this.queryParams.keyWords}
                onInput={event => {
                  this.queryParams.keyWords = event.target.value;
                  this.refresh();
                }}
              />
            </div>
          </div>
          {/* 展示区 */}
          <a-table
            columns={this.columns}
            dataSource={this.dataSource}
            rowKey={record => record.id}
            multiple={true}
            loading={this.loading}
            pagination={false}
            onChange={(a, b, c) => {
              this.order = c.order;
              this.refresh();
            }}
            rowSelection={{
              type: this.multiple ? 'checkbox' : 'radio',
              columnWidth: 30,
              selectedRowKeys: this.selectedLabelsIds,
              onChange: (selectedRowKeys, selectedRows) => {
                this.selectedLabelsIds = selectedRowKeys;
                this.selectedLabels = selectedRows;
                this.updateSelected(selectedRows);
              },
            }}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                  return <a-tooltip title={result}>{result}</a-tooltip>;
                },
                name: (text, record, index) => {
                  return [
                    <a-input
                      class="cellWidth"
                      value={record.name}
                      onChange={value => {
                        record.name = value.target.value;
                      }}
                    />,
                  ];
                },
                classify: (text, record, index) => {
                  return [
                    <a-input
                      class="cellWidth"
                      value={record.classify}
                      onChange={value => {
                        record.classify = value.target.value;
                      }}
                    ></a-input>,
                  ];
                },
                creationTime: (text, record, index) => {
                  let result = moment(record.creationTime).format('YYYY-MM-DD hh:mm:ss');
                  return (
                    <a-tooltip placement="topLeft" title={result}>
                      <span>{result}</span>
                    </a-tooltip>
                  );
                },
                citation: (text, record, index) => {
                  return [
                    <span>
                      {vIf(
                        <a onClick={() => this.save(record)}>
                          <a-icon
                            type="check"
                            style="fontSize: 16px;padding-left:5px;padding-right:10px"
                          />
                        </a>,
                        vP(this.permissions, permissionsSmRegulation.Labels.Update),
                      )}
                      {vIf(
                        <a onClick={() => this.delete(record)}>
                          <a-icon type="close" style="color: red;fontSize: 16px;" />
                        </a>,
                        vP(this.permissions, permissionsSmRegulation.Labels.Delete),
                      )}
                    </span>,
                  ];
                },
              },
            }}
          ></a-table>
          <a-pagination
            style="margin-top:10px; text-align: right;"
            size="small"
            total={this.totalCount}
            pageSize={this.queryParams.maxResultCount}
            current={this.pageIndex}
            onChange={this.onPageChange}
            onShowSizeChange={this.onPageChange}
            showSizeChanger
            showQuickJumper
            showTotal={paginationConfig.showTotal}
          />
        </div>
      </a-modal>
    );
  },
};
