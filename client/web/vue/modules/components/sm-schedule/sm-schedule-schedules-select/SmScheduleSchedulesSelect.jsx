// 文件选择对话框
import SmScheduleSchedulesSelectModal from './SmScheduleSchedulesSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiSchedule from '../../sm-api/sm-schedule/Schedule';
import { PageState } from '../../_utils/enum';
import './style/index.less';

let apiSchedule = new ApiSchedule();

export default {
  name: 'SmScheduleSchedulesSelect',

  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    height: { type: Number, default: 100 }, // 当前选择框的大小
    margin: { type: Array }, //当前选择框位置
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容
    placeholder: { type: String, default: '请点击选择任务' },
    multiple: { type: Boolean, default: false }, //是否多选，默认多选
    bordered: { type: Boolean, default: true }, // 边框模式
    noId: { type: String, default: '' }, //选择前置任务时需要排出的计划
    pageState: { type: String, default: PageState.Add }, //页面状态
  },

  data() {
    return {
      id: null,
      iVisible: false,
      selectedSchedules: [], //已选择设备
      iValue:[],
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedSchedules;
    },
  },
  watch: {
    value: {
      handler: function (nVal, oVal) {
        if (this.multiple) {
          this.iValue = nVal;
        } else {
          this.iValue = [nVal];
        }
        this.initSchedule();
      },
      immediate: true,
    },
    pageState: {
      handler: function (nVal, oVal) {
        if (nVal) {
          this.pageState = nVal;
        }
      },
      immediate: true,
    },
  },

  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiSchedule = new ApiSchedule(this.axios);
    },

    scheduleSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选任务初始化
    async initSchedule() {
      let _selectedSchedules = [];
      if (this.iValue && this.iValue.length > 0 && this.multiple) {
        let response = await apiSchedule.getByIds(this.iValue);

        if (requestIsSuccess(response)) {
          _selectedSchedules = response.data.items.map(item => item);
        }
      } else {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiSchedule.getByIds(id);
            if (requestIsSuccess(response)) {
              _selectedSchedules.push(response.data.items[0]);
            }
          }
        });
      }
      this.selectedSchedules = _selectedSchedules;
    },

    selected(data) {
      this.iValue = data.map(item => item.id);
      this.selectedSchedules = data;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedSchedules && this.selectedSchedules.length > 0
            ? this.selectedSchedules.map(item => item.id)
            : [],
        );
        this.$emit('input', this.selectedSchedules);
      } else {
        this.$emit(
          'change',
          this.selectedSchedules[0] ? this.selectedSchedules[0].id : null,
        );
        this.$emit('input', this.selectedSchedules);
      }
    },
  },
  render() {
    let schedules = this.tags.map(item => {
      return (
        <div class="selected-item">
          <a-icon style={'color: #f4222d;margin-top:4px'} type={'inbox'} />
          <span class="selected-name">{item ? item.name : null} </span>
          {!this.disabled ? (
            <span
              class="btn-close"
              onClick={e => {
                e.stopPropagation();
                this.iValue = this.iValue.filter(id => id !== item.id);
                this.selectedSchedules = this.selectedSchedules.filter(
                  _item => _item.id !== item.id,
                );
                if(this.multiple){ //多选[]和单选null，不分开会出现删掉之后又自动选的bug
                  this.$emit(
                    'change',
                    this.iValue && this.iValue.length > 0 ?this.iValue.map(item => item) : [],
                  );
                  this.$emit('input', this.iValue && this.iValue.length > 0 ? this.iValue : []);
                }else{
                  this.$emit(
                    'change',
                    this.iValue[0] ? this.iValue[0].id : null,
                  );
                  this.$emit('input', this.iValue[0] ? this.iValue[0] : null);
                }
                
              }}
            >
              <a-icon type="close" />
            </span>
          ) : (
            undefined
          )}
        </div>
      );
    });

    return (
      <div
        class={{
          'schedule-select-panel': true,
          'ant-input': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        onClick={() => this.scheduleSelect()}
        onMouseEnter={() => this.enter()}
        style={{
          height: this.bordered ? this.height + 'px' : 'auto',
          margin: this.margin ? this.margin[0] + 'px '+this.margin[1] + 'px '+this.margin[2] + 'px '+this.margin[3] + 'px' : 'auto',
        }}
      >
        {this.tags.length == 0 ? <label class="tip">{this.placeholder}</label> : ''}

        <div class="selectedSchedule">{schedules} </div>
        <div style={' align-self:center;padding-right: 10px; '}>
          {schedules.length > 0 && !this.disabled ? (
            <a-icon
              class="btn-close-circle"
              onClick={e => {
                e.stopPropagation();
                this.selectedSchedules = [];
                this.$emit(
                  'change',
                  this.multiple ? [] : null,
                );
                this.$emit('input', this.multiple ? [] : null);
               
              }}
              theme="filled"
              type="close-circle"
            />
          ) : (
            undefined
          )}
        </div>

        {/* 前置任务选择模态框 */}
        <SmScheduleSchedulesSelectModal
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedSchedules}
          multiple={this.multiple}
          noId={this.noId}
          placeholder={this.placeholder}
          onOk={this.selected}
          onChange={v => (this.iVisible = v)}
        />
      </div>
    );
  },
};
