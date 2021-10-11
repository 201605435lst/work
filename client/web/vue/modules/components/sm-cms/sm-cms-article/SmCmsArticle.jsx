import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import moment from 'moment';
import './style/index.less';
import permissionsSmCms from '../../_permissions/sm-cms';

export default {
  name: 'SmCmsArticle',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    action: { type: [String, Function, Object, Promise], default: null }, // 获取数据的地址或者执行方法
    id: { type: String, default: null }, // 当前数据源id
  },
  data() {
    return {
      article: null, // 文章数据详情
      fileServerEndPoint: '', //文件服务请求头
    };
  },
  computed: {

  },
  watch: {
    action: {
      handler: function (val, nVal) {
        if (val) {
          this.refresh();
        }
      },
      immediate: true,
    },
    id: {
      handler: function (val, nVal) {
        if (val) {
          this.refresh();
        }
      },
      immediate: true,
    },
  },
  async created() {
    this.refresh();
    this.fileServerEndPoint = localStorage.getItem('fileServerEndPoint');
  },

  methods: {
    async refresh() {
      if (!this.action || !this.id) return;
      let response = null;
      if (typeof (this.action) === 'string') {
        response = await this.axios({
          url: `${this.action}`,
          method: 'get',
          params: { id: this.id },
        });
      } else {
        this.action.then(res => {
          response = res;
        });
      }
      if (response && requestIsSuccess(response)) {
        this.article = response.data;
        this.article.content = response.data.content.replace(
          new RegExp(`src="`, 'g'),
          `src="${this.fileServerEndPoint}`,
        );
      }
    },

  },
  render() {
    let categories = '';
    if (this.article && this.article.categories && this.article.categories.length > 0) {
      this.article.categories.map((item, index) => {
        categories += `${item.categoryTitle}${index != this.article.categories.length - 1 ? '、' : ''}`;
      });
    }

    return (
      <div class="sm-cms-article">
        {this.article ? <div class="article">
          <div class="article-header">
            <div class="h-title">{this.article.title}</div>
            <div class="h-info">
              <span class="h-time"
              >发表时间：{
                  moment(this.article.date).format("YYYY-MM-DD HH:MM")
                }</span>
              <span>来源：{categories} </span>
            </div>
          </div>
          <div class="body">
            <div class="left">
              <div
                class="content"
                {...{
                  domProps: {
                    innerHTML: this.article.content,
                  },
                }}>
              </div>
              <div class="author">责任编辑：{this.article ? this.article.author : ''}</div>
            </div>

            {/* <div class="right">
              <div class="imgs">
                {
                  this.article && this.article.carousels && this.article.carousels.length > 0 ?
                    this.article.carousels.map(item => {
                      return <div class="img-box">
                        <div class="img-item">
                          {item.file ? <img class="img" src={getFileUrl(item.file.url)} /> : undefined}
                        </div>
                      </div>;
                    })
                    : undefined
                }
              </div>
             
            </div> */}
          </div>
        </div> : undefined}
        <div class="back">
          <a-button onClick={() => {
            this.$emit('back');
          }}>返回</a-button>
        </div>
      </div >
    );
  },
};
