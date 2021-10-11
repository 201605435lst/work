import { pagination as paginationConfig, tips } from '../../../_utils/config';
import { ModalStatus } from '../../../_utils/enum';
import SmStdBasicMvdPropertyTreeSelect from '../../sm-std-basic-mvd-property-tree-select';
import ApiMVDProperty from '../../../sm-api/sm-std-basic/MVDProperty';
import ApiProductCategoryRltMVDProperty from '../../../sm-api/sm-std-basic/ProductCategoryRltMVDProperty';
import { requestIsSuccess } from '../../../_utils/utils';

let apiMVDProperty = new ApiMVDProperty();
let apiProductCategoryRltMVDProperty = new ApiProductCategoryRltMVDProperty();
export default {
  name: 'SmStdBasicProductCategoryRltProperty',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    productCategoryId: { type: String, default: null },
  },

  data() {
    return {
      dataSource: [],
      cacheDataSource: [],
      editingKey: '', //当前编辑的key
      ids: [],
      id: null,

      form: {},
      record: null,
      status: ModalStatus.Hide,

      // pageIndex: 1,
      totalCount: 0,

      // queryParams: {
      //   keyWord: null,
      //   maxResultCount: paginationConfig.defaultPageSize,
      // },

      editable: false,
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
          width: '20%',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '属性名称',
          dataIndex: 'name',
          width: '40%',
        },
        {
          title: '属性值',
          dataIndex: 'value',
          width: '25%',
          scopedSlots: { customRender: 'value' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  watch: {
    productCategoryId: function () {
      this.ids = [];
      this.refresh();
    },
  },

  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.refresh(false);
  },

  methods: {
    initAxios() {
      apiMVDProperty = new ApiMVDProperty(this.axios);
      apiProductCategoryRltMVDProperty = new ApiProductCategoryRltMVDProperty(this.axios);
    },

    // async add(key) {
    //   this.status = ModalStatus.Add;
    //   this.id = key;
    //   let response = await apiProductCategoryRltMVDProperty.getListByProductCategoryId({
    //     productCategoryId: this.id,
    //   });
    //   if (requestIsSuccess(response)) {
    //     this.ids = response.data.items.map(x => x.mvdPropertyId);
    //     this.dataSource = response.data.items;
    //     this.cacheDataSource = this.dataSource;
    //     this.totalCount = response.data.totalCount;
    //   }
    //   // this.refresh();
    // },

    async remove(key) {
      let _this = this;
      this.$confirm({
        title: tips.remove.title,
        content: h => <div style="color:red;">{tips.remove.content}</div>,
        okType: 'danger',
        onOk() {

          _this.ids = _this.ids.filter(id => id !== key);
          _this.cacheDataSource = _this.cacheDataSource.filter(x => x.mvdPropertyId !== key);
          _this.dataSource = _this.dataSource.filter(x => x.mvdPropertyId !== key);
          // _this.dataSource = _this.dataSource.filter(x => x.id !== key);


          // _this.refresh();
        },
      });
    },

    // cancel() {
    //   this.status = ModalStatus.Hide;
    //   this.ids = [];
    //   this.dataSource = null;
    // },

    async ok() {
      // let response = await apiMVDProperty.getList({
      //   ids: this.ids,
      //   isAll: true,
      //   skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
      //   ...this.queryParams,
      // });
      // if (requestIsSuccess(response)) this.dataSource = response.data.items;
      // let list = []; //信息交换模板属性的id和value
      // this.dataSource.map(item => {
      //   let sublist = {
      //     mVDPropertyId: item.mvdPropertyId ? item.mvdPropertyId : item.id,
      //     value: item.value,
      //   };
      //   list.push(sublist);
      // });
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiProductCategoryRltMVDProperty.editList({
            productCategoryId: this.productCategoryId,
            // list: list,
            list: this.dataSource,
          });
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.refresh();
          }
        }
      });
    },

    async refresh(resetPage = true, page) {
      console.log('refresh');
      // if (resetPage) {
      //   this.pageIndex = 1;
      //   this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      // }
      // if (this.ids.length == 0) {
      //   this.dataSource = null;
      // }
      // else {
      let response = await apiProductCategoryRltMVDProperty.getListByProductCategoryId({
        productCategoryId: this.productCategoryId,
        isAll: false,
        ids: this.ids.filter(item => item != null),
        //skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        //...this.queryParams,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        // this.dataSource.map(item => {
        //   item.value = null;
        //   this.cacheDataSource.map(cacheitem => {
        //     //cacheDataSource第一次是从关联表的数据库查找，也可能是从信息交换模板属性中找
        //     if (
        //       (cacheitem.mvdPropertyId == item.id || cacheitem.id == item.id) &&
        //       cacheitem.value != null
        //     )
        //       item.value = cacheitem.value;
        //   });
        // });
        this.cacheDataSource = this.dataSource;
        this.totalCount = response.data.totalCount;
        this.ids = response.data.items.map(x => x.mvdPropertyId);

        // if (page && this.totalCount && this.queryParams.maxResultCount) {
        //   let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
        //   if (this.totalCount % this.queryParams.maxResultCount !== 0) {
        //     currentPage = page + 1;
        //   }
        //   if (page > currentPage) {
        //     this.pageIndex = currentPage;
        //     this.refresh(false, this.pageIndex);
        //   }
        // }
      }
      // }
    },

    // onPageChange(page, pageSize) {
    //   this.pageIndex = page;
    //   this.queryParams.maxResultCount = pageSize;
    //   if (page !== 0) {
    //     this.refresh(false);
    //   }
    // },

    //编辑按钮
    edit(key) {
      const newData = [...this.dataSource];
      const target = newData.filter(item => key === item.id)[0];
      this.editingKey = key;
      if (target) {
        target.editable = true;
        this.dataSource = newData;
      }
    },

    async save(key) {
      const newData = [...this.dataSource];
      const newCacheData = [...this.cacheDataSource];
      const target = newData.filter(item => key === item.id)[0];
      const targetCache = newCacheData.filter(item => key === item.id)[0];
      if (target && targetCache) {
        delete target.editable;
        this.dataSource = newData;
        this.cacheDataSource = newCacheData;
        Object.assign(targetCache, target);
      }
      this.editingKey = '';
    },
  },

  render() {
    return (
      <div>
        <SmStdBasicMvdPropertyTreeSelect
          axios={this.axios}
          style="padding-bottom:10px"
          placeholder={'请选择信息交换模板属性'}
          treeCheckable={true}
          maxTagCount={5}
          allowClear={true}
          value={this.ids}
          onChange={item => {
            this.ids = item;
            console.log('select changed');
            this.refresh();
          }}
        />
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          pagination={false}
          scroll={{ y: 400 }}
          rowKey={record => record.id}
          bordered={this.bordered}
          onOk={this.ok}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                // let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return index + 1;
              },
              value: (text, record, index) => {
                return record.editable ? (
                  <a-input
                    style="margin: -10px 0"
                    value={record.value}
                    onChange={event => (record.value = event.target.value)}
                  />
                ) : (
                  record.value
                );
              },
              operations: (text, record, index) => {
                let result;
                if (!record.editable)
                  result = (
                    <span disabled={this.editingKey !== ''}>
                      <a onClick={() => this.edit(record.id)}>编辑</a>
                      <a-divider type="vertical" />
                      <a
                        onClick={() =>
                          this.remove(record.mvdPropertyId ? record.mvdPropertyId : record.id)
                        }
                      >
                        删除
                      </a>
                    </span>
                  );
                else
                  result = (
                    <span>
                      <a onClick={() => this.save(record.id)}>保存</a>
                      <a-divider type="vertical" />
                      <a
                        onClick={() =>
                          this.remove(record.mvdPropertyId ? record.mvdPropertyId : record.id)
                        }
                      >
                        删除
                      </a>
                    </span>
                  );
                return result;
              },
            },
          }}
        ></a-table>
        <div style="margin-top: 10px;">
          <a-button type="primary" onClick={() => this.ok()}>
            保存
          </a-button>
          <a-button
            style="margin-left:20px"
            onClick={() => {
              this.cancel();
            }}
          >
            取消
          </a-button>
        </div>
      </div>
    );
  },
};
