import { pagination as paginationConfig } from '../../_utils/config';
import ApiProjectItem from '../../sm-api/sm-std-basic/ProjectItem';
import ApiIndividualProject from '../../sm-api/sm-std-basic/IndividualProject';
import ApiProjectItemRltProcessTemplate from '../../sm-api/sm-std-basic/ProjectItemRltProcessTemplate';
import SmStdBasicProjectItemListSelect from '../sm-std-basic-project-item-list-select/SmStdBasicProjectItemListSelect';
import SmStdBasicIndividualProjectTreeSelect from '../sm-std-basic-individual-project-tree-select';
import { requestIsSuccess } from '../../_utils/utils';

let apiProjectItem = new ApiProjectItem();
let apiIndividualProject = new ApiIndividualProject();
let apiProjectItemRltProcessTemplate = new ApiProjectItemRltProcessTemplate();
export default {
  name: 'SmStdBasicProjectItemRltProcessTemplate',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Object, default: null }, // 用于传工序模板的信息
    value: { type: Array, default: () => [] }, //主要用于单独使用此控件时，用来传单项工程和工程工项的Ids
    useAlone: { type: Boolean, default: false }, // 是否为单独使用
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      pageIndex: 1,
      totalCount: 0,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWord: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },

      processTemplateId: null,
      projectItemIds: [], //选中的工程工项Id
      individualProjectIds: [], //选中的单项工程Id
      dataSource: null, //选中的工程工项
      type: 0, //关联工程工项或关联单项工程
      form: {}, // 表单
      action: null,
      iValue: [],
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
          title: '专业',
          dataIndex: 'specialtyName',
          scopedSlots: { customRender: 'specialtyName' },
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
          width: 80,
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
          this.processTemplateId = this.datas.id;
          this.form = this.$form.createForm(this, {});
          this.initAxios();
          this.type = 0;
          this.init();
        }
      },
      immediate: true, //当监听value时。不写immediate不行，写了就会报this.axios is undefined。
      deep: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
    this.type = 0;
    this.useAlone ? undefined : this.init();
    this.valueRefresh();
    // this.init();
  },
  methods: {
    initAxios() {
      apiProjectItem = new ApiProjectItem(this.axios);
      apiIndividualProject = new ApiIndividualProject(this.axios);
      apiProjectItemRltProcessTemplate = new ApiProjectItemRltProcessTemplate(this.axios);
    },

    async init() {
      // if (resetPage) {
      //   this.pageIndex = 1;
      //   this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      // }
      this.projectItemIds = null;
      this.individualProjectIds = null;
      this.dataSource = null;
      this.totalCount = 0;
      let response = await apiProjectItemRltProcessTemplate.getListByProcessTemplateId({
        id: this.datas.id,
        isAll: true,
        // skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        this.individualProjectIds = this.dataSource
          ? this.dataSource.filter(item => item.type == 0).length > 0
            ? this.dataSource.filter(item => item.type == 0).map(x => x.projectItemId)
            : ''
          : '';
        this.projectItemIds = this.dataSource
          ? this.dataSource.filter(item => item.type == 1).length > 0
            ? this.dataSource.filter(item => item.type == 1).map(x => x.projectItemId)
            : ''
          : '';
        this.dataSource.map(item => {
          item.children = null;
        });
        // if (page && this.totalCount && this.queryParams.maxResultCount) {
        //   let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
        //   if (this.totalCount % this.queryParams.maxResultCount !== 0) {
        //     currentPage = page + 1;
        //   }
        //   if (page > currentPage) {
        //     this.pageIndex = currentPage;
        //     this.init(false, this.pageIndex);
        //   }
        // }
      }
    },
    
    async refresh() {
      let data1 = {
        ids: this.projectItemIds ? this.projectItemIds : [], //从数据库中读出的ids
        isAll: true,
        ...this.queryParams,
      };

      let data = {
        ids: this.individualProjectIds ? this.individualProjectIds : [],
        isAll: true,
        ...this.queryParams,
      };
      let response = null;
      let response2 = null;
      response = await apiIndividualProject.getListIndividualProject(data);
      response2 = await apiProjectItem.getListProjectItem(data1);
      this.dataSource =[];
      let number=0;
      if (requestIsSuccess(response))
      {
        response.data.items.map(it=>
        {
          let editValue = {
            projectItemId: it.id,
            name: it.name,
            code: it.code,
            specialtyName:it.specialtyName,
            remark: it.remark,
          };
          this.dataSource.push(editValue);
          number+=1;
          // }
        });
      }
      if (requestIsSuccess(response2))
      {
        response2.data.items.map(it=>
        {
          let editValue = {
            projectItemId: it.id,
            name: it.name,
            code: it.code,
            specialtyName:it.specialtyName,
            remark: it.remark,
          };
          this.dataSource.push(editValue);
          number+=1;
        });
      }
      this.totalCount = number;
      this.dataSource.map(item => {
        item.children = null;
      });
    },

    // async refresh(resetPage = true, page) {
    //   if (resetPage) {
    //     this.pageIndex = 1;
    //     this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
    //   }

    //   let data1 = {
    //     ids: this.projectItemIds ? this.projectItemIds : [], //从数据库中读出的ids
    //     isAll: false,
    //     skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
    //     ...this.queryParams,
    //   };

    //   let data = {
    //     ids: this.individualProjectIds ? this.individualProjectIds : [],
    //     isAll: false,
    //     skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
    //     ...this.queryParams,
    //   };
    //   let response = null;
    //   let response2 = null;
    //   response = await apiIndividualProject.getListIndividualProject(data);
    //   response2 = await apiProjectItem.getListProjectItem(data1);

      
    //   // let responseAll = await apiProjectItemRltProcessTemplate.getListByProcessTemplateId({
    //   //   id: this.datas.id,
    //   //   isAll: false,
    //   //   skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
    //   //   ...this.queryParams,
    //   // });
      

    //   // if (requestIsSuccess(responseAll)) {
    //   this.dataSource =[];
    //   let number=0;
    //   if (requestIsSuccess(response))
    //   {
    //     response.data.items.map(it=>
    //     {
    //       // let befor= this.dataSource.filter(x => x.projectItemId == it.id);
    //       // console.log(befor);
    //       // if(befor.length==0)
    //       // {
    //       let editValue = {
    //         projectItemId: it.id,
    //         name: it.name,
    //         code: it.code,
    //         specialtyName:it.specialtyName,
    //         remark: it.remark,
    //       };
    //       this.dataSource.push(editValue);
    //       number+=1;
    //       // }
    //     });
    //   }
    //   if (requestIsSuccess(response2))
    //   {
    //     response2.data.items.map(it=>
    //     {
    //       // let befor= this.dataSource.filter(x => x.projectItemId == it.id);
    //       // if(befor==null||befor.length==0)
    //       // {
    //       let editValue = {
    //         projectItemId: it.id,
    //         name: it.name,
    //         code: it.code,
    //         specialtyName:it.specialtyName,
    //         remark: it.remark,
    //       };
    //       this.dataSource.push(editValue);
    //       number+=1;
    //       // }
    //     });
    //   }
    //   this.totalCount = number;
    //   this.dataSource.map(item => {
    //     item.children = null;
    //   });
    //   if (page && this.totalCount && this.queryParams.maxResultCount) {
    //     let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
    //     if (this.totalCount % this.queryParams.maxResultCount !== 0) {
    //       currentPage = page + 1;
    //     }
    //     if (page > currentPage) {
    //       this.pageIndex = currentPage;
    //       this.refresh(false, this.pageIndex);
    //     }
    //   }
    //   // }
    // },
    // onPageChange(page, pageSize) {
    //   this.pageIndex = page;
    //   this.queryParams.maxResultCount = pageSize;
    //   if (page !== 0) {
    //     this.refresh(false);
    //   }
    // },

    sendIds() {
      this.$emit('change', this.individualProjectIds, this.projectItemIds);
    },

    async valueRefresh(resetPage = true, page) {
      // if (resetPage) {
      //   this.pageIndex = 1;
      //   this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      // }
      let data = {
        ids: this.value ? this.value : [],
        isAll: true,
        // skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      this.dataSource=[];
      this.totalCount =0;
      let response = null;
      let response2 = null;
      // if (this.type == 0) 
      response = await apiIndividualProject.getListIndividualProject(data);
      // else
      response2 = await apiProjectItem.getListProjectItem(data);
      // response = await apiProjectItemRltProcessTemplate.getListByProcessTemplateId({
      //   id: this.datas.id,
      //   isAll: true,
      // });
      if (requestIsSuccess(response)) {
      
        let data = response.data.items;
        this.totalCount = response.data.totalCount;
        data.map(item => {
          item.children = null;
          this.dataSource.push(item);
          this.individualProjectIds.push(item.id);
         
        });
        // this.individualProjectIds = [...new Set(this.individualProjectIds)]; //数组去重
        // this.projectItemIds = [...new Set(this.projectItemIds)];
      }
      if (requestIsSuccess(response2)) {
      
        let data2 = response2.data.items;
        this.totalCount += response2.data.totalCount;
        data2 .map(item => {
          item.children = null;
          this.dataSource.push(item);
          this.projectItemIds.push(item.id);
        });
        
      }
      // if (page && this.totalCount && this.queryParams.maxResultCount) {
      //   let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
      //   if (this.totalCount % this.queryParams.maxResultCount !== 0) {
      //     currentPage = page + 1;
      //   }
      //   if (page > currentPage) {
      //     this.pageIndex = currentPage;
      //     this.valueRefresh(false, this.pageIndex);
      //   }
      // }
      this.individualProjectIds = [...new Set(this.individualProjectIds)]; //数组去重
      this.projectItemIds = [...new Set(this.projectItemIds)];
    },

    async save() {
      

      let data = this.individualProjectIds
        ? this.projectItemIds
          ? this.individualProjectIds.concat(this.projectItemIds)
          : this.individualProjectIds
        : this.projectItemIds;
      
      let response = await apiProjectItemRltProcessTemplate.editList({
        processTemplateId: this.datas.id,
        projectItemIdList: data,
      });
      if (requestIsSuccess(response)) {
        this.$message.success('保存成功');
        this.init();
      }
    },
    //取消
    cancel() {
      this.init();
    },
    delete(datas) {
  
      //并不会触发选择框的onChange事件
      // if (this.type == 0)
      this.individualProjectIds = this.individualProjectIds.filter(x => x != datas.projectItemId);
      // else 
      this.projectItemIds = this.projectItemIds.filter(x => x != datas.projectItemId);
     
      
      this.sendIds();
      this.refresh();
    },
  },
  render() {
    return (
      <div class="sm-std-basic-individual-project">
        <div>
          {/* {!this.datas ? this.$message.error('请选择工序模板') : undefined} */}
          <a-radio-group value={this.type} style="margin-bottom:10px">
            <a-radio
              value={0}
              onChange={() => {
                this.type = 0;
                // this.useAlone ? this.valueRefresh() : this.init();
              }}
            >
              关联单项工程
            </a-radio>
            <a-radio
              value={1}
              onChange={() => {
                this.type = 1;
                // this.useAlone ? this.valueRefresh() : this.init();
              }}
            >
              关联工程工项
            </a-radio>
          </a-radio-group>
          {this.type == 0 ? (
            <SmStdBasicIndividualProjectTreeSelect
              style="margin-bottom:10px"
              axios={this.axios}
              treeCheckable={true}
              treeCheckStrictly={true}
              maxTagCount={5}
              showSearch={true}
              value={this.individualProjectIds}
              placeholder={'请选择关联的单项工程'}
              onChange={value => {
                this.individualProjectIds = value;
                this.refresh();
                this.sendIds(); //change事件，主要用于别的模块单独使用时发送所选所有的IDs
              }}
            />
          ) : (
            <SmStdBasicProjectItemListSelect
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
                this.sendIds();
              }}
            />
          )}
          <a-table
            columns={this.columns}
            style="border-collapse: collapse;display:block;overflow: auto;height:465px;"
            dataSource={this.dataSource}
            rowKey={datas => datas.id}
            // height='600Px'
            pagination={false}
            {...{
              scopedSlots: {
                index: (text, datas, index) => {
                  let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                  return result;
                },
                operations: (text, datas, index) => {
                  return [
                    <span>
                      <a
                        onClick={() => {
                          this.delete(datas);
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
          <div style="diaplay:flex;">
            {/* <a-pagination
              style="margin-top: 10px;"
              total={this.totalCount}
              pageSize={this.queryParams.maxResultCount}
              current={this.pageIndex}
              onChange={this.onPageChange}
              onShowSizeChange={this.onPageChange}
              showSizeChanger
              showQuickJumper
              showTotal={paginationConfig.showTotal}
            /> */}
            {this.useAlone ? (
              undefined
            ) : (
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
            )}
          </div>
        </div>
      </div>
    );
  },
};
