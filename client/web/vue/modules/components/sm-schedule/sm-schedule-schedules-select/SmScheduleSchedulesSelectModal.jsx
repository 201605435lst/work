import './style/index.less';
import Schedules from '../sm-schedule-schedules';

export default {
  name: 'SmScheduleSchedulesModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    value: { type: Array, default: () => [] },//已选项
    noId: { type: String, default: '' },
    placeholder: { type: String, default: '请点击选择任务' },
    multiple: { type: Boolean, default: false }, // 是否多选
  },
  data() {
    return {
      selectedSchedules: [], //已选任务
      iValue: null,   //已选id
      iVisible: false,
    };
  },

  watch: {
    value: {
      handler: function (value, oldvalue) {
        this.selectedSchedules = value;
        if(value){
          this.iValue = value.map(item => item.id);
        }
      },
      immediate: true,
    },

    visible: {
      handler: function (value, oldValue) {
        this.selectedSchedules = this.value;
        this.iVisible = value;
      },
      immediate: true,
    },
  },

  methods: {
    onOk() {
      this.$emit('ok', this.selectedSchedules);
      this.onClose();
    },
    onClose() {
      this.$emit('change', false);
      this.selectedSchedules = [];
    },
  },
  render() {
    return (
      <a-modal
        width={1000}
        title="计划选择"
        class="sm-schedule-schedules-select-modal"
        visible={this.visible}
        onOk={this.onOk}
        onCancel={this.onClose}
      >
        <div class="selected">
          {this.selectedSchedules && this.selectedSchedules.length > 0 ? (
            this.selectedSchedules.map(item => {
              return <div class="selected-item">
                <a-icon style={{ color: '#f4222d' }} type={'inbox'} />
                <span class="selected-name"> {item ? item.name : null} </span>
                <span
                  class="btn-close"
                  onClick={() => {
                    this.iValue = this.iValue.filter(id => id !== item.id);
                    this.selectedSchedules = this.selectedSchedules.filter(_item => _item.id !== item.id);
                  }}
                >
                  <a-icon type="close" />
                </span>
              </div>;
            })

          ) : (
            <span style="margin-left:10px;">{this.placeholder}</span>
          )}
        </div>
        <Schedules
          axios={this.axios}
          selected={this.selectedSchedules}
          multiple={this.multiple}
          isSimple={true}
          noId={this.noId}
          onChange={iSelected => (this.selectedSchedules = iSelected)}
          onInput={iValue => (this.iValue = iValue)}
        >
        </Schedules>
      </a-modal>
    );
  },
};
