/**
 * 一个区域下的设备 scopeType  scopeId
 */

import { requestIsSuccess, parseScope, d3Path2Pair } from '../../_utils/utils';
import { treeArrayLoop, getTreeArrayParentsByKey } from '../../_utils/tree_array_tools';
import { ScopeType } from '../../_utils/enum';
import _ from 'lodash';

import ApiEquipment from '../../sm-api/sm-resource/Equipments';
let apiEquipment = new ApiEquipment();

import './style/index.less';

const NodeTypes = {
  ComponentCategory: 1,
  Equipment: 2,
};

export default {
  name: 'SmD3ScopeEquipments',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    multiple: { type: Boolean, default: true },
    scopeCode: { type: String, default: null }, // 设备范围码 1@Name@OrganizationId.2@Name@RailwayId.3@Name@StationId.4@Name@InstallationId
    value: { type: Array, default: () => [] }, // 选中的设备名称列表 {equipmentGroupName:'',equipmentName:''}
    hiddens: { type: Array, default: () => [] }, // 选中的设备名称列表 {equipmentGroupName:'',equipmentName:''}
    showHeader: { type: Boolean, default: true },
  },
  data() {
    return {
      iValue: [],
      treeData: [],
      flatData: [],
      checkedKeys: [],
      expandedKeys: [],
    };
  },

  computed: {
    selectedKeys() {
      let checkedItem = this.flatData.filter(
        item => this.iValue.indexOf(`${item.equipmentGroupName}@${item.name}`) > -1,
      );
      let checkedKeys = checkedItem.map(item => item.id);
      let halfChecked = [];

      for (let item of checkedItem) {
        halfChecked = [
          ...halfChecked,
          ...getTreeArrayParentsByKey(this.flatData, 'children', 'id', item.id).map(
            item => item.id,
          ),
        ];
      }

      return { checked: checkedKeys, halfChecked: halfChecked };
    },
    scope() {
      return parseScope(this.scopeCode);
    },
    title: function () {
      let title = this.scope ? this.scope.name : '';
      if (this.value.length) {
        title += `（${this.value.length}）`;
      }
      return title;
    },
  },

  watch: {
    value: {
      handler: async function (value, oldValue) {
        this.iValue = value;
        this.initAxios();
        if ((this.iValue == null || this.iValue.length === 0) && this.treeData.length == 0) {
          await this.refresh();
        } else {
          // 查询设备是否在列表中，如果不在，重新刷新
          for (let item of this.iValue) {
            let arr = item.split('@');
            if (!this.flatData.find(x => x.equipmentGroupName === arr[0] && x.name === arr[1])) {
              await this.refresh();
              break;
            }
          }
        }

        if ((value || []).join(".") != (oldValue || []).join(".")) {
          let equipments = [];
          value.map(path => {
            let arr = path.split("@");
            let pair = { groupName: arr[0], name: arr[1] };
            let equipment = this.flatData.find(x => x.equipmentGroupName && x.equipmentGroupName === pair.groupName && x.name === pair.name);
            if (equipment) {
              equipments.push({
                id: equipment.id,
                name: equipment.name,
                groupName: equipment.equipmentGroupName,
              });
            }
          });
          this.$emit('change', this.iValue, equipments);
        }
      },
      immediate: true,
    },
    scopeCode: {
      handler: function (value, oldValue) {
        this.initAxios();
        this.refresh();
        // this.iValue = [];
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.iValue = this.value;
    this.refresh();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipment = new ApiEquipment(this.axios);
    },

    async refresh(data) {
      if (!this.scopeCode) return;

      let node = data ? data.dataRef : undefined;

      return new Promise(async resolve => {
        if (node && node.children && node.children.length) {
          resolve();
          return;
        }
        let response = await apiEquipment.searchListByScopeCode({
          scopeCode: this.scopeCode,
          parentId: node ? node.id : undefined,
          type: node ? node.type : NodeTypes.ComponentCategory,
          InitialGroupAndNames: this.iValue, // {equipmentGroupName:'',equipmentName:''}
        });

        if (requestIsSuccess(response)) {
          let items = JSON.parse(JSON.stringify(response.data));
          let flatItems = [];

          treeArrayLoop(items, item => {
            item['key'] = item.id;
            item['title'] = item.name;
            item['class'] = 'equipment-tree-node';
            // item['disabled'] = !!this.flatData.find(x => x.parentId == item.id);
            item['scopedSlots'] = {
              title: 'title',
            };
            item['_data'] = {
              visible: true,
              blink: false,
            };

            if (item.type == NodeTypes.Equipment && !item.children) {
              item['isLeaf'] = true;
            }
            // if (item.type === NodeTypes.ComponentCategory) {
            //   item['disableCheckbox'] = true;
            // }
            flatItems.push(item);
          });

          // 记录平装数组

          if (node) {
            node.children = items;
            this.treeData = [...this.treeData];
            this.flatData = [...this.flatData, ...flatItems];
          } else {
            this.treeData = [];
            this.$nextTick(() => {
              this.treeData = [...items];
              this.flatData = [...flatItems];
            });
          }
        }

        resolve();
      });
    },
  },
  render() {
    let tree = (
      <a-tree
        treeData={this.treeData}
        loadData={this.refresh}
        multiple={this.multiple}
        checkedKeys={this.selectedKeys}
        checkStrictly={true}
        expandedKeys={this.expandedKeys}
        blockNode
        checkable
        selectable={false}
        onCheck={(data, event) => {
          let keys = data.checked;
          let groupAndNames = this.flatData
            .filter(x => keys.indexOf(x.id) > -1 && x.type === NodeTypes.Equipment)
            .map(x => `${x.equipmentGroupName}@${x.name}`);

          this.iValue = groupAndNames;

          let equipments = keys.map(key => {
            let equipment = this.flatData.find(item => item.id === key && item.type === NodeTypes.Equipment);
            return {
              id: key,
              name: equipment.name,
              groupName: equipment.equipmentGroupName,
            };
          });
          this.$emit('change', this.iValue, equipments);
        }}
        onExpand={keys => {
          this.expandedKeys = keys;
        }}
        {...{
          scopedSlots: {
            title: node => {
              let icon;
              let title;
              let timer = null;
              let hidden =
                this.hiddens.find(x => x === `${node.equipmentGroupName}@${node.name}`) != null;
              if (node.type === NodeTypes.ComponentCategory) {
                icon = (
                  <span class="icon">
                    <a-icon type="folder-open" />
                  </span>
                );
              }

              title = (
                <div
                  onDblclick={e => {
                    clearTimeout(timer);
                    if (node.type === NodeTypes.Equipment) {
                      this.$emit('flyTo', {
                        groupName: node.equipmentGroupName,
                        name: node.name,
                        equipment: node.dataRef,
                        selected: false,
                      });
                      let groupAndName = `${node.equipmentGroupName}@${node.name}`;
                      this.iValue = this.iValue.indexOf(groupAndName) > -1 ? [] : [groupAndName];
                      this.$emit('change', this.iValue);
                    }
                    e.stopPropagation();
                  }}
                  onClick={e => {
                    clearTimeout(timer);
                    timer = setTimeout(() => {
                      if (node.type === NodeTypes.Equipment) {
                        let groupAndName = `${node.equipmentGroupName}@${node.name}`;
                        this.iValue = this.iValue.indexOf(groupAndName) > -1 ? [] : [groupAndName];
                        this.$emit('change', this.iValue);
                      }
                      e.stopPropagation();
                    }, 200);
                    e.stopPropagation();
                  }}
                  class="title"
                >
                  <span class="btns">
                    {node.type === NodeTypes.Equipment
                      ? [
                        <span
                          class={{
                            btn: true,
                            'btn-fly': true,
                            blink: node._data.blink,
                          }}
                          title="定位"
                          onClick={e => {
                            if (node.type === NodeTypes.Equipment)
                              this.$emit('flyTo', {
                                groupName: node.equipmentGroupName,
                                name: node.name,
                                equipment: node.dataRef,
                                // selected: true,
                              });
                            e.stopPropagation();
                          }}
                        >
                          <a-icon type="environment" />
                        </span>,

                        // <span
                        //   class={{
                        //     btn: true,
                        //     'btn-visible': true,
                        //     hidden: hidden,
                        //   }}
                        //   title="显示"
                        //   onClick={e => {
                        //     this.$emit('visibleChange', {
                        //       groupName: node.equipmentGroupName,
                        //       name: node.name,
                        //     });
                        //     e.stopPropagation();
                        //   }}
                        // >
                        //   <a-icon type={!hidden ? 'eye' : 'eye-invisible'} />
                        // </span>,
                      ]
                      : undefined}
                  </span>
                  <span>{node.name}</span>
                </div>
              );

              return [icon, title];
            },
          },
        }}
      ></a-tree>
    );

    return (
      <sc-panel
        class="sm-d3-scope-equipments"
        flex="1"
        showHeader={this.showHeader}
        title={this.title}
        bordered={false}
        onClose={() => {
          this.$emit('close');
        }}
      >
        <a-icon slot="icon" type="profile" />
        {this.treeData.length ? (
          tree
        ) : (
          <span
            style="
              margin-top: 10px;
              color: #bdbdbd;"
          >
            无数据
          </span>
        )}
      </sc-panel>
    );
  },
};
