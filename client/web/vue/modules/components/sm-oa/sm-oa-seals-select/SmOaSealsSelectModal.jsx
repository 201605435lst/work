import './style/index.less';
import SmOaSeals from '../sm-oa-seals';


export default {
  name: 'SmOaSealsSelectModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: [String, Array], default: null },//已选项
    personal:{ type: Boolean, default: true },//个人模式，只能选择属于自己的签章
    visible: { type: Boolean, default: false },
  },
  data() {
    return {
      selectedSeals: [],
      iVisible: false,
    };
  },

  computed: {
    tags() {
      return this.selectedSeals;
    },
  },

  watch: {
    value: {
      handler: function (value) {
        this.selectedSeals = value;
      },
      immediate: true,
    },
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        //this.selectedSeals = this.value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedSeals);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedSeals = [];
    },
  },

  render() {
    return (
      <a-modal
        width={1000}
        title="签章选择"
        class="sm-basic-selectedSeals-modal"
        visible={this.iVisible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="sm-basic-selectedSeals-modal">
          <div class="selected">
            {this.tags && this.tags.length > 0 ? (
              this.tags.map(item => {
                return <div class="selected-item">
                  <a-icon style={{ color: '#f4222d' }} type={'bank'} />
                  <span class="selected-name"> {item ? item.name : null} </span>
                  <span
                    class="btn-close"
                    onClick={() => {
                      this.selectedSeals = this.selectedSeals.filter(_item => _item.id !== item.id);
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
          <div class="selectedSeals-list">
            <SmOaSeals
              axios={this.axios}
              selected={this.selectedSeals}
              multiple={false}
              isSelect={true}
              personal={this.personal}
              onChange={selected => {
                this.selectedSeals = [];
                this.selectedSeals = selected;
              }}
            />
          </div>
        </div>
      </a-modal>
    );
  },
};
