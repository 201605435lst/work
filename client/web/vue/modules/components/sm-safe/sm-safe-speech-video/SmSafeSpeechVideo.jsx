import './style/index';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import ApiDiary from '../../sm-api/sm-schedule/Diary';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmVideo from '../../sm-common/sm-video/SmVideo';
import SmVideoBase from '../../sm-common/sm-video/src/SmVideoBase';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
import moment from 'moment';
let apiDiary = new ApiDiary();

export default {
  name: 'SmSafeSpeechVideo',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      pageIndex: 1,
      queryParams: {
        keuWords: null,
        startTime: null,
        endTime: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '施工部位',
          dataIndex: 'location',
          ellipsis: true,
          scopedSlots: { customRender: 'location' },
        },
        {
          title: '施工内容',
          ellipsis: true,
          dataIndex: 'schedule',
          scopedSlots: { customRender: 'schedule' },
        },
        {
          title: '施工日期',
          ellipsis: true,
          dataIndex: 'date',
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '视频',
          dataIndex: 'video',
          width: 400,
          ellipsis: true,
          scopedSlots: { customRender: 'video' },
        },
        {
          title: '缩略图',
          dataIndex: 'thumb',
          ellipsis: true,
          scopedSlots: { customRender: 'thumb' },
        },
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
    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      let videoTypes = ['.avi', '.mov', '.rmvb', '.rm', '.flv', '.mp4', '.3gp', '.mpeg', '.mpg'];
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else if (videoTypes.includes(file.type)) {
        this.$refs.SmVideo.preview(true, getFileUrl(file.url), file.name);
      } else {
        this.$message.warning('当前文件不支持预览');
      }
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        startTime: this.queryParams.startTime ? this.queryParams.startTime.format('YYYY-MM') : '',
        endTime: this.queryParams.endTime ? this.queryParams.endTime.format('YYYY-MM') : '',
      };
      let response = await apiDiary.getSpeachVideo({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });

      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
      this.loading = false;
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    video(files) {
      return (files && files.map(item => {
        if (item.file) {
          let name = item.file.name > 8 ? `${item.file.name.substring(0, 8)}...${item.file.type}` : item.file.name + item.file.type;
          return <span class="video">
            <a-tooltip placement="topLeft" title={name}>
              <a
                onClick={() => {
                  this.play(item.file);
                }}
              >
                {name}
              </a>
            </a-tooltip>

          </span>;
        }
      }));
    },
    thumb(files) {
      return (files && files.map(item => {
        let url= getFileUrl(item.file.url);
        if (item.file) {
          return (
            <span class="thumb">
              <a
                onClick={() => {
                  this.play(item.file);
                }}
              >
                <video class="thumb-picture" >
                  <source src={url} controls="false" />
                </video>
              </a>

          
            </span>
          );
        }
      }));
    },
  },
  render() {
    return (
      <div class="sm-safe-speech-video">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.endTime = null;
            this.queryParams.startTime = null;
            this.queryParams.keuWords = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear={true}
              placeholder="请输入施工内容和施工部位"
              value={this.queryParams.keuWords}
              onInput={event => {
                this.queryParams.keuWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <div class="sm-safe-speech-video-date">
            <a-form-item label="日期选择">
              <a-month-picker
                style="width: 100%;"
                placeholder="开始时间"
                allowClear={true}
                value={this.queryParams.startTime}
                onChange={value => {
                  this.queryParams.startTime = value;
                  this.refresh();
                }}
              />
              <span class="month-picker_">-</span>
              <a-month-picker
                style="width: 100%;"
                allowClear={true}
                value={this.queryParams.endTime}
                onChange={value => {
                  this.queryParams.endTime = value;
                  this.refresh();
                }}
                placeholder="结束时间"
              />
            </a-form-item>
          </div>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              location: (text, record) => {
                let name = record.location ? record.location : '';
                return (
                  <a-tooltip placement="topLeft" title={name}>
                    <span>{name}</span>
                  </a-tooltip>
                );
              },
              schedule: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.schedule}>
                    <span>{record.schedule}</span>
                  </a-tooltip>
                );
              },
              date: (text, record) => {
                console.log(record);
                let result = record.date ? moment(record.date).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },

              video: (text, record) => {
                return (
                  <span class="video-talk-media">
                    {this.video(record.talkMedias)}
                  </span>
                );
              },
              thumb: (text, record) => {
                return (
                  <span class="thumb-talk-media">
                    {this.thumb(record.talkMedias)}
                  </span>
                );
              },
            },
          }

          }
        ></a-table>
        <SmVideo axios={this.axios} ref="SmVideo" />
        {/* 图片类预览组件 */}
        <SmFileImageView axios={this.axios} ref="SmFileImageView" />
        {/* 文档浏览组件 */}
        <SmFileDocumentView axios={this.axios} ref="SmFileDocumentView" />
        {/* 分页器 */}
        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
      </div>
    );
  },
};
