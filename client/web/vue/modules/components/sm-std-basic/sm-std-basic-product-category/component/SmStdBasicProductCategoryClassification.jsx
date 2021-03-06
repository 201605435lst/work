import * as utils from '../../../_utils/utils';
import ApiProductCategory from '../../../sm-api/sm-std-basic/ProductCategory';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import { requestIsSuccess, vIf, vP } from '../../../_utils/utils';
import permissionsSmStdBasic from '../../../_permissions/sm-std-basic';
let apiProductCategory = new ApiProductCategory();
import SmStdBasicProductCategoryModel from './SmStdBasicProductCategoryModel';

// 定义表单字段常量
const formFields = ['parentId', 'name', 'code', 'extendCode', 'extendName', 'unit', 'remark'];
export default {
  name: 'SmStdBasicProductCategoryClassification',
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
          title: '编码',
          ellipsis: true,
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '扩展名称',
          ellipsis: true,
          dataIndex: 'extendName  ',
          scopedSlots: { customRender: 'extendName' },
        },
        {
          title: '扩展编码',
          ellipsis: true,
          dataIndex: 'extendCode  ',
          scopedSlots: { customRender: 'extendCode' },
        },
        {
          title: '单位',
          dataIndex: 'unit',
          ellipsis: true,
          width: '70px',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '备注',
          ellipsis: true,
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
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
      apiProductCategory = new ApiProductCategory(this.axios);
    },
    // 页面刷新
    async refresh(resetPage = true) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiProductCategory.getList({
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
          this.getGeneratedCode();
          let data = {
            parentId: this.parentId,
            ...values,
            code: values ? this.code + values.code : '',
            levelName: this.datas ? this.datas.LevelName : '',
          };
          response = await apiProductCategory.create(data);
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
      this.$refs.SmStdBasicProductCategoryModel.edit(record);
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
            let response = await apiProductCategory.delete(record.id);
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
    // 生成编码
    async generatedCode() {
      let num = '001';
      let response = await apiProductCategory.getListCode(this.datas ? this.datas.id : null);
      if (requestIsSuccess(response) && response.data) {
        let result = response.data;
        if (result) {
          let arr = result.code.split('.');
          let code = arr[arr.length - 1];
          num = (parseInt(code) + 1).toString();
          if (num.length == 1) {
            num = '00' + num;
          }
          if (num.length == 2) {
            num = '0' + num;
          }
        }
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ code: num });
      });
    },
    //存数据库的时候拼接
    getGeneratedCode() {
      let code = '';
      if (this.datas && this.datas.code) {
        code = this.datas.code;
      }
      this.code = code + '.';
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
      <div class="sm-std-basic-product-category-classification">
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
              extendName: (text, record, index) => {
                let result = record ? record.extendName : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },

              extendCode: (text, record, index) => {
                let result = record ? record.extendCode : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },
              unit: (text, record, index) => {
                let result = record ? record.unit : '';
                return result ? (
                  <a-tooltip title={result} placement="topLeft">
                    {result}
                  </a-tooltip>
                ) : (
                  ''
                );
              },

              remark: (text, record, index) => {
                let result = record ? record.remark : '';
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
                      vP(this.permissions, permissionsSmStdBasic.ProductCategories.Update),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.ProductCategories.Update) &&
                        vP(this.permissions, permissionsSmStdBasic.ProductCategories.Delete),
                    )}

                    {vIf(
                      <a
                        onClick={() => {
                          this.remove(record);
                        }}
                      >
                        删除
                      </a>,
                      vP(this.permissions, permissionsSmStdBasic.ProductCategories.Delete),
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
                            message: '请输入产品名称',
                            required: true,
                            whitespace: true,
                          },
                          { max: 200, message: '名称最多输入200字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={5} md={5}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input-search
                    placeholder="请编码"
                    enter-button="编码"
                    onSearch={this.generatedCode}
                    v-decorator={[
                      'code',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请输入编码',
                            whitespace: true,
                          },
                          { max: 50, message: '编码最多输入50字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="扩展名称"
                    v-decorator={[
                      'extendName',
                      {
                        initialValue: null,
                        rules: [
                          { whitespace: true },
                          { max: 50, message: '扩展名称最多输入50字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="扩展编码"
                    v-decorator={[
                      'extendCode',
                      {
                        initialValue: null,
                        rules: [
                          { whitespace: true },
                          { max: 50, message: '扩展编码最多输入50字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-input
                    placeholder="单位"
                    v-decorator={[
                      'unit',
                      {
                        initialValue: null,
                        rules: [{ whitespace: true }, { max: 30, message: '单位最多输入30字符' }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={3} md={3}>
                <a-form-item wrapper-col={{ span: 24 }}>
                  <a-textarea
                    rows="1"
                    placeholder="备注"
                    v-decorator={[
                      'remark',
                      {
                        initialValue: null,
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
          vP(this.permissions, permissionsSmStdBasic.ProductCategories.Create),
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
        <SmStdBasicProductCategoryModel
          ref="SmStdBasicProductCategoryModel"
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
