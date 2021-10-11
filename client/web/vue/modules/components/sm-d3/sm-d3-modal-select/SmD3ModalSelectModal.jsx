import './style/index.less';
import SmD3 from '../sm-d3/SmD3';
import signalr from '@/utils/signalr';
export default {
  name: 'SmD3ModalSelectModal',
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
    advancedCount: { type: Number, default: 6 },
  },
  data() {
    return {
      selectedValue: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedValue;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedValue = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedValue = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedValue);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedValue = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1200}
        title="模型选择"
        class="sm-d3-modal-select-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'environment'} />
                <span class="selected-name"> {item ? item.name : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.selectedValue = this.selectedValue.filter(_item => _item.id !== item.id);
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
        <div class="selectedValue-list">
          <SmD3
            axios={this.axios}
            signalr={signalr}
            snEarthProjectUrl={this.snEarthProjectUrl}
            globalTerrainUrl='//172.16.1.12:8165/terrain/World'
            globalImageryUrl='//172.16.1.12:8165/imagery/World'
            select={true}
            selectedEquipments={this.selectedValue}
            onSelectedEquipmentsChange={e => {
              this.selectedValue = e;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
