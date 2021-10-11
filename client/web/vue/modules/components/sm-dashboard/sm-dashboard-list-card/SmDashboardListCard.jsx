
import './style';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import './style/index.less';
import { resourceIcon } from '../../sm-file/sm-file-manage/src/common';
let apiEntity = new ApiEntity();

export default {
  name: 'SmDashboardListCard',
  props: {
    axios: { type: Function, default: null },
    action: { type: [String, Function, Object, Promise], default: null }, // 获取数据的地址或者执行方法
    title: { type: String, default: "卡片标题" }, // 卡片标题 
    icon: { type: String, default: null }, // 卡片标题图标
    extra: { type: String, default: null }, // 卡片右上角操作区域
    rowKey: { type: String, default: 'id' }, // 数据rowKey，如“id”等
    theme: { type: String, default: 'default' }, // 列表主题，如“table”，“thumb”，“default”
    size: { type: String, default: 'default' }, // 卡片尺寸
    height: { type: Number, default: 300 }, // 卡片内容最大高度
    skipCount: { type: Number, default: 0 }, // 请求数据跳过的条数
    maxListCount: { type: Number, default: 10 }, // 卡片最多显示条数
    categoryCode: { type: String, default: null }, // 卡片最多显示条数
    thumbUrl: { type: String, default: 'thumb.url' }, // 卡片样式
    columns: { type: Array, default: () => [] }, // 显示的字段配置，如
  },
  data() {
    return {
      dataSource: [], //数据源
    };
  },
  computed: {},
  watch: {
    action: {
      handler: function (val, nVal) {
        if (val) {
          this.refresh();
        }
      },
      immediate: true,
    },
  },
  async created() {
    // this.refresh();
  },
  methods: {
    async refresh() {
      if (!this.action) return;
      let response = null;
      let params = {
        skipCount: this.skipCount,
        maxResultCount: this.maxListCount,
        categoryCode: this.categoryCode,
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
          if (newData) {
            if (newData.hasOwnProperty(item)) {
              if (item === 'url') {
                url = getFileUrl(newData[item]);
              } else {
                newData = newData[item];
              }
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
    let list = null;
    if (this.theme === 'simple') {
      list = this.dataSource.map((item, index) => {
        return <div
          class="simple"
          style={{
            'padding-bottom': index !== this.dataSource.length - 1 ? '10px' : '',
          }}>
          {this.columns && this.columns.length > 0 ? this.columns.map((columnItem, subIndex) => {
            return <div
              class={{ 'item': true, 'ellipsis': columnItem.ellipsis }}
              style={{
                "padding-bottom": index !== this.dataSource.length - 1 ? '20px;' : '',
                'width': columnItem.width ? `${columnItem.width}px` : '',
                'flex': columnItem.width ? '' : 1,
                'margin-right': subIndex !== this.columns.length - 1 ? '24px' : '',
                'text-align': columnItem['align'] ? columnItem['align'] : 'left',
              }}
              title={columnItem.ellipsis ? this.getItemContent(item, columnItem, index) : null}
              onClick={event => {
                this.$emit('click', event, index, item.id, item);
              }}
            >
              <span style="cursor: pointer;">{this.getItemContent(item, columnItem, index)}</span>
            </div>;
          }) : undefined
          }
        </div >;
      });
    } else {
      list = this.dataSource.map((item, index) => {

        return <div class="default">
          {this.columns && this.columns.length > 0 ?
            <div
              class="item"
              style={index !== this.dataSource.length - 1 ? 'padding-bottom: 20px;' : ''}
            >
              {this.theme === 'default.thumb' ? <div class="thumb"
                onClick={event => {
                  this.$emit('click', event, index, item.id, item);
                }}>
                <div class="img-box">
                  {this.getThumb(item)}
                </div>
              </div>
                : undefined}
              <div
                class="info"
                onClick={event => {
                  this.$emit('click', event, index, item.id, item);
                }}>
                <div class="title">
                  <div class="left">
                    <div class="left-title">{this.getItemContent(item, this.columns[0], index)}</div>
                  </div>
                  <div class="right">{this.getItemContent(item, this.columns[1], index)}</div>
                </div>
                <div class="detail">{this.getItemContent(item, this.columns[2], index)}</div></div>

            </div>
            : undefined
          }
        </div >;
      });
    }
    return (
      <div class="sm-dashboard-list-card">
        <a-card size={this.size} >
          <template slot="title">
            {this.$slots.icon ? <span class="icon" style="margin-right:10px;">{this.$slots.icon}</span> : this.icon}
            <span title={this.title}>{this.$slots.title || this.title}</span>
          </template>
          <template slot="extra" style="display:flex;">
            {this.$slots.extra ? this.$slots.extra :
              <a
                onClick={() => {
                  this.$emit('more');
                }}
              >
                {this.extra}
              </a>
            }
          </template>
          <div class="card-body" style={{ 'height': `${this.height}px` }}>
            {this.dataSource && this.dataSource.length > 0 ?
              list :
              <div class="empty">无数据</div>}
          </div>
        </a-card>
      </div>
    );
  },
};
