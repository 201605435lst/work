import './style/index.less';
import SmMaterialInventory from '../sm-material-inventory';

export default {
  name: 'SmMaterialInventoryModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    placeholder: { type: String, default: '请点击选择' },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
    partitionId: { type: String, default: null },//仓库id
    size: { type: String, default: 'default' },
  },
  data() {
    return {
      selectedList: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedList;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedList = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedList = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedList);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedList = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="库存物资选择"
        class="sm-material-inventory-modal"
        destroyOnClose={true}
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type='gold' />
                <a-tooltip placement="topLeft" title={item && item.material ? `${item.material.name}(${item.amount})` : null} >
                  <span class="selected-name"> {item && item.material ? `${item.material.name}(${item.amount})` : null} </span>
                </a-tooltip>

                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedList = this.selectedList.filter(_item => _item.id !== item.id);
                  }}
                >
                  <a-icon type="close" />
                </span>
              </div>;
            })

          ) : (
            <span style="margin-left:10px;">请选择</span>
          )}
        </div>
        <div class="sm-material-inventory-list">
          <SmMaterialInventory
            partitionId={this.partitionId}
            axios={this.axios}
            isSimple={true}
            multiple={this.multiple}
            selected={this.selectedList}
            size={this.size}
            onChange={selected => {
              this.selectedList = [];
              this.selectedList = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
