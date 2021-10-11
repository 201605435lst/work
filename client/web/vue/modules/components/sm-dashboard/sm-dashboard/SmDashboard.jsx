
import './style/index.less';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import './style/index.less';
import { resourceIcon } from '../../sm-file/sm-file-manage/src/common';
import SmDashboardListCard from '../sm-dashboard-list-card';
import moment from 'moment';
import ApiArticle from '../../sm-api/sm-cms/Article';

let apiArticle = new ApiArticle();

export default {
  name: 'SmDashboard',
  props: {
    axios: { type: Function, default: null },
    title: { type: String, default: "卡片标题" }, // 卡片标题 
    icon: { type: String, default: null }, // 卡片标题图标
    extra: { type: String, default: null }, // 卡片右上角操作区域
    theme: { type: String, default: 'default' }, // 列表主题，如“table”，“thumb”，“default”
  },
  data() {
    return {
      action: '/api/app/cmsArticle/getList',
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '文章标题',
          dataIndex: 'title',
          ellipsis: true,
          width: 100,
        },
        {
          title: '创建时间',
          dataIndex: 'date',
          customRender: (text, record, index) => {
            return moment(text).format('YYYY-MM-DD HH:mm:ss');
          },
          width: 120,
          ellipsis: true,
        },
        {
          title: '详情',
          dataIndex: 'summary',
          ellipsis: true,
        },
      ];
    },

    columnsOfMessage() {
      return [
        {
          title: '文章标题',
          dataIndex: 'title',
          ellipsis: true,
          width: 100,
        },
        {
          title: '创建时间',
          dataIndex: 'date',
          customRender: (text, record, index) => {
            return moment(text).format('YYYY-MM-DD HH:mm:ss');
          },
          ellipsis: true,
        },
      ];
    },
  },

  watch: {},

  async created() {
    // this.refresh();
  },
  methods: {
    async refresh() {
      apiArticle = new ApiArticle(this.axios);
      this.actionOfNews = apiArticle.getList({ categoryIds: ['f2c6bd91-05e1-4a63-8908-d651cf499cb4'], skipCount: 0, maxResultCount: 10 });
      this.actionOfInfo = apiArticle.getList({ categoryIds: ['73f4da0a-1f87-4098-805b-3aa159889af9'], skipCount: 0, maxResultCount: 10 });
      this.actionOfMessage = apiArticle.getList({ categoryIds: ['cbf2acfa-f2d5-4979-8582-05944427fbf0'] });
    },

    getDataArray(response) {
      if (requestIsSuccess(response)) {
        if (response.data instanceof Array) {
          this.dataSource = response.data;
        } else {
          this.dataSource = response.data.items;
        }
      }
    },

    getItemContent(item, column, index) {
      let result = '';
      if (item.hasOwnProperty(column.dataIndex)) {
        if (column.hasOwnProperty('customRender')) {
          result = column.customRender(item[column.dataIndex], item, index);
        } else {
          result = item[column.dataIndex];
        }
      }
      return result;
    },

    getThumb(data) {
      let url = null;
      let thumbArray = this.thumbUrl.split('.');
      if (thumbArray.length > 0) {
        let newData = data;
        for (let item of thumbArray) {
          if (newData.hasOwnProperty(item)) {
            if (item === 'url') {
              url = getFileUrl(newData[item]);
            } else {
              newData = newData[item];
            }
          }
        }
      }
      let typeArray = ['.png', '.jpg', '.img', '.gif', '.tif'];
      let result = null;
      typeArray.map(item => {
        if (url && url.indexOf(item)) {
          result = < img class="img-item" src={url} alt="图片" />;
        } else {
          result = <a-icon type={resourceIcon.unknown} />;
        }
      });

      return result;
    },
  },
  render() {
    return (
      <div class="sm-dashboard">
        <SmDashboardListCard
          axios={this.axios}
          action={this.action}
          columns={this.columns}
          theme="default.thumb"
          style="flex:1; margin-right:10px;"
          title="新闻"
          categoryCode="news"
        >
          <template slot="icon">
            <a-icon type="dribbble" />
          </template>
          <template slot="extra">
            <a>更多</a>
          </template>
        </SmDashboardListCard>

        <SmDashboardListCard
          axios={this.axios}
          action={this.action}
          columns={this.columns}
          theme="default"
          style="flex:1;margin-right:10px;"
          title="资讯"
          categoryCode="informations"
        >
          <template slot="icon">
            <a-icon type="weibo" />
          </template>
          <template slot="extra">
            <a>更多</a>
          </template>
        </SmDashboardListCard>

        <SmDashboardListCard
          axios={this.axios}
          action={this.action}
          columns={this.columnsOfMessage}
          theme="simple"
          style="width:310px;"
          title="通知"
          categoryCode="message"
        >
          <template slot="icon">
            <a-icon type="aliwangwang" />
          </template>
          <template slot="extra">
            <a>更多</a>
          </template>
        </SmDashboardListCard>

      </div>
    );
  },
};
