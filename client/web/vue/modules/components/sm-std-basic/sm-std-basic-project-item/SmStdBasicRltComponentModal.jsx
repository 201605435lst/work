import { ModalStatus } from '../../_utils/enum';
import ApiComponentCategory from '../../sm-api/sm-std-basic/ComponentCategory';
import ApiProductCategory from '../../sm-api/sm-std-basic/ProductCategory';
import ApiProjectItemRltComponentCategory from '../../sm-api/sm-std-basic/ProjectItemRltComponentCategory';
import ApiProjectItemRltProductCategory from '../../sm-api/sm-std-basic/ProjectItemRltProductCategory';
import * as utils from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import CompentTreeSelect from '../../sm-std-basic/sm-std-basic-component-category-tree-select';
import ProductTreeSelect from '../../sm-std-basic/sm-std-basic-product-category-tree-select';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';

let apiComponentCategory = new ApiComponentCategory();
let apiProductCategory = new ApiProductCategory();
let apiProjectItemRltComponentCategory = new ApiProjectItemRltComponentCategory();
let apiProjectItemRltProductCategory = new ApiProjectItemRltProductCategory();
export default {
  name: 'SmStdBasicRltComponentModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: null,
      showComponent: false,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },
      ids: null, //关联产品或构件的ids
      id: null, //工程工项Id
      dataSource: null,
    };
  },

  computed: {
    title() {
      return this.showComponent ? '关联构件' : '关联产品';
    },
    visible() {
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
          title: '名称',
          dataIndex: 'name',
        },
        {
          title: '编码',
          dataIndex: 'code',
        },
        {
          title: '扩展名称',
          dataIndex: 'extendName',
        },
        {
          title: '扩展编码',
          dataIndex: 'extendCode',
        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiComponentCategory = new ApiComponentCategory(this.axios);
      apiProductCategory = new ApiProductCategory(this.axios);
      apiProjectItemRltComponentCategory = new ApiProjectItemRltComponentCategory(this.axios);
      apiProjectItemRltProductCategory = new ApiProjectItemRltProductCategory(this.axios);
    },

    //关联构件
    async relevenceComponent(record) {
      this.status = ModalStatus.Add;
      this.id = record.id;
      this.showComponent = true;
      let response = await apiProjectItemRltComponentCategory.getListByProjectItemId({
        id: record.id,
        isAll: true,
      });
      let data = response.data.items.map(x => x.componentCategoryId);
      this.ids = data;
      this.refresh();
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    //关联产品
    async relevenceProduct(record) {
      this.status = ModalStatus.Add;
      this.id = record.id;
      this.showComponent = false;
      let response = await apiProjectItemRltProductCategory.getListByProjectItemId({
        id: record.id,
        isAll: true,
      });
      let data = response.data.items.map(x => x.productCategoryId);
      this.ids = data;
      this.refresh();
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    delete(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          _this.ids = _this.ids.filter(x => x != record.id);
          _this.refresh();
        },
        onCancel() {},
      });
    },

    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
    },

    async ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = this.showComponent
            ? await apiProjectItemRltComponentCategory.editList({
                projectItemId: this.id,
                componentCategoryIdList: this.ids,
              })
            : await apiProjectItemRltProductCategory.editList({
                projectItemId: this.id,
                productCategoryIdList: this.ids,
              });
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.form.resetFields();
          }
        }
      });
    },

    onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = null;
      response = this.showComponent
        ? await apiComponentCategory.getListComponent({
            ids: this.ids,
            isAll: false,
            skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
            ...this.queryParams,
          })
        : await apiProductCategory.getListProduct({
            ids: this.ids,
            isAll: false,
            skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
            ...this.queryParams,
          });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        this.dataSource.map(item => {
          item.children = null;
        });
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
    },
  },

  render() {
    return (
      <a-modal
        visible={this.visible}
        title={this.title}
        onCancel={this.close}
        onOk={this.ok}
        width={800}
      >
        {this.showComponent ? (
          <CompentTreeSelect
            style="margin-bottom:10px;"
            value={this.ids}
            placeholder={this.status == ModalStatus.View ? '' : '请选择要关联的构件'}
            axios={this.axios}
            onChange={value => {
              this.ids = value;
              this.refresh();
            }}
            treeCheckable={true}
            maxTagCount={5}
            // showSearch={true}
            treeCheckStrictly={true}
          />
        ) : (
          <ProductTreeSelect
            style="margin-bottom:10px;"
            value={this.ids}
            placeholder={this.status == ModalStatus.View ? '' : '请选择要关联的产品'}
            axios={this.axios}
            onChange={value => {
              this.ids = value;
              this.refresh();
            }}
            treeCheckable={true}
            maxTagCount={5}
            // showSearch={true}
            treeCheckStrictly={true}
          />
        )}

        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return result;
              },
              operations: (text, record, index) => {
                return [
                  <span>
                    <a
                      onClick={() => {
                        this.delete(record);
                      }}
                    >
                      删除
                    </a>
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        <a-pagination
          style="margin-top: 10px;display: flex;justify-content:flex-end"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
      </a-modal>
    );
  },
};
