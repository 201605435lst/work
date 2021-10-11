import * as utils from '../../../_utils/utils';
import ApiQuotaCategory from '../../../sm-api/sm-std-basic/QuotaCategory';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import { requestIsSuccess, vIf, vP } from '../../../_utils/utils';
import permissionsSmStdBasic from '../../../_permissions/sm-std-basic';

let apiQuotaCategory = new ApiQuotaCategory();
import SmStdBasicQuotaCategoryModel from './SmStdBasicQuotaCategoryModel';

// 定义表单字段常量
const formFields = ['parentId', 'name', 'code', 'standardCode', 'content', 'areaId'];
export default {
  name: 'SmStdBasicQuotaCategoryClassification',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Object, default: null }, //父级记录
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },

      parentId: null,
      form: {}, // 表单
      code: null, //自动生成的编码
    };
  },

  computed: {
    columns() {
      return [
        {
          title: '#',
          dataIndex: 'index',
          width: '50px',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        {
          title: '专业',
          ellipsis: true,
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '标准编号',
          ellipsis: true,
          dataIndex: 'standardCode  ',
          scopedSlots: { customRender: 'standardCode' },
        },
        {
          title: '工作内容',
          ellipsis: true,
          dataIndex: 'content  ',
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '行政区域',
          dataIndex: 'areaId',
          ellipsis: true,
          width: '70px',
          scopedSlots: { customRender: 'areaId' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '120px',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    datas: {
      handler: function(val, oldVal) {
        if (this.datas != null) {
          this.parentId = this.datas.id;
          this.form = this.$form.createForm(this, {});
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
    this.refresh();
  },

  methods: {
    initAxios() {
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
    },
    // 页面刷新
    async refresh(resetPage = true) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiQuotaCategory.getList({
        parentId: this.parentId,
        ids: [],
        isAll: false,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        maxResultCount: this.queryParams.maxResultCount,
      });
      if (requestIsSuccess(response) && response.data && response.data.items) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        this.dataSource.map(item => {
          item.children = null;
        });
      }
    },

    //保存
    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            parentId: this.parentId,
            ...values,
            areaId: this.areaId,
          };
          response = await apiQuotaCategory.create(data);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
            await this.refresh(false);
            this.getParentId();
            this.form.resetFields();
          }
        }
      });
    },
    //取消
    cancel() {
      this.form.resetFields();
    },
    //编辑
    edit(record) {
      this.$refs.SmStdBasicQuotaCategoryModel.edit(record);
    },
    // 删除
    remove(record) {
      this.$emit('record', record);
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiQuotaCategory.delete(record.id);
            if (requestIsSuccess(response)) {
              setTimeout(resolve, 100);
              await _this.refresh(true);
              _this.getParentId();
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    //向父级传递数据
    getParentId() {
      let data = {
        parentId: this.parentId,
        length: this.dataSource.length,
      };
      this.$emit('dataValue', data);
    },

    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
  },
  render() {
    return (
      <div class="sm-std-basic-quota-category-classification">
        <a-table
          // size="middle"
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          bordered={this.bordered}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                );
              },
              name: (text, record, index) => {
                let result = record ? record.name : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },
              code: (text, record, index) => {
                let result = record ? record.code : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },
              standardCode: (text, record, index) => {
                let result = record ? record.standardCode : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },

              content: (text, record, index) => {
                let result = record ? record.content : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },
              areaId: (text, record, index) => {
                let result = record ? record.areaId : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >
                        编辑
                      </a>,
                      vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Update),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Update) &&
                        vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Delete),
                    )}

                    {vIf(
                      <a
                        onClick={() => {
                          this.remove(record);
                        }}
                      >
                        删除
                      </a>,
                      vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Delete),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {vIf(
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={1} md={1}>
                <a-form-item wrapper-col={{ offset: 24 }}>{this.totalCount + 1}</a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="请输入"
                    v-decorator={[
                      'name',
                      {
                        initialValue: null,
                        rules: [
                          {
                            message: '请输入定额分类名称',
                            required: true,
                            whitespace: true,
                          },
                          { max: 100, message: '名称最多输入100字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={5} md={5}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input-search
                    placeholder="专业"
                    v-decorator={[
                      'code',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请输入专业',
                            whitespace: true,
                          },
                          { max: 100, message: '专业最多输入100字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="标准编号"
                    v-decorator={[
                      'standardCode',
                      {
                        initialValue: null,
                        rules: [
                          { whitespace: true },
                          { max: 100, message: '标准编号最多输入100字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="工作内容"
                    v-decorator={[
                      'content',
                      {
                        initialValue: null,
                        rules: [{ whitespace: true }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="行政区域"
                    v-decorator={[
                      'areaId',
                      {
                        initialValue: null,
                        rules: [{ whitespace: true }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <span>
                    <a
                      onClick={() => {
                        this.save();
                      }}
                    >
                      保存
                    </a>
                    <a-divider type="vertical" />
                    <a
                      onClick={() => {
                        this.cancel();
                      }}
                    >
                      取消
                    </a>
                  </span>
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>,
          vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Create),
        )}

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

        {/* 添加/编辑模板 */}
        <SmStdBasicQuotaCategoryModel
          ref="SmStdBasicQuotaCategoryModel"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
          onGetParent={() => {
            this.getParentId();
          }}
        />
      </div>
    );
  },
};
