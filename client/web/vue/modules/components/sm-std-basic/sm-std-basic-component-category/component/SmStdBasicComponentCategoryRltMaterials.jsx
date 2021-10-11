import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import ApiComputerCode from '../../../sm-api/sm-std-basic/ComputerCode';
import ApiComponentCategoryRltMaterial from '../../../sm-api/sm-std-basic/ComponentCategoryRltMaterial';
import SmStdBasicComputerCodeSelect from '../../sm-std-basic-computer-code-select/SmStdBasicComputerCodeSelect';
import { requestIsSuccess } from '../../../_utils/utils';

let apiComputerCode = new ApiComputerCode();
let apiComponentCategoryRltMaterial = new ApiComponentCategoryRltMaterial();
export default {
  name: 'SmStdBasicComponentCategoryRltMaterials',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Object, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      computerCodes: null,
      computerType: 3,
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },

      categoryId: null,
      form: {}, // 表单
      action: null,
    };
  },

  computed: {
    //关联工程工项
    columns() {
      return [
        {
          title: '序号',
          width: '10%',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '名称及规格',
          width: '20%',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        {
          title: '电算代号',
          width: '20%',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
          ellipsis: true,
        },
        {
          title: '单位',
          width: '15%',
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
          ellipsis: true,
        },
        {
          title: '单位重量',
          width: '15%',
          dataIndex: 'weight',
          scopedSlots: { customRender: 'weight' },
          ellipsis: true,
        },
        {
          title: '操作',
          width: '20%',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  watch: {
    datas: {
      handler: function(val, oldVal) {
        if (this.datas != null) {
          this.categoryId = this.datas.id;
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
  },
  methods: {
    async initAxios() {
      apiComputerCode = new ApiComputerCode(this.axios);
      apiComponentCategoryRltMaterial = new ApiComponentCategoryRltMaterial(this.axios);
      this.computerCodes = null;
      this.dataSource = null;
      let response = await apiComponentCategoryRltMaterial.getListByComponentCategoryId({
        id: this.datas ? this.datas.id : undefined,
        isAll: true,
      });

      if (requestIsSuccess(response) && response.data.items) {
        this.computerCodes = response.data.items.map(x => x.computerCodeId); //传入组件的Ids
      }
      this.refresh();
    },

    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        type: this.computerType,
        ids: this.computerCodes,
        isRltMaterial: true,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      if (this.computerCodes == null) {
        this.dataSource = null;
        this.totalCount = 0;
        return;
      }
      let response = await apiComputerCode.getList(data);
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
    },

    onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    async save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiComponentCategoryRltMaterial.editList({
            componentCategoryId: this.categoryId,
            computerCodeIdList: this.computerCodes,
          });
          if (requestIsSuccess(response)) this.$message.success('保存成功');
          this.form.resetFields();
        }
      });
    },
    //取消
    cancel() {
      this.initAxios();
    },
    delete(record) {
      this.computerCodes = this.computerCodes.filter(x => x != record.id);
      this.refresh();
    },
  },
  render() {
    return (
      <div>
        <SmStdBasicComputerCodeSelect
          ref="SmStdBasicComputerCodeSelect"
          style={'margin-bottom:10px'}
          axios={this.axios}
          allowClear
          showSearch={true}
          treeCheckable={true}
          disabled={this.isShow}
          value={this.computerCodes}
          placeholder={'请选择关联的材料'}
          computerType={this.computerType}
          onChange={value => {
            this.computerCodes = value;
            this.refresh(false);
          }}
        />
        <a-table
          columns={this.columns}
          visible={true}
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
                      {' '}
                      删除
                    </a>
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
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
        <div style="margin-top: 10px;">
          <a-button type="primary" onClick={() => this.save()}>
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
