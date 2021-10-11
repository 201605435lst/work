import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import ApiCategory from '../../sm-api/sm-cms/Category';
import moment from 'moment';
import './style/index.less';
import permissionsSmCms from '../../_permissions/sm-cms';
let apiCategory = new ApiCategory();

export default {
  name: 'SmCmsCategory',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    action: { type: [String, Function, Object, Promise], default: null }, // 获取数据的地址或者执行方法
    categoryCode: { type: String, default: null }, // 当前栏目的标识
    titleWidth: { type: Number, default: null }, // 标题宽度
    summaryWidth: { type: Number, default: null }, // 简介宽度
  },
  data() {
    return {
      articles: [], // 当前栏目所属文章
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        categoryCode: null, //栏目
        maxResultCount: paginationConfig.defaultPageSize,
      },
      categoryTitle: '',//栏目标题
    };
  },
  computed: {

  },
  watch: {
    action: {
      handler: function (val, nVal) {
        if (val) {
          this.initAxios();
          this.queryParams.categoryCode = this.categoryCode;
          this.refresh();
        }
      },
      immediate: true,
    },
    id: {
      handler: function (val, nVal) {
        if (val) {
          this.queryParams.categoryCode = this.categoryCode;
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.queryParams.categoryCode = this.categoryCode;
    this.refresh();
  },

  methods: {
    initAxios() {
      apiCategory = new ApiCategory(this.axios);
    },

    async refresh() {
      if (!this.action) return;
      let response = null;
      let params = {
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      if (typeof (this.action) === 'string') {
        response = await this.axios({
          url: `${this.action}`,
          method: 'get',
          params: { ...params },
        });
        this.getDataArray(response);
      } else {
        this.action.then(res => {
          response = res;
          this.getDataArray(response);
        });
      }
      if (this.categoryCode) {
        let categoryResponse = await apiCategory.getByCode(this.categoryCode);
        if (requestIsSuccess(categoryResponse)) {
          this.categoryTitle = categoryResponse.data.title;
        }
      }
    },

    getDataArray(response) {
      if (requestIsSuccess(response) && response.data) {
        if (response.data instanceof Array) {
          this.articles = response.data;
          this.totalCount = response.data.length;
        } else {
          this.articles = response.data.items;
          this.totalCount = response.data.totalCount;
        }
      }
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },

  },
  render() {
    return (
      <div class="sm-cms-category">
        <div class="category-title">
          <span>{this.categoryTitle}</span>
        </div>
        <div class="content">
          <a-row>
            {this.articles.length > 0 ?
              this.articles.map((item, index) => {
                return <div class="item">
                  {item.thumb ? <div class="left">
                    <div class="thumb-box">
                      <div class="thumb">
                        <img class="img" src={getFileUrl(item.thumb.url)} />
                      </div>
                    </div>
                    <div class="info">
                      <div class="title"
                        style={this.titleWidth ? {
                          "width": `${this.titleWidth}px`,
                          'overflow': 'hidden',
                          'white-space': 'nowrap',
                          'text-overflow': 'ellipsis',
                        } : {}}
                        onClick={() => {
                          this.$emit('preview', item.id);
                        }}>
                        <span>{item.title}</span>
                      </div>
                      <div
                        class="summary"
                        style={this.summaryWidth ? {
                          "width": `${this.summaryWidth}px`,
                          'overflow': 'hidden',
                          'white-space': 'nowrap',
                          'text-overflow': 'ellipsis',
                        } : {}}
                      >{item.summary}</div>
                    </div>
                  </div> : undefined}

                  <div class="right">
                    <span>{moment(item.date).format('YYYY-MM-DD HH:mm:ss')}</span>
                  </div>
                </div>;
              })
              : <a-empty />}
            {/* 分页器 */}

            < a-pagination
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

          </a-row>
        </div>
        <div class="back-button">
          <a-button style="margin-right:10px;" onClick={() => {
            this.$emit('back');
          }}>返回</a-button>
        </div>

      </div>
    );
  },
};
