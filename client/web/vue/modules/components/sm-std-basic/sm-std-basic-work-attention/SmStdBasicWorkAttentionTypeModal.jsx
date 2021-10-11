import ApiWorkAttention from '../../sm-api/sm-std-basic/WorkAttention';
import { form as formConfig, tips } from '../../_utils/config';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import './style';
import { deleteEmptyProps } from '../../_utils/tree_array_tools';
import moment from 'moment';
let apiWorkAttention = new ApiWorkAttention();
export default {
  name: 'SmStdBasicWorkAttentionTypeModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      typeName: null,
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: null, // 表单绑的对象,
      confirmLoading: false,//确定按钮加载状态
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dataSource: [], // 列表数据源
      loading: false,
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {

          title: '类别',
          dataIndex: 'content',
          ellipsis: true,
          scopedSlots: { customRender: 'content' },

        },
        {
          title: '上次编辑时间',
          dataIndex: 'editData',
          scopedSlots: { customRender: 'editData' },

        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.refresh();
  },

  methods: {
    initAxios() {
      apiWorkAttention = new ApiWorkAttention(this.axios);
    },


    add(record) {
      this.status = ModalStatus.Add;
      this.refresh();
    },
    //编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.record = null;
      this.form.resetFields();
      this.typeName = null;
      this.$emit('success');
      this.status = ModalStatus.Hide;
      this.dataSource.map(item => {
        item.isEdit = false;
      });
    },
    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiWorkAttention.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.record = record;
              _this.$message.success('操作成功');
              _this.$emit('success');
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    // 刷新列表
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        repairTagKey: this.repairTagKey,
      };
      let response = await apiWorkAttention.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
        isType: true,
      });
      if (requestIsSuccess(response)) {
        let _dataSource = [];
        _dataSource = response.data.items;
        this.dataSource = _dataSource.map(item => {
          return {
            ...item,
            isEdit: false,
          };
        });
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
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    cancel() {
      this.typeName = null;
    },
    // 数据提交
    async ok(record) {
      if (record) {
        if(!record.content || record.content.replace(/\s/g,'')==''){
          this.$message.error('类别不能为空');
        }else{
          let data = {
            ...record,
            repairTagKey: this.repairTagKey,
          };
          let response = await apiWorkAttention.update(data);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('类别修改成功');
            record.isEdit = false;
            this.refresh(true);
            this.$emit('success');
          }
        }
      } else {
        if(!this.typeName || this.typeName.replace(/\s/g,'')=='' ){

          this.$message.error('类别不能为空');
        }else{
          let data = {
            content: this.typeName,
            isType: true,
            repairTagKey: this.repairTagKey,
          };
          let response = await apiWorkAttention.create(data);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('类别添加成功');
            this.typeName = null;
            this.refresh(true);
            this.$emit('success');
          }
        }
      }

    },
  },

  render() {
    return (
      <a-modal
        class="sm-std-basic-work-attention-type-model"
        title={`类别维护`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        okText="关闭"
        onOk={
          this.close
        }
        width={700}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item >
                <a-table
                  columns={this.columns}
                  rowKey={record => record.id}
                  dataSource={this.dataSource}
                  pagination={false}
                  loading={this.loading}
                  {...{
                    scopedSlots: {
                      index: (text, record, index) => {
                        let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                        return <a-tooltip title={result}>{result}</a-tooltip>;
                      },
                      content: (text, record, index) => {
                        let result = record ? record.content : '';
                        let input = <div>
                          <a-input
                            placeholder={this.status == ModalStatus.View ? '' : '请输入标题'}
                            disabled={this.status == ModalStatus.View}
                            value={record.content}
                            onChange={event => {
                              record.content = event.target.value;
                            }}
                          />
                        </div>;

                        let value = record.isEdit ? input : result;
                        return value;
                      },
                      editData: (text, record, index) => {
                        let result = (record && record.lastModificationTime) ?
                          moment(record.lastModificationTime).format("YYYY-MM-DD HH:mm:ss") :
                          moment(record.creationTime).format("YYYY-MM-DD HH:mm:ss");
                        return <a-tooltip title={result}>{result}</a-tooltip>;
                      },
                      operations: (text, record) => {
                        return [
                          <span>
                            {
                              record.isEdit ?
                                <span>
                                  <a onClick={() => {

                                    this.ok(record);
                                  }}
                                  >保存
                                  </a>
                                  <a-divider type="vertical" />
                                </span> : ''
                            }
                            <a
                              onClick={() => {
                                record.isEdit = true;
                              }}
                            >编辑
                            </a>
                            <a-divider type="vertical" />
                            <a
                              onClick={() => {
                                this.remove(record);
                              }}
                            > 删除
                            </a>
                          </span>,
                        ];
                      },
                    },
                  }}
                >
                </a-table>
              </a-form-item>
            </a-col>
            <a-col sm={2} md={2}>
              <a-form-item >
                {this.totalCount + 1}
              </a-form-item>
            </a-col>
            <a-col sm={16} md={16}>
              <a-form-item  >
                <a-input
                  placeholder={this.status == ModalStatus.View ? '' : '请输入类别'}
                  disabled={this.status == ModalStatus.View}
                  value={this.typeName}
                  onChange={event => {
                    this.typeName = event.target.value;
                  }}
                />
              </a-form-item>
            </a-col>
            <a-col sm={4} md={4}>
              <a-form-item  >
                <span>
                  <a
                    onClick={() => {
                      this.ok();
                    }}
                  >保存</a>
                  <a-divider type="vertical" />
                  <a
                    onClick={() => {
                      this.cancel();
                    }}
                  >取消
                  </a>
                </span>
              </a-form-item>
            </a-col>
          </a-row>
          {/* 分页器 */}
          <a-pagination
            style="float:right; margin-top:10px"
            total={this.totalCount}
            pageSize={this.queryParams.maxResultCount}
            current={this.pageIndex}
            onChange={this.onPageChange}
            onShowSizeChange={this.onPageChange}
            showSizeChanger
            showQuickJumper
            showTotal={paginationConfig.showTotal}
          />
        </a-form>
      </a-modal >
    );
  },
};
