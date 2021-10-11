import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiDiary from '../../sm-api/sm-schedule/Diary';

let apiDiary = new ApiDiary();
import moment from 'moment';
export default {
  name: 'SmScheduleDiarysStatistics',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      data: [],
      loading: false,
      endTime: moment(),
      sumDay: 0, //总天数
      sumLog: 0, //填报日志总天数
      startTime: moment().subtract(11, 'months'),
    };
  },
  computed: {
    columns() {
      let arr = [];
      for (let i = 1; i <= 31; i++) {
        arr.push({
          title: i,
          dataIndex: `col_${i}`,
          fixed: i == 31 ? 'right' : '',
        });
      }
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          width: 80,
          fixed: 'left',
        },
        {
          title: '年-月',
          dataIndex: 'data',
          scopedSlots: { customRender: 'data' },
          width: 90,
          fixed: 'left',
        },
        ...arr,
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiDiary = new ApiDiary(this.axios);
    },
    async refresh() {
      let data = {
        startTime: this.startTime.format('YYYY-MM'),
        endTime: this.endTime.format('YYYY-MM'),
      };
      let response = await apiDiary.getLogStatistics(data);
      if (requestIsSuccess(response)) {
        let _data = [];
        response &&
          response.data.map((item, index) => {
            let diary = new Object();
            diary.dateTime = item && item.dateTime ? item.dateTime : '';
            diary.key = index;
            this.sumDay += item.sumDay;
            this.sumLog += item.sumLog;
            for (let i = 0; i <= item.diaryLogDayStatisticsDto.length - 1; i++) {
              let col = `col_${i + 1}`;
              diary[col] = item.diaryLogDayStatisticsDto[i].hasDiary ? (
                <div>
                  <si-seleted style="color: #1890ff;font-size: 22px;" />
                </div>
              ) : (
                <div>
                  <si-close style="color: red;font-size: 18px;" />
                </div>
              );
            }
            _data.push(diary);
          });
        this.data = _data;
      }
    },
    cancel() {
      this.$emit('cancel');
      this.form.resetFields();
    },
  },
  render() {
    return (
      <div class="sm-schedule-diarys-statistics">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.endTime == moment(),
            (this.startTime = moment().subtract(11, 'months')),
            this.refresh();
          }}
        >
          <a-form-item label="日期选择">
            <a-month-picker
              placeholder="开始时间"
              allowClear={false}
              value={this.startTime}
              onChange={value => {
                this.startTime = value;
                this.refresh();
              }}
            />
            <span class="month-picker_">-</span>
            <a-month-picker
              allowClear={false}
              value={this.endTime}
              onChange={value => {
                this.endTime = value;
                this.refresh();
              }}
              placeholder="结束时间"
            />
          </a-form-item>
        </sc-table-operator>
        <div class="diarys-statistics-view">
          {`总天数：${this.sumDay} 天, 日志填报：${this.sumLog} 天`}
        </div>
        <div class="diarys-statistics-table">
          <a-table
            columns={this.columns}
            dataSource={this.data}
            loading={this.loading}
            pagination={false}
            bordered={true}
            scroll={{ x: 1705, y: 500 }}
            rowKey={record => record.key}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1;
                },
                data: (text, record, index) => {
                  return moment(record.dateTime).format('YYYY-MM');
                },
              },
            }}
          ></a-table>
          <a-button type="primary"
            onClick={() => this.cancel()}
          >返回</a-button>
        </div>
      </div>
    );
  },
};
