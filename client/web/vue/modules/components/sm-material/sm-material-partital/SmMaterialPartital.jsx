/**
 * 说明：料库管理-分区管理
 * 作者：easten
 */
import './style';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiPartition from '../../sm-api/sm-material/Partition';
import PartitionModal from './src/PartitionModal';
import PicturePosition from './src/PicturePosition';
import SmMapControl from '../../sm-common/sm-map-control';
let apiPartition = new ApiPartition();

export default {
  name: 'SmMaterialPartital',
  props: {
    axios: { type: Function, default: null },
    height: { type: Number, default: 500 },
  },
  data() {
    return {
      partial: [],
      selected: {},
    };
  },
  computed: {
    partialData() {
      return this.partial.length == 0 ? [{ name: '请添加分区', id: '00' }] : this.partial;
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiPartition = new ApiPartition(this.axios);
    },
    async refresh() {
      let response = await apiPartition.getTreeList();
      if (requestIsSuccess(response)) {
        this.partial = response.data;
        // this.selected = {};
      }
    },
    partialSelect(keys, evt) {
      this.selected = evt.selectedNodes.length > 0 ? evt.selectedNodes[0].data.props.dataRef : {};
    },
    groupAdd() {
      this.$refs.PartitionModal.add(this.selected);
    },
    groupEdit() {
      this.$refs.PartitionModal.edit(this.selected);
    },
    groupDelete() {
      let _this = this;
      if (this.selected.id === null || this.selected.id == undefined) {
        this.$message.warn('请选择需要删除的记录');
        return;
      }
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiPartition.delete(_this.selected.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    getParentFileUrl(topId) {
      if (this.partial.length > 0) {
        let tops = this.partial.filter(a => a.parentId === null);
        let top = tops.find(a => a.id === topId);
        if (top != null && top.file != null) {
          return getFileUrl(top.file.url);
        }
      } return null;
    },
  },
  render() {
    return (
      <div class="sm-partital flex-row">
        <div class="left flex-1">
          <div class="head flex-row flex-item-between">
            <span>位置分区</span>
            <div class="tool">
              <span title="添加分组" onClick={() => this.groupAdd()}>
                <a-icon type="plus" />
              </span>
              <span title="编辑分组" onClick={() => this.groupEdit()}>
                <a-icon type="edit" />
              </span>
              <span title="删除分组" onClick={() => this.groupDelete()}>
                <a-icon type="minus" />
              </span>
            </div>
          </div>
          <div class="body">
            <a-tree
              tree-data={this.partialData}
              onSelect={this.partialSelect}
              defaultSelectedKeys={['0']}
              replaceFields={{ title: 'name', key: 'id' }}
              showIcon
              autoExpandParent
              defaultExpandAll
              defaultExpandParent
            >
              <a-icon slot="group" type="cluster" />
              <a-icon slot="node" type="audit" />
            </a-tree>
          </div>
        </div>
        <div class="right flex-col flex-4">
          <div class="head flex-row">
            <span>分区信息</span>
          </div>
          <div class="body flex-col">
            <div class="title flex-col">
              <span>分区名称：{this.selected.name}</span>
              <span>分区描述: {this.selected.description}</span>
            </div>
            <div class="content flex-1">
              <SmMapControl
                mode="view"
                height={this.height}
                zoom={13}
                position={[this.selected.x, this.selected.y]}
                showPoint={true}
                visible={this.selected.type === 0}
                singlePoint={true}
              />
              <PicturePosition position={[this.selected.x, this.selected.y]} view={true} height={this.height} visible={this.selected.type === 1} src={this.getParentFileUrl(this.selected.topId)} />
            </div>
          </div>
        </div>
        <PartitionModal src={this.getParentFileUrl(this.selected.topId)} axios={this.axios} ref="PartitionModal" onSuccess={() => this.refresh()} />
      </div>
    );
  },
};
