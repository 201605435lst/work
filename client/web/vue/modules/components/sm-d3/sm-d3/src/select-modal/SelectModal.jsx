import SmD3 from '../../SmD3';
import signalr from '@/utils/signalr';

export default {
  name: 'SelectModal',
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
    isSelect: { type: Boolean, default: false },//是否选择模式
  },
  data() {
    return {
      selectedEquipments: null,
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedMaterialPlan;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedMaterialPlan = value;
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        this.selectedMaterialPlan = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedMaterialPlan);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedMaterialPlan = [];

    },
  },
  render() {
    return (
      <a-modal
        title='工程量信息'
        visible={this.iVisible}
        width='70%'
        onCancel={() => {
          this.d3ModalVisible = false;
        }}
        onOk={async () => {
          if (this.selectedEquipments.length === 0) return this.$message.warn('未选择设备');
          this.d3ModalVisible = false;
          await this.setPlanMaterial();
          await this.refresh();
        }}
      >
        <div
          style='
      height: 600px;
      display: flex;
      /* position: fixed;
      z-index: 100000000;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0; */
  '
        >
          <SmD3
            axios={this.axios}
            signalr={signalr}
            snEarthProjectUrl={this.snEarthProjectUrl}
            globalTerrainUrl='//172.16.1.12:8165/terrain/World'
            globalImageryUrl='//172.16.1.12:8165/imagery/World'
            select={true}
            selectedEquipments={this.selectedEquipments}
            onSelectedEquipmentsChange={e => {
              this.selectedEquipments = e;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
