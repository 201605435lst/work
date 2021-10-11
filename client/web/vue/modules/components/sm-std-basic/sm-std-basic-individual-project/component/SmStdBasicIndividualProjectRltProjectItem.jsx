import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import ApiProjectItem from '../../../sm-api/sm-std-basic/ProjectItem';
import ApiProjectItemRltIndividualProject from '../../../sm-api/sm-std-basic/ProjectItemRltIndividualProject';
import SmStdBasicProjectItemListSelect from '../../sm-std-basic-project-item-list-select';
import { requestIsSuccess } from '../../../_utils/utils';

let apiProjectItem = new ApiProjectItem();
let apiProjectItemRltIndividualProject = new ApiProjectItemRltIndividualProject();
export default {
  name: 'SmStdBasicIndividualProjectRltProjectItem',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Object, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      projectItemIds: null,
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },

      individualProjectId: null,
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
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        {
          title: '编码',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
          ellipsis: true,
        },
        {
          title: '备注',
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
          ellipsis: true,
        },
        {
          title: '操作',
          width:80,
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
          this.individualProjectId = this.datas.id;
          this.form = this.$form.createForm(this, {});
          this.initAxios();
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
      apiProjectItem = new ApiProjectItem(this.axios);
      apiProjectItemRltIndividualProject = new ApiProjectItemRltIndividualProject(this.axios);
      this.projectItemIds = null;
      this.dataSource = null;
      let response = await apiProjectItemRltIndividualProject.getListByProjectItemId({
        id: this.datas ? this.datas.id : undefined,
        isAll: true,
      });
      if (requestIsSuccess(response) && response.data.items)
        this.projectItemIds = response.data.items.map(x => x.projectItemId); //传入组件的Ids
      this.projectItemIds = [...new Set(this.projectItemIds)];
      console.log(this.projectItemIds );
      this.refresh();
    },

    async refresh() {
      // if (resetPage) {
      //   this.pageIndex = 1;
      //   this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      // }
      this.dataSource=null;
      if (this.projectItemIds.length == 0) {
        this.dataSource = null;
        this.totalCount = 0;
        return;
      }

      let data = {
        ids: this.projectItemIds,
        isAll: true,
        // skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      let response = await apiProjectItem.getList(data);
      if (requestIsSuccess(response) && response.data.items) {
        this.dataSource=[];
        response.data.items.map(item=>
        {
          if(this.projectItemIds.indexOf(item.id) > -1)
          {

            this.dataSource.push(item);

          }
        });
        // this.dataSource = [...new Set(this.dataSource)];
        // console.log(this.dataSource );
        // this.dataSource = response.data.items.filter;
        // this.totalCount = this.dataSource.length;
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
    },

    // async refresh(resetPage = true, page) {
    //   if (resetPage) {
    //     this.pageIndex = 1;
    //     this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
    //   }
    //   this.dataSource=[];
    //   if (this.projectItemIds.length == 0) {
    //     this.dataSource = null;
    //     this.totalCount = 0;
    //     return;
    //   }
    //   let data = {
    //     ids: this.projectItemIds,
    //     skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
    //     ...this.queryParams,
    //   };
    //   let response = await apiProjectItem.getList(data);
    //   if (requestIsSuccess(response) && response.data.totalCount > 0) {
    //     response.data.items.map(item=>
    //     {
    //       if(this.projectItemIds.indexOf(item.id) > -1)
    //       {

    //         this.dataSource.push(item);

    //       }
    //     });
    //     this.dataSource = [...new Set(this.dataSource)];

    //     // this.dataSource = response.data.items.filter;
    //     this.totalCount = this.dataSource.length;
    //     if (page && this.totalCount && this.queryParams.maxResultCount) {
    //       let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
    //       if (this.totalCount % this.queryParams.maxResultCount !== 0) {
    //         currentPage = page + 1;
    //       }
    //       if (page > currentPage) {
    //         this.pageIndex = currentPage;
    //         this.refresh(false, this.pageIndex);
    //       }
    //     }
    //   }
    // },

    // onPageChange(page, pageSize) {
    //   this.pageIndex = page;
    //   this.queryParams.maxResultCount = pageSize;
    //   if (page !== 0) {
    //     this.refresh(false);
    //   }
    // },

    async save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiProjectItemRltIndividualProject.editList({
            individualProjectId: this.individualProjectId,
            projectItemIdList: this.projectItemIds,
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
      this.projectItemIds = this.projectItemIds.filter(x => x != record.id);
      this.refresh();
    },
  },
  render() {
    return (
      <div>
        <SmStdBasicProjectItemListSelect
          ref="SmStdBasicProjectItemListSelect"
          style="margin-bottom:10px"
          axios={this.axios}
          treeCheckable={true}
          treeCheckStrictly={true}
          maxTagCount={5}
          showSearch={true}
          value={this.projectItemIds}
          placeholder={'请选择关联的工程工项'}
          onChange={value => {
            this.projectItemIds = value;
            this.refresh();
          }}
        />

        <a-table
          columns={this.columns}
          // visible={true}
          style="border-collapse: collapse;display:block;overflow: auto;height:465px;"
          dataSource={this.dataSource}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 ;
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

        {/* <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        /> */}
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
