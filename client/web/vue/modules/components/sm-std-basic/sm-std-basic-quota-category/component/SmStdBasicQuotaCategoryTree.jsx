import { tips as tipsConfig } from '../../../_utils/config';
import ApiQuotaCategory from '../../../sm-api/sm-std-basic/QuotaCategory';
import SmStdBasicQuotaCategoryModel from './SmStdBasicQuotaCategoryModel';
import { requestIsSuccess, vP, vIf } from '../../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../../_utils/tree_array_tools';
import permissionsSmStdBasic from '../../../_permissions/sm-std-basic';
import SmImport from '../../../sm-import/sm-import-basic';
import SmTemplateDownload from '../../../sm-common/sm-import-template-module';
import SmExport from '../../../sm-common/sm-export-module';
import '../style/index';

let apiQuotaCategory = new ApiQuotaCategory();
export default {
  name: 'SmStdBasicQuotaCategoryTree',
  props: {
    axios: { type: Function, default: null },
    placeholder: { type: String, default: '请选择' },
    allowClear: { type: Boolean, default: true }, //是否清除
    transferData: { type: Object, default: null }, //下级分类中传过来的数据
    editData: { type: Object, default: null }, //基本信息表传过来的数据
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      quotaCategorys: [], // 列表数据源
      searchValue: '',
      autoExpandParent: true,
      record: null, //当前树节点的记录
      parentId: null, //当前记录的父节点
      fileList: [],
      expandedKeys: [], //展开的树节点
      exportData: undefined, //按需导出时的查询条件
    };
  },

  computed: {},
  watch: {
    transferData: {
      handler: function(val, oldVal) {
        this.refreshByParentId(this.transferData);
      },
      deep: true,
    },
    editData: {
      handler: function(val, oldVal) {
        this.refresh(this.editData);
      },
      deep: true,
    },
    exportData: {
      handler: function(val, oldVal) {
        this.exportData = val;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.loadByParentId();
  },

  methods: {
    initAxios() {
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
    },

    //文件导出
    async downloadFile(para) {
      let queryParams = {};
      queryParams = {
        parentId: null,
        keyWords: this.exportData ? (this.exportData.keyWords ? this.exportData.keyWords : '') : '',
      };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },

    //初始化页面加载数据
    async loadByParentId(data) {
      this.expandedKeys = [];
      let response = await apiQuotaCategory.getList({
        parentId: null,
        ids: [],
        keyWords: data ? (data.keyWords ? data.keyWords : '') : '',
        isAll: true,
      });
      if (requestIsSuccess(response) && response.data && response.data.items) {
        let _quotaCategorys = treeArrayItemAddProps(response.data.items, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
          { sourceProp: 'id', targetProp: 'key' },
          {
            targetProp: 'isLeaf',
            handler: item => {
              return item.children === null ? true : false;
            },
          },
          {
            targetProp: 'scopedSlots',
            handler: item => {
              return { title: 'title' };
            },
          },
        ]);
        this.quotaCategorys = _quotaCategorys;
      }
    },

    //异步加载数据
    async onLoadData(treeNode) {
      if (
        treeNode &&
        treeNode.dataRef &&
        treeNode.dataRef.children &&
        treeNode.dataRef.children.length == 0
      ) {
        let response = await apiQuotaCategory.getList({
          parentId: treeNode.dataRef.value,
          ids: [],
          isAll: true,
        });
        if (requestIsSuccess(response) && response.data && response.data.items) {
          treeNode.dataRef.children = treeArrayItemAddProps(response.data.items, 'children', [
            { sourceProp: 'name', targetProp: 'title' },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
            {
              targetProp: 'isLeaf',
              handler: item => {
                return item.children === null ? true : false;
              },
            },
            {
              targetProp: 'scopedSlots',
              handler: item => {
                return { title: 'title' };
              },
            },
          ]);
        }
      }
    },
    // 添加
    add(record) {
      this.record = record;
      this.$refs.SmStdBasicQuotaCategoryModel.add(record);
    },
    // 查看
    view(record) {
      this.$emit('record', record);
    },
    //编辑
    edit(record) {
      this.record = record;
      this.$refs.SmStdBasicQuotaCategoryModel.edit(record);
    },
    // 删除
    remove(record) {
      this.record = record;
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiQuotaCategory.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    // 树展开事件
    onExpand(expandedKeys, { expanded, node }) {
      this.expandedKeys = expandedKeys;
      this.autoExpandParent = false;
      this.onLoadData(node);
    },
    // 页面刷新
    async refresh(data) {
      //修改数据时候的页面刷新
      if (data != undefined) {
        if (data == 'Add') {
          // 给树的根节点添加子节点
          if (this.record != null) {
            //分两种情况：1、当前树已经存在子级。
            if (this.record && this.record.dataRef && this.record.dataRef.children) {
              this.record.dataRef.children = [];
            }
            // 2、当前树不存在子级
            if (this.record && this.record.children == null) {
              this.putQuotaCategoriesChildren(this.quotaCategorys);
            }
            this.onLoadData(this.record);
          } else {
            this.expandedKeys = [];
            this.loadByParentId();
          }
        } else {
          // 查询当前记录,进行修改
          let response = await apiQuotaCategory.get(data.id);
          if (requestIsSuccess(response) && response.data) {
            let dataResult = response.data;
            await this.editTreeData(this.quotaCategorys, dataResult);
          }
        }
      }
      //删除数据之后的页面刷新
      if (data == undefined) {
        this.getQuotaCategories(this.quotaCategorys);
        this.delateQuotaCategoriesChildren(this.quotaCategorys, this.parentId);
        this.$emit('record', null);
      }
    },
    async refreshByParentId(data) {
      this.getRecordByParentId(this.quotaCategorys, data);
      let valueSourse = {};
      if (this.getRecord) {
        valueSourse.dataRef = this.getRecord;
        this.onLoadData(valueSourse);
      }
    },
    //修改树结构的数据
    editTreeData(array, data) {
      try {
        array.forEach((item, index, arr) => {
          if (array.some(item => item.id == data.id)) {
            array.map(item => {
              if (item.id == data.id) {
                item.specialtyId = data.specialtyId;
                item.name = data.name;
                item.code = data.code;
                item.standardCodeId = data.standardCodeId;
                item.content = data.content;
                item.parentId = data.parentId;
                item.areaId = data.areaId;
              }
            });
            throw new Error('error');
          }

          if (item && item.children != null) {
            this.editTreeData(item.children, data);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    //根据传过来的parentId找到当前记录
    getRecordByParentId(array, data) {
      try {
        array.forEach((item, index, arr) => {
          if (item && data && item.id == data.parentId) {
            this.getRecord = item;
            //判断下面是否有子节点
            if (data && data.length == 0) {
              item.children = null;
              item.isLeaf = true;
            } else {
              item.children = [];
              item.isLeaf = false;
            }
            throw new Error('error');
          }
          if (item && item.children != null) {
            this.getRecordByParentId(item.children, data);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    // 删除当前记录（循环数组中的数组）
    getQuotaCategories(array) {
      try {
        array.forEach((item, index, arr) => {
          if (item.key == this.record.key) {
            arr.splice(index, 1);
            this.parentId = item.parentId;
            throw new Error('error');
          }
          if (item.children != null) {
            this.getQuotaCategories(item.children);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    // 给添加的根节点的树添加一个孩子节点
    putQuotaCategoriesChildren(array) {
      try {
        array.forEach((item, index, arr) => {
          if (item.key == this.record.key) {
            item.children = [];
            item.isLeaf = false;
            throw new Error('error');
          }
          if (item.children != null) {
            this.putQuotaCategoriesChildren(item.children);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    //当删除数据后。若无孩子节点，需要删除children和isleaf状态
    delateQuotaCategoriesChildren(array, id) {
      try {
        array.forEach((item, index, arr) => {
          if (item.id == id) {
            if (item.children != null && item.children.length < 1) {
              item.children = null;
              item.isLeaf = true;
            }
            throw new Error('error');
          }
          if (item.children != null) {
            this.delateQuotaCategoriesChildren(item.children, id);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'quotaCategory',
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },

    // 搜索事件
    async onSearch(value) {
      this.expandedKeys = [];
      if (!value) {
        await this.loadByParentId();
        this.exportData = undefined;
      } else {
        let data = {
          keyWords: value,
        };
        this.exportData = data;
        await this.loadByParentId(data);
      }
      this.$emit('record', null);
    },
  },
  render() {
    return (
      <div class="sm-std-basic-quota-category-tree">
        <a-card title="分类列表" class="tree-card">
          <a
            slot="extra"
            onClick={() => {
              this.expandedKeys = [];
            }}
            style="margin-right:20px"
          >
            <a-tooltip placement="top" title="收起">
              <si-top size={26} />{' '}
            </a-tooltip>
          </a>
          {vIf(
            <a
              slot="extra"
              onClick={() => {
                this.add();
              }}
            >
              <a-tooltip placement="top" title="添加">
                <si-add-select size={22} />{' '}
              </a-tooltip>
            </a>,
            vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Create),
          )}

          <div class="tree-body">
            <div class="std-tree">
              <div class="std-tree-body">
                <a-tree
                  ref="treeAdd"
                  expandedKeys={this.expandedKeys}
                  // blockNode
                  autoExpandParent={this.autoExpandParent}
                  treeData={this.quotaCategorys}
                  onExpand={this.onExpand}
                  loadData={this.onLoadData}
                  {...{
                    scopedSlots: {
                      title: record => {
                        let title = record.name;
                        return [
                          <div
                            class="categoryIcon"
                            onClick={() => {
                              this.view(record);
                            }}
                          >
                            <a-tooltip placement="topLeft" title={title}>
                              <span style="width:140px; overflow: hidden; text-overflow: ellipsis;">
                                {title}
                              </span>
                            </a-tooltip>
                            <span class="icons">
                              {vIf(
                                <span
                                  class="icon"
                                  onClick={() => {
                                    this.add(record);
                                  }}
                                >
                                  <a-tooltip placement="top" title="添加">
                                    <si-add-select />
                                  </a-tooltip>
                                </span>,
                                vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Create),
                              )}

                              {vIf(
                                <span
                                  class="icon"
                                  onClick={() => {
                                    this.edit(record);
                                  }}
                                >
                                  <a-tooltip placement="top" title="修改">
                                    <si-editor />
                                  </a-tooltip>
                                </span>,
                                vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Update),
                              )}

                              {vIf(
                                <span
                                  class="icon"
                                  onClick={() => {
                                    this.remove(record);
                                  }}
                                >
                                  <a-tooltip placement="top" title="删除">
                                    <si-ashbin />
                                  </a-tooltip>
                                </span>,
                                vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Delete),
                              )}
                            </span>
                          </div>,
                        ];
                      },
                    },
                  }}
                ></a-tree>
              </div>
              <div class="std-tree-search">
                <a-input-search
                  placeholder="请输入定额分类名称或编码"
                  enter-button
                  onSearch={item => this.onSearch(item)}
                />
              </div>

              <div class="importOrExport">
                {/* <a-button onClick={() => this.loadByParentId()}>刷新</a-button> */}
                {vIf(
                  <div class="import">
                    <SmImport
                      ref="smImport"
                      url="api/app/stdBasicQuotaCategory/upload"
                      axios={this.axios}
                      downloadErrorFile={true}
                      importKey="quotaCategory"
                      onSelected={file => this.fileSelected(file)}
                      onIsSuccess={() => this.loadByParentId()}
                    />
                  </div>,
                  vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Import),
                )}
                {vIf(
                  <SmExport
                    ref="smExport"
                    url="api/app/stdBasicQuotaCategory/export"
                    defaultTitle="导出"
                    axios={this.axios}
                    templateName="quotaCategory"
                    downloadFileName="定额分类"
                    rowIndex={5}
                    onDownload={para => this.downloadFile(para)}
                  ></SmExport>,
                  vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Export),
                )}
              </div>
              {vIf(
                <div class="import">
                  <SmTemplateDownload
                    axios={this.axios}
                    width="290px"
                    downloadKey="quotaCategory"
                    downloadFileName="定额分类"
                  ></SmTemplateDownload>
                  ,
                </div>,
                vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Import),
              )}
            </div>
          </div>
        </a-card>

        {/* 添加/编辑模板 */}
        <SmStdBasicQuotaCategoryModel
          ref="SmStdBasicQuotaCategoryModel"
          axios={this.axios}
          onSuccess={data => {
            this.refresh(data);
          }}
        />
      </div>
    );
  },
};
