import '../style/index';
import { requestIsSuccess, vIf, vP } from '../../../_utils/utils';
import { treeArrayItemAddProps } from '../../../_utils/tree_array_tools';
import SmProjectArchivesCatrgotyModal from '../../sm-project-archives/SmProjectArchivesCatrgotyModal';
import SmProjectDossierCatrgotyModal from '../SmProjectDossierCatrgotyModal';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import permissionsSmProject from '../../../_permissions/sm-project';
import * as utils from '../../../_utils/utils';
export default {
  name: 'ProjectCardTree',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    api: { type: Object, default: null },
    type: { type: String, default: null }, //模态框类型
    archivesCatrgotyId: { type: String, default: null }, //父级id
  },
  data() {
    return {
      loading: false,
      queryParams: {
        parentId: null,
      },
      expandedKeys: [], //展开的树节点
      iApi: null, //接口
      iType: null, //模态框类型
      dataSource: [],
    };
  },
  computed: {},
  watch: {
    api: {
      handler: function(val, oldVal) {
        if (this.api) {
          console.log("进来了");
          this.iApi = val;
          this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
    type: {
      handler: function(val, oldVal) {
        if (this.type) {
          this.iType = this.type;
        }
      },
      immediate: true,
      deep: true,
    },
    archivesCatrgotyId: {
      handler: function(val, oldVal) {
        this.queryParams.parentId = val;
        this.refresh();
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {},

    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await _this.iApi.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.record = null;
              _this.$emit('view', null);
              _this.refresh(false);

              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    async refresh() {
      let data = {
        ...this.queryParams,
      };
      this.expandedKeys = [];
      let response = await this.iApi.getList({ ...data });
      if (requestIsSuccess(response) && response.data) {
        let _dataSource = treeArrayItemAddProps(response.data, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
          {
            targetProp: 'isLeaf',
            handler: item => {
              return item.children && item.children instanceof Array ? false : true;
            },
          },
          {
            targetProp: 'scopedSlots',
            handler: item => {
              return { title: 'title' };
            },
          },
        ]);
        this.dataSource = _dataSource;
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
        let response = await this.iApi.getList({ parentId: treeNode.dataRef.value });
        if (requestIsSuccess(response) && response.data && response.data) {
          treeNode.dataRef.children = treeArrayItemAddProps(response.data, 'children', [
            { sourceProp: 'name', targetProp: 'title' },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
            {
              targetProp: 'isLeaf',
              handler: item => {
                return item.children && item.children instanceof Array ? false : true;
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
    add(record) {
      if (this.iType == 'ProjectArchivesCatrgoty') {
        this.$refs.SmProjectArchivesCatrgotyModal.add(record);
      }
      if (this.iType == 'ProjectDossierCatrgoty') {
        this.$refs.SmProjectDossierCatrgotyModal.add(record);
      }
    },
    view(record) {
      if (this.iType == 'ProjectArchivesCatrgoty') {
        this.$refs.SmProjectArchivesCatrgotyModal.view(record);
      }
      if (this.iType == 'ProjectDossierCatrgoty') {
        this.$refs.SmProjectDossierCatrgotyModal.view(record);
      }
    },
    edit(record) {
      if (this.iType == 'ProjectArchivesCatrgoty') {
        this.$refs.SmProjectArchivesCatrgotyModal.edit(record);
      }
      if (this.iType == 'ProjectDossierCatrgoty') {
        this.$refs.SmProjectDossierCatrgotyModal.edit(record);
      }
    },
    // 树展开事件
    onExpand(expandedKeys, { expanded, node }) {
      this.expandedKeys = expandedKeys;
      this.autoExpandParent = false;
      this.onLoadData(node);
    },
    async lock(record) {
      if (record) {
        let data = {
          isEncrypt: !record.isEncrypt,
        };
        let response = await this.iApi.lock({ id: record.id, ...data });
        if (utils.requestIsSuccess(response)) {
          this.$message.success('操作成功');
          this.refresh();
        }
      }
     
    },
  },
  render() {
    return (
      <div class="sm-project-card-tree">
        <div class={this.$slots.cardTreeSlot ? 'card-tree-select' : ''}>
          {this.$slots.cardTreeSlot ? this.$slots.cardTreeSlot : null}
        </div>
        <a-card
          title={this.iType == 'ProjectArchivesCatrgoty' ? '分类管理' : '卷宗分类'}
          class="card-tree"
        >
          <a slot="extra" onClick={() => this.add()}>
            {this.iType == 'ProjectArchivesCatrgoty'
              ? vIf(
                <a-tooltip placement="top" title="添加">
                  <si-add-select size={22} />{' '}
                </a-tooltip>,
                vP(this.permissions, permissionsSmProject.ArchivesCategorys.Create),
              )
              : ''}
          </a>
          <a-tree
            expandedKeys={this.expandedKeys}
            // autoExpandParent={this.autoExpandParent}
            treeData={this.dataSource}
            showIcon
            blockNode={this.iType == 'ProjectDossierCatrgoty' ? true : false}
            onExpand={this.onExpand}
            loadData={this.onLoadData}
            {...{
              scopedSlots: {
                title: record => {
                  let title = record.name;
                  return [
                    <div class="category-item" onClick={() => this.$emit('view', record)}>
                      <span class="item-icon">
                        <a-icon
                          type={
                            !record.isFile ? (record.isLeaf ? 'folder' : 'folder-open') : 'file'
                          }
                        />
                      </span>
                      <span class="title">{title}</span>
                      {this.iType == 'ProjectArchivesCatrgoty' ? (
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
                            vP(this.permissions, permissionsSmProject.ArchivesCategorys.Create),
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
                            vP(this.permissions, permissionsSmProject.ArchivesCategorys.Update),
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
                            vP(this.permissions, permissionsSmProject.ArchivesCategorys.Delete),
                          )}
                          {vIf(
                            <span
                              class="icon"
                              onClick={() => {
                                this.lock(record);
                              }}
                            >
                              <a-tooltip
                                disabledplacement="top"
                                title={record.isEncrypt ? '解密' : '加密'}
                              >
                                <a-icon type={!record.isEncrypt ? "unlock" : "lock"} style="font-size:18px;margin-bottom: 2px;"></a-icon>
                              </a-tooltip>
                            </span>,
                            vP(this.permissions, permissionsSmProject.ArchivesCategorys.Encrypt),
                          )}
                        </span>
                      ) : (
                        ''
                      )}
                    </div>,
                  ];
                },
              },
            }}
          />
          {/* <a-icon slot="switcherIcon" ></a-icon>
            <a-icon slot="folder" type="folder" />   //文件夹
            <a-icon slot="folder-open" type="folder-open" />  //文件夹打开
            <a-icon slot="file" type="file" />  //文件
          </a-tree> */}
        </a-card>
        {/* 档案   添加/编辑模板 */}
        <SmProjectArchivesCatrgotyModal
          ref="SmProjectArchivesCatrgotyModal"
          axios={this.axios}
          onSuccess={(action, data) => {
            this.refresh(false);
          }}
        />
        {/* 卷宗   添加/编辑模板 */}
        <SmProjectDossierCatrgotyModal
          ref="SmProjectDossierCatrgotyModal"
          axios={this.axios}
          onSuccess={(action, data) => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
