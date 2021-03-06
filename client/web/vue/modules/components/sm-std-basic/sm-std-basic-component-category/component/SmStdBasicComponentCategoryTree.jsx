import { tips as tipsConfig } from '../../../_utils/config';
import ApiComponentCategory from '../../../sm-api/sm-std-basic/ComponentCategory';
import SmStdBasicComponentCategoryModel from './SmStdBasicComponentCategoryModel';
import { requestIsSuccess, vIf, vP } from '../../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../../_utils/tree_array_tools';
import '../style/index';
import permissionsSmStdBasic from '../../../_permissions/sm-std-basic';
import SmImport from '../../../sm-import/sm-import-basic';
import SmTemplateDownload from '../../../sm-common/sm-import-template-module';
import SmExport from '../../../sm-common/sm-export-module';
let apiComponentCategory = new ApiComponentCategory();
export default {
  name: 'SmStdBasicComponentCategoryTree',
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
      componentCategories: [], // 列表数据源
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
  mounted() {
    this.loadByParentId();
  },
  async created() {
    this.initAxios();
    this.loadByParentId();
  },

  methods: {
    initAxios() {
      apiComponentCategory = new ApiComponentCategory(this.axios);
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
      let response = await apiComponentCategory.getList({
        parentId: null,
        ids: [],
        keyWords: data ? (data.keyWords ? data.keyWords : '') : '',
        isAll: true,
      });
      if (requestIsSuccess(response) && response.data && response.data.items) {
        let _componentCategories = treeArrayItemAddProps(response.data.items, 'children', [
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
        this.componentCategories = _componentCategories;
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
        let response = await apiComponentCategory.getList({
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
    //根据关键字加载页面
    async loadById(data) {
      let response = await apiComponentCategory.getTreeListByKeyWord(data);
      if (requestIsSuccess(response) && response.data) {
        let _componentCategories = treeArrayItemAddProps(response.data, 'children', [
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
        this.componentCategories = _componentCategories;
      }
    },
    // 添加
    add(record) {
      this.record = record;
      this.$refs.SmStdBasicComponentCategoryModel.add(record);
    },
    // 查看
    view(record) {
      this.$emit('record', record);
    },
    //编辑
    edit(record) {
      this.record = record;
      this.$refs.SmStdBasicComponentCategoryModel.edit(record);
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
            let response = await apiComponentCategory.delete(record.id);
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
              this.putComponentCategoriesChildren(this.componentCategories);
            }
            this.onLoadData(this.record);
          } else {
            this.expandedKeys = [];
            this.loadByParentId();
          }
        } else {
          // 查询当前记录,进行修改
          let response = await apiComponentCategory.get(data.id);
          if (requestIsSuccess(response) && response.data) {
            let dataResult = response.data;
            await this.editTreeData(this.componentCategories, dataResult);
          }
        }
      }
      //删除数据之后的页面刷新
      if (data == undefined) {
        this.getComponentCategories(this.componentCategories);
        this.delateComponentCategoriesChildren(this.componentCategories, this.parentId);
        this.$emit('record', null);
      }
    },
    async refreshByParentId(data) {
      this.getRecordByParentId(this.componentCategories, data);
      let valueSourse = {};
      if (this.getRecord) {
        valueSourse.dataRef = this.getRecord;
        this.onLoadData(valueSourse);
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
    //修改树结构的数据
    editTreeData(array, data) {
      try {
        array.forEach((item, index, arr) => {
          if (array.some(item => item.id == data.id)) {
            array.map(item => {
              if (item.id == data.id) {
                item.code = data.code;
                item.name = data.name;
                item.extendCode = data.extendCode;
                item.extendName = data.extendName;
                item.levelName = data.levelName;
                item.parentId = data.parentId;
                item.componentCategoryRltProductCategories =
                  data.componentCategoryRltProductCategories;
                item.productCategoryId = data.productCategoryId;
                item.remark = data.remark;
                item.unit = data.unit;
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
    // 删除当前记录（循环数组中的数组）
    getComponentCategories(array) {
      try {
        array.forEach((item, index, arr) => {
          if (item.key == this.record.key) {
            arr.splice(index, 1);
            this.parentId = item.parentId;
            throw new Error('error');
          }
          if (item.children != null) {
            this.getComponentCategories(item.children);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    // 给添加的根节点的树添加一个孩子节点
    putComponentCategoriesChildren(array) {
      try {
        array.forEach((item, index, arr) => {
          if (item.key == this.record.key) {
            item.children = [];
            item.isLeaf = false;
            throw new Error('error');
          }
          if (item.children != null) {
            this.putComponentCategoriesChildren(item.children);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
    },
    //当删除数据后。若无孩子节点，需要删除children和isleaf状态
    delateComponentCategoriesChildren(array, id) {
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
            this.delateComponentCategoriesChildren(item.children, id);
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
        importKey: 'componentCategories',
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
      <div class="sm-std-basic-component-category-tree">
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
            vP(this.permissions, permissionsSmStdBasic.ComponentCategories.Create),
          )}
          <div class="tree-body">
            <div class="std-tree">
              <div class="std-tree-body">
                <a-tree
                  expandedKeys={this.expandedKeys}
                  // blockNode
                  autoExpandParent={this.autoExpandParent}
                  treeData={this.componentCategories}
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
                            <span class="title">{title}</span>
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
                                vP(
                                  this.permissions,
                                  permissionsSmStdBasic.ComponentCategories.Create,
                                ),
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
                                vP(
                                  this.permissions,
                                  permissionsSmStdBasic.ComponentCategories.Update,
                                ),
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
                                vP(
                                  this.permissions,
                                  permissionsSmStdBasic.ComponentCategories.Delete,
                                ),
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
                  placeholder="请输入构件名称或构件编码"
                  enter-button
                  onSearch={item => this.onSearch(item)}
                />
              </div>
              <div class="importOrExport">
                {vIf(
                  <div class="import">
                    <SmImport
                      ref="smImport"
                      url="api/app/stdBasicComponentCategory/upload"
                      axios={this.axios}
                      downloadErrorFile={true}
                      importKey="componentCategories"
                      onSelected={file => this.fileSelected(file)}
                      onIsSuccess={() => setTimeout(() => this.loadByParentId(), 800)}
                    />
                  </div>,
                  vP(this.permissions, permissionsSmStdBasic.ComponentCategories.Import),
                )}
                {vIf(
                  <SmExport
                    ref="smExport"
                    url="api/app/stdBasicComponentCategory/export"
                    defaultTitle="导出"
                    axios={this.axios}
                    templateName="componentCategory"
                    downloadFileName="构件分类"
                    rowIndex={2}
                    onDownload={para => this.downloadFile(para)}
                  ></SmExport>,
                  vP(this.permissions, permissionsSmStdBasic.ComponentCategories.Export),
                )}
              </div>
              {vIf(
                <SmTemplateDownload
                  width="290px"
                  axios={this.axios}
                  downloadKey="componentCategory"
                  downloadFileName="构件分类"
                ></SmTemplateDownload>,
                vP(this.permissions, permissionsSmStdBasic.ComponentCategories.Import),
              )}
            </div>
          </div>
        </a-card>

        {/* 添加/编辑模板 */}
        <SmStdBasicComponentCategoryModel
          ref="SmStdBasicComponentCategoryModel"
          axios={this.axios}
          onSuccess={data => {
            this.refresh(data);
          }}
        />
      </div>
    );
  },
};
